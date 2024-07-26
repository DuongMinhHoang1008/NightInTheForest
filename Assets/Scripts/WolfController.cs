using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour
{
    public AudioSource walking;
    public float maxSpeed;
    
    private Animator animator;
    // Start is called before the first frame update
    private Rigidbody2D rigidbody2D;
    private Vector2 direction = new Vector2(0, 0);
    private bool chase;
    private Vector2 player;
    private HealthSystem health;
    private AudioSource howling;
    Globalvar globalvar;
    NavMeshAgent navMeshAgent;
    void Awake()
    {
        Globalvar.Instance().EnemyCount += 1;

        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<HealthSystem>();
        howling = GetComponent<AudioSource>();

        globalvar = Globalvar.Instance();
        
        // senseTrigger.OnTriggerEntered2D += OnSenseTriggerEntered2D;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }
    private void Start() {
        int rand = UnityEngine.Random.Range(0,4);
        if (howling != null && rand == 0) {
            howling.Play();
        }
        navMeshAgent.speed = maxSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        player = globalvar.playerPos;
        if (CheckPlayer() && player != null) {
            direction = player - rigidbody2D.position;
            direction.Normalize();
            navMeshAgent.SetDestination(player);
        }
        
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
        animator.SetFloat("Speed", direction.magnitude);
    }
    private void FixedUpdate() {
        
    }
    // private void OnSenseTriggerEntered2D(Collider2D other) {
    //     if (other.GetComponent<PlayerController>() != null) {
    //         chase = true;
    //         if (player == null) {
    //             player = other.GetComponent<Rigidbody2D>();
    //         }
    //     }
    // }
    public void ChangeHealth(float number) {
        if (!health.ChangeHealth(number)) {
            Globalvar.Instance().EnemyCount -= 1;
            Destroy(gameObject);
        }
    }
    public void ChangeMaxHealth(float number) {
        health.Health = health.Health * number;
        // Debug.Log(health.health);
    }
    public void SlowSpeed() {
        //navMeshAgent.speed = maxSpeed / 2;
    }
    public void FastSpeed() {
        //navMeshAgent.speed = maxSpeed;
    }
    public bool CheckPlayer() {
        if ((player - (Vector2)transform.position).magnitude < 50) {
            return true;
        }
        return false;
    }
}
