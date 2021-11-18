using System;
using UnityEngine;
namespace GamerWolf.Utils.HealthSystem {
    public class HealthEntity : MonoBehaviour,IDamagable {
        
        [SerializeField] protected int maxHealth;
        [SerializeField] protected bool isDead;
        [SerializeField] protected int currentHealth;
        [SerializeField] protected bool canDie;
        [SerializeField] protected bool canHit;
        public event EventHandler onDead;
        public event EventHandler OnHit;
        protected virtual void Awake(){

            ResetHealth();   
        }
        protected virtual void Start(){
        }
        public void ResetHealth(){
            Debug.Log(transform.name + " is Revived");
            isDead = false;
            currentHealth = maxHealth;
            SetCanDie(true);
        }
        public void TakeHit(int damageValue){
            if(canDie){
                if(canHit){
                    currentHealth -= damageValue;
                    OnHit?.Invoke(this,EventArgs.Empty);
                    if(currentHealth <= 0 && !isDead){
                        currentHealth = 0;
                        Die();
                    }
                }
            }
        }
        protected virtual void Die(){
            isDead = true;
            onDead?.Invoke(this,EventArgs.Empty);
        }
        public virtual void SetCanDie(bool _value){
            canDie = _value;
        }
        public bool GetIsDead(){
            return isDead;
        }
    }

}