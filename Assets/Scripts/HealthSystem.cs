using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float invincibleTime = 0.5f;
    bool isInvincible = false;
    public bool IsInvincible {
        get => isInvincible;
    }
    public float Health {
        set => health = value;
        get => health;
    }
    // Start is called before the first frame update
    private void Awake() {
        health = maxHealth;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //If still alive return true, else return false
    public bool ChangeHealth(float number) {
        if (!isInvincible) {
            health += number;
            if (health < 0) {
                health = 0;
            } else if (health > maxHealth) {
                health = maxHealth;
            }
            // Debug.Log(health);
            if (health <= 0) return false;
            isInvincible = true;
            Invoke("StopInvincible", invincibleTime);
        }
        return true;
    }
    void StopInvincible() {
        isInvincible = false;
    }
    public float GetHealthPercentage() {
        return health / maxHealth * 100;
    }
}
