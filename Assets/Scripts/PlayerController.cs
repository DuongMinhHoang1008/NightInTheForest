using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public AudioSource walking;
    public InputAction moveAction;
    public InputAction interactAction;
    public InputAction useAction;
    public GameObject hand;
    public float speed;
    public GameObject footprint;
    private Vector2 move;
    private Vector2 direction;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private HealthSystem health;

    // Start is called before the first frame update
    void Start()
    {
        moveAction.Enable();
        interactAction.Enable();
        useAction.Enable();
        interactAction.performed += PickUpItem;
        useAction.performed += UseItem;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<HealthSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        move = moveAction.ReadValue<Vector2>();
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(0.0f, move.y)) {
            direction = move;
            direction.Normalize();
        }
        // if (direction.magnitude > 0 && walking != null) {
        //     walking.Play();
        // }
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
        animator.SetFloat("Speed", move.magnitude);

        Globalvar.Instance().playerPos = gameObject.transform.position;
        Globalvar.Instance().lifeTime += Time.deltaTime;
        float time = Globalvar.Instance().lifeTime;
        if (time > 600) {
            if (health.ChangeHealth(0)) {
                health.ChangeHealth(100000000000000);
                Invoke("GameOver", 3.0f);
                Globalvar.Instance().win = true;
            }
        }
        string minute = ((time / 60 < 10)? "0" : "") + ((int) (time / 60)).ToString();
        string second = ((time % 60 < 10)? "0" : "") + ((int) (time % 60)).ToString();
        UIHandler.instance.UpdateTime(minute + ":" + second);
    }
    void FixedUpdate() {
        Vector2 movePos = rigidbody2D.position + speed * move * Time.deltaTime;
        rigidbody2D.MovePosition(movePos);
    }
    void PickUpItem(InputAction.CallbackContext context) {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.25f, direction, 0.25f, LayerMask.GetMask("Item", "Station", "~Enemy", "~Player"));
        HoldingController handController = hand.GetComponent<HoldingController>();
        if (hit.collider != null) {
            GameObject hitObj = hit.collider.gameObject;
            if (!handController.IsHolding()) {
                if (hitObj.layer == LayerMask.NameToLayer("Item")) {
                    handController.PickUp(hitObj);
                }
            } else {
                Debug.Log(LayerMask.LayerToName(hitObj.layer));
                if (hitObj.layer == LayerMask.NameToLayer("Station") && hitObj.GetComponent<CampFireController>() != null 
                    && hitObj.GetComponent<CampFireController>().GetFireType() == FireType.CampFire
                    && handController.IsConsumable()) {
                    hitObj.GetComponent<CampFireController>().ReFill(handController.GetItemType());
                    handController.ThrowIntoFire();
                } else {
                    handController.Drop();
                }
            }
        } else if (handController.IsHolding()) {
            handController.Drop();
        }
    }
    void UseItem(InputAction.CallbackContext context) {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.25f, direction, 0.5f, LayerMask.GetMask("Tree"));
        HoldingController handController = hand.GetComponent<HoldingController>();
        if (handController.IsHolding()) {
            handController.CraftAndUseItem(hit);
            if (handController.GetItemType() == ItemType.Apple) {
                ChangeHealth(1);
                handController.Discard();
            }
        }
    }
    public void ChangeHealth(float number) {
        bool alive = health.ChangeHealth(number);
        UIHandler.instance.UpdateHealth((int) Math.Ceiling(health.GetComponent<HealthSystem>().GetHealthPercentage()));
        if (!alive) {
            Invoke("GameOver", 3.0f);
            Globalvar.Instance().win = false;
        }
    }
    public void Chopping() {
        animator.SetTrigger("Axe");
    }
    public void GameOver() {
        SceneManager.LoadScene("GameOver");
    }
}
