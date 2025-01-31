﻿using UnityEngine;
using Cinemachine;
using System.Linq;
using System.Collections.Generic;

namespace GamerWolf.Utils {
	[DefaultExecutionOrder(0)]
	[ExecuteInEditMode]
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	public class CameraMultiTarget : MonoBehaviour {
		public float Pitch;
		public float Yaw;
		public float Roll;
		public float PaddingLeft;
		public float PaddingRight;
		public float PaddingUp;
		public float PaddingDown;
		public float MoveSmoothTime = 0.19f;

		private CinemachineVirtualCamera _camera;
		private List<Transform> _targets = new List<Transform>();
		private DebugProjection _debugProjection;

		enum DebugProjection { DISABLE, IDENTITY, ROTATED }
		enum ProjectionEdgeHits { TOP_BOTTOM, LEFT_RIGHT }

		public static CameraMultiTarget current{get;private set;}
		
		
		private void Awake(){
			current = this;
			_camera = gameObject.GetComponent<CinemachineVirtualCamera>();
			_debugProjection = DebugProjection.ROTATED;
		}
		private void LateUpdate() {
			if (_targets.Count == 0){
				return;
			}
			
			var targetPositionAndRotation = TargetPositionAndRotation(_targets.ToArray());

			Vector3 velocity = Vector3.zero;
			transform.position = Vector3.SmoothDamp(transform.position, targetPositionAndRotation.Position, ref velocity, MoveSmoothTime);
			transform.rotation = targetPositionAndRotation.Rotation;
		}
		
		private PositionAndRotation TargetPositionAndRotation(Transform[] targets){
			float halfVerticalFovRad = (_camera.m_Lens.FieldOfView * Mathf.Deg2Rad) / 2f;
			float halfHorizontalFovRad = Mathf.Atan(Mathf.Tan(halfVerticalFovRad) * _camera.m_Lens.Aspect);

			var rotation = Quaternion.Euler(Pitch, Yaw, Roll);
			var inverseRotation = Quaternion.Inverse(rotation);

			var targetsRotatedToCameraIdentity = targets.Select(target => inverseRotation * target.transform.position).ToArray();

			float furthestPointDistanceFromCamera = targetsRotatedToCameraIdentity.Max(target => target.z);
			float projectionPlaneZ = furthestPointDistanceFromCamera + 3f;

			ProjectionHits viewProjectionLeftAndRightEdgeHits = 
				ViewProjectionEdgeHits(targetsRotatedToCameraIdentity, ProjectionEdgeHits.LEFT_RIGHT, projectionPlaneZ, halfHorizontalFovRad).AddPadding(PaddingRight, PaddingLeft);
			ProjectionHits viewProjectionTopAndBottomEdgeHits = 
				ViewProjectionEdgeHits(targetsRotatedToCameraIdentity, ProjectionEdgeHits.TOP_BOTTOM, projectionPlaneZ, halfVerticalFovRad).AddPadding(PaddingUp, PaddingDown);
			
			var requiredCameraPerpedicularDistanceFromProjectionPlane =
				Mathf.Max(
					RequiredCameraPerpedicularDistanceFromProjectionPlane(viewProjectionTopAndBottomEdgeHits, halfVerticalFovRad),
					RequiredCameraPerpedicularDistanceFromProjectionPlane(viewProjectionLeftAndRightEdgeHits, halfHorizontalFovRad)
			);

			Vector3 cameraPositionIdentity = new Vector3(
				(viewProjectionLeftAndRightEdgeHits.Max + viewProjectionLeftAndRightEdgeHits.Min) / 2f,
				(viewProjectionTopAndBottomEdgeHits.Max + viewProjectionTopAndBottomEdgeHits.Min) / 2f,
				projectionPlaneZ - requiredCameraPerpedicularDistanceFromProjectionPlane);

			DebugDrawProjectionRays(cameraPositionIdentity, 
				viewProjectionLeftAndRightEdgeHits, 
				viewProjectionTopAndBottomEdgeHits, 
				requiredCameraPerpedicularDistanceFromProjectionPlane, 
				targetsRotatedToCameraIdentity, 
				projectionPlaneZ, 
				halfHorizontalFovRad, 
				halfVerticalFovRad);

			return new PositionAndRotation(rotation * cameraPositionIdentity, rotation);
		}

		private static float RequiredCameraPerpedicularDistanceFromProjectionPlane(ProjectionHits viewProjectionEdgeHits, float halfFovRad){
			float distanceBetweenEdgeProjectionHits = viewProjectionEdgeHits.Max - viewProjectionEdgeHits.Min;
			return (distanceBetweenEdgeProjectionHits / 2f) / Mathf.Tan(halfFovRad);
		}

		private ProjectionHits ViewProjectionEdgeHits(IEnumerable<Vector3> targetsRotatedToCameraIdentity, ProjectionEdgeHits alongAxis, float projectionPlaneZ, float halfFovRad){
			float[] projectionHits = targetsRotatedToCameraIdentity
				.SelectMany(target => TargetProjectionHits(target, alongAxis, projectionPlaneZ, halfFovRad))
				.ToArray();
			return new ProjectionHits(projectionHits.Max(), projectionHits.Min());
		}
		
		private float[] TargetProjectionHits(Vector3 target, ProjectionEdgeHits alongAxis, float projectionPlaneDistance, float halfFovRad){
			float distanceFromProjectionPlane = projectionPlaneDistance - target.z;
			float projectionHalfSpan = Mathf.Tan(halfFovRad) * distanceFromProjectionPlane;

			if (alongAxis == ProjectionEdgeHits.LEFT_RIGHT){
				return new[] {target.x + projectionHalfSpan, target.x - projectionHalfSpan};
			}
			else
			{
				return new[] {target.y + projectionHalfSpan, target.y - projectionHalfSpan};
			}
		
		}
		
		private void DebugDrawProjectionRays(Vector3 cameraPositionIdentity, ProjectionHits viewProjectionLeftAndRightEdgeHits,
			ProjectionHits viewProjectionTopAndBottomEdgeHits, float requiredCameraPerpedicularDistanceFromProjectionPlane,
			IEnumerable<Vector3> targetsRotatedToCameraIdentity, float projectionPlaneZ, float halfHorizontalFovRad,
			float halfVerticalFovRad) {
			
			if (_debugProjection == DebugProjection.DISABLE)
				return;
			
			DebugDrawProjectionRay(
				cameraPositionIdentity,
				new Vector3((viewProjectionLeftAndRightEdgeHits.Max - viewProjectionLeftAndRightEdgeHits.Min) / 2f,
					(viewProjectionTopAndBottomEdgeHits.Max - viewProjectionTopAndBottomEdgeHits.Min) / 2f,
					requiredCameraPerpedicularDistanceFromProjectionPlane), new Color32(31, 119, 180, 255));
			DebugDrawProjectionRay(
				cameraPositionIdentity,
				new Vector3((viewProjectionLeftAndRightEdgeHits.Max - viewProjectionLeftAndRightEdgeHits.Min) / 2f,
					-(viewProjectionTopAndBottomEdgeHits.Max - viewProjectionTopAndBottomEdgeHits.Min) / 2f,
					requiredCameraPerpedicularDistanceFromProjectionPlane), new Color32(31, 119, 180, 255));
			DebugDrawProjectionRay(
				cameraPositionIdentity,
				new Vector3(-(viewProjectionLeftAndRightEdgeHits.Max - viewProjectionLeftAndRightEdgeHits.Min) / 2f,
					(viewProjectionTopAndBottomEdgeHits.Max - viewProjectionTopAndBottomEdgeHits.Min) / 2f,
					requiredCameraPerpedicularDistanceFromProjectionPlane), new Color32(31, 119, 180, 255));
			DebugDrawProjectionRay(
				cameraPositionIdentity,
				new Vector3(-(viewProjectionLeftAndRightEdgeHits.Max - viewProjectionLeftAndRightEdgeHits.Min) / 2f,
					-(viewProjectionTopAndBottomEdgeHits.Max - viewProjectionTopAndBottomEdgeHits.Min) / 2f,
					requiredCameraPerpedicularDistanceFromProjectionPlane), new Color32(31, 119, 180, 255));

			foreach (var target in targetsRotatedToCameraIdentity) {
				float distanceFromProjectionPlane = projectionPlaneZ - target.z;
				float halfHorizontalProjectionVolumeCircumcircleDiameter = Mathf.Sin(Mathf.PI - ((Mathf.PI / 2f) + halfHorizontalFovRad)) / (distanceFromProjectionPlane);
				float projectionHalfHorizontalSpan = Mathf.Sin(halfHorizontalFovRad) / halfHorizontalProjectionVolumeCircumcircleDiameter;
				float halfVerticalProjectionVolumeCircumcircleDiameter = Mathf.Sin(Mathf.PI - ((Mathf.PI / 2f) + halfVerticalFovRad)) / (distanceFromProjectionPlane);
				float projectionHalfVerticalSpan = Mathf.Sin(halfVerticalFovRad) / halfVerticalProjectionVolumeCircumcircleDiameter;
				
				DebugDrawProjectionRay(target,
					new Vector3(projectionHalfHorizontalSpan, 0f, distanceFromProjectionPlane),
					new Color32(214, 39, 40, 255));
				DebugDrawProjectionRay(target,
					new Vector3(-projectionHalfHorizontalSpan, 0f, distanceFromProjectionPlane),
					new Color32(214, 39, 40, 255));
				DebugDrawProjectionRay(target,
					new Vector3(0f, projectionHalfVerticalSpan, distanceFromProjectionPlane),
					new Color32(214, 39, 40, 255));
				DebugDrawProjectionRay(target,
					new Vector3(0f, -projectionHalfVerticalSpan, distanceFromProjectionPlane),
					new Color32(214, 39, 40, 255));
			}
		}

		private void DebugDrawProjectionRay(Vector3 start, Vector3 direction, Color color){
			Quaternion rotation = _debugProjection == DebugProjection.IDENTITY ? Quaternion.identity : transform.rotation;
			Debug.DrawRay(rotation * start, rotation * direction, color);
		}
		public void SetCamraTargetingData(float _paddingDown,float _paddingUp){
			PaddingDown = _paddingDown;
			PaddingUp = _paddingUp;
		}
		public void SetTargets(Transform target) {
			if(!_targets.Contains(target)){
				_targets.Add(target);
			}
		}
		public void RemoveTarget(Transform _target){
			if(_targets.Contains(_target)){
				_targets.Remove(_target);
			}
		}


	}
}
