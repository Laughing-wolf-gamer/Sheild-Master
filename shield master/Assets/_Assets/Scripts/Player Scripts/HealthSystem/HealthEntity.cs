using System;
using UnityEngine;
namespace GamerWolf.Utils.HealthSystem {
    public class OnHitArgs : EventArgs{
        public Vector3 HitPoint;
    }
    public class HealthEntity : MonoBehaviour,IDamagable {
        
        [SerializeField] protected int maxHealth;
        [SerializeField] protected bool isDead;
        [SerializeField] protected int currentHealth;
        [SerializeField] protected bool canDie;
        [SerializeField] protected bool canHit;
        public event EventHandler onDead;
        public event EventHandler<OnHitArgs> OnHit;
        protected virtual void Awake(){
            ResetHealth();   
        }
        protected virtual void Start(){}
        public void ResetHealth(){
            Debug.Log("Health is Reset for " + transform.name);
            isDead = false;
            currentHealth = maxHealth;
            SetCanDie(true);
        }
        public void TakeHit(int damageValue){
            if(canDie){
                if(canHit){
                    currentHealth -= damageValue;
                    if(currentHealth <= 0 && !isDead){
                        currentHealth = 0;
                        Die();
                    }
                }
            }
        }
        protected virtual void Die(){
            isDead = true;
            Debug.Log(transform.name + " is Dead");
            onDead?.Invoke(this,EventArgs.Empty);
        }
        protected float GetHealthNormalized(){
            return (float)currentHealth/maxHealth;
        }
        public virtual void SetCanDie(bool _value){
            canDie = _value;
        }
        public bool GetIsDead(){
            return isDead;
        }

        public void TakeHit(int damageValue, Vector3 hitPoint){
            if(canDie){
                if(canHit){
                    OnHit?.Invoke(this,new OnHitArgs{HitPoint = hitPoint});
                    TakeHit(damageValue);
                }
            }
        }
    }

}