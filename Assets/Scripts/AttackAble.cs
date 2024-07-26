using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAble : MonoBehaviour
{
    public AudioSource growling;
    public Trigger attackTrigger;
    public float coolDownTime = 2f;
    public float damage = -1f;
    bool onAttackCooldown = false;
    // Start is called before the first frame update
    void Start()
    {
        attackTrigger.OnTriggerStayed2D += OnAttackTriggerStayed2D;
        // attackTrigger.OnTriggerEntered2D += OnAttackTriggerEntered2D;
        // attackTrigger.OnTriggerExited2D += OnAttackTriggerExited2D;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAttackTriggerStayed2D(Collider2D collider) {
        PlayerController playerController = collider.GetComponent<PlayerController>();
        if (playerController != null) {
            if (!onAttackCooldown) {
                playerController.ChangeHealth(damage);
                onAttackCooldown = true;
                Invoke("StopCooldown", coolDownTime);
            }
        }
    }
    void OnAttackTriggerEntered2D(Collider2D collider) {
        PlayerController playerController = collider.GetComponent<PlayerController>();
        if (playerController != null) {
            if (growling != null) {
                growling.Play();
            }
            gameObject.GetComponent<WolfController>().SlowSpeed();
        }
    }
    void OnAttackTriggerExited2D(Collider2D collider) {
        PlayerController playerController = collider.GetComponent<PlayerController>();
        if (playerController != null) {
            gameObject.GetComponent<WolfController>().FastSpeed();
        }
    }
    void StopCooldown() {
        onAttackCooldown = false;
    }
}
