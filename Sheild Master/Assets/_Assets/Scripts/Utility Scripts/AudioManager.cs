using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace SheildMaster{
    
    public enum SoundType{
        BGM,
        ButtonClickSound,
        Coin_Collect,
        Coin_Use,
        Player_Win,
        Player_Lost,
        Player_Death,
        Enemy_Death,
        Item_Purchase,
        Fire_Sound,
        Game_Start,
        Wall_hit,
    }
    
    [System.Serializable]
    public class Sounds{
        public SoundType soundType;
        public AudioClip audioClip;
        public bool isLooping;
        public bool playOnAwake;
        public bool playonShot;
        [Range(0f,1f)]
        public float volumeSlider = 1;
        [Range(-3f,3f)]
        public float pitchSlider = 1;
        public bool isMute;
        public bool isSfx;
        

        [HideInInspector]
        public AudioSource source;

    }
    
    public class AudioManager : MonoBehaviour{
        public static AudioManager current{get;private set;}
        

        [SerializeField] private Sounds[] sounds;
        [SerializeField] private SettingsSO soundSettings;
        private List<AudioSource> sfxSourceList;
        

        
        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
                Debug.Log($"Another Audio Manager is Found And Destroyed");
            }
            DontDestroyOnLoad(current.gameObject);
        }
        private void Start(){
            sfxSourceList = new List<AudioSource>();
            foreach(Sounds s in sounds){
                s.source = gameObject.AddComponent<AudioSource>();
                if(s.isSfx){
                    sfxSourceList.Add(s.source);
                }
                s.source.loop = s.isLooping;
                s.source.pitch = s.pitchSlider;
                s.source.volume = s.volumeSlider;
                s.source.playOnAwake = s.playOnAwake;
                s.source.clip = s.audioClip;
                s.source.mute = s.isMute;
            }
             
        }
        private void Update(){
            MuteMusic(!soundSettings.GetIsMusicOn());
            MuteSFX(!soundSettings.GetIsSoundOn());
        }

        public void MuteMusic(bool Mute){
            
            for (int i = 0; i < sounds.Length; i++){
                if(sounds[i].soundType == SoundType.BGM){
                    sounds[i].source.mute = Mute;
                }
                
            }
            
        }
        public void MuteSFX(bool mute){
            for (int i = 0; i < sfxSourceList.Count; i++){
                sfxSourceList[i].mute = mute;
            }
        }
        
        
        public void PlayMusic(SoundType soundType){
            Sounds s = Array.Find(sounds ,s => s.soundType == soundType);
            if(s != null){
                if(s.source.clip != null){
                    s.source.Play();

                }
            }
        }
        public void PauseMusic(SoundType soundType){
            Sounds s = Array.Find(sounds ,s => s.soundType == soundType);
            if(s != null){
                if(s.source.clip != null){
                    s.source.Pause();
                    
                }
            }
        }
        public void PlayOneShotMusic(SoundType soundType){
            Sounds s = Array.Find(sounds ,s => s.soundType == soundType);
            if(s != null){
                if(s.source.clip != null){
                    s.source.PlayOneShot(s.audioClip);
                }
            }
        }
        public void StopAudio(SoundType soundType){
            Sounds s = Array.Find(sounds ,s => s.soundType == soundType);
            if(s != null){
                if(s.source.clip != null){
                    s.source.Stop();
                }
                
            }
        }
        
    }

}