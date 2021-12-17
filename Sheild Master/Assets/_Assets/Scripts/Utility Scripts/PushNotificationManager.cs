using System;
using UnityEngine;
using Firebase.Messaging;


public class PushNotificationManager : MonoBehaviour {

    public static PushNotificationManager current;
    private void Awake(){
        if(current == null){
            current = this;
        }else{
            Destroy(current.gameObject);
        }
        DontDestroyOnLoad(current.gameObject);

    }
    private void Start(){
        FirebaseMessaging.TokenReceived += OnTokenRecived;
        FirebaseMessaging.MessageReceived += OnMessegeRecived;
    }

    private void OnMessegeRecived(object sender, MessageReceivedEventArgs e){
        Debug.Log("Recived a New Messege " + e.Message);
    }

    private void OnTokenRecived(object sender, TokenReceivedEventArgs e){
        Debug.Log("Recived a Token " + e.Token);
    }
}
