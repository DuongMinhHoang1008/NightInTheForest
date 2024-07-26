using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum FireType {
    CampFire,
    Torch
}

public class CampFireController : MonoBehaviour
{
    public FireType fireType = FireType.CampFire;
    private Light2D light;
    public Trigger BurningTrigger;
    public float burnDamage = 10;
    public float lifeTime = 60f;
    public Sprite campFireSprite;
    public Sprite torchSprite;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Globalvar globalvar;
    private void Awake() {
        light = GetComponent<Light2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        globalvar = Globalvar.Instance();
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (fireType) {
            case FireType.CampFire:
                gameObject.layer = LayerMask.NameToLayer("Station");
                animator.enabled = true;
                light.pointLightOuterRadius = 11f;
                spriteRenderer.sprite = campFireSprite;
                break;
            case FireType.Torch:
                gameObject.layer = LayerMask.NameToLayer("Item");
                animator.enabled = false;
                light.pointLightOuterRadius = 6f;
                spriteRenderer.sprite = torchSprite;
                break;
        }
        BurningTrigger.GetComponent<CircleCollider2D>().radius = light.pointLightOuterRadius;
        BurningTrigger.OnTriggerStayed2D += OnBurningTriggerStayed;
        InvokeRepeating("LightDown", lifeTime / 10, lifeTime / 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LightDown() {
        if (light.pointLightOuterRadius >= 1) {
            light.pointLightOuterRadius = light.pointLightOuterRadius - 1f;
            BurningTrigger.GetComponent<CircleCollider2D>().radius = light.pointLightOuterRadius;
        }
    }
    void OnBurningTriggerStayed(Collider2D other) {
        WolfController wolf = other.GetComponent<WolfController>();
        if (wolf != null && light.pointLightOuterRadius >= 1) {
            float dis = ((Vector2)gameObject.transform.position - other.GetComponent<Rigidbody2D>().position).magnitude; 
            wolf.ChangeHealth(burnDamage * ((light.pointLightOuterRadius - (int)dis + 1) / light.pointLightOuterRadius));
        }
    }
    public void ReFill(ItemType type) {
        float prev = light.pointLightOuterRadius;
        int deltaRange = 6;
        switch (type) {
            case ItemType.Stick:
                deltaRange = 6;
                break;
            case ItemType.Wood:
                deltaRange = 11;
                break;
            case ItemType.HardWood:
                deltaRange = 21;
                break;
            default:
                deltaRange = 6;
                break;
        }
        if (light.pointLightOuterRadius > 21f) {
            deltaRange = (int) (21 - prev);
        }
        UpdateLightRangeNode(deltaRange);
    }
    public FireType GetFireType() {
        return fireType;
    }
    public void ChangeType(FireType type) {
        if (type == FireType.Torch) {
            fireType = type;
            gameObject.layer = LayerMask.NameToLayer("Item");
            animator.enabled = false;
            spriteRenderer.sprite = torchSprite;
            light.pointLightOuterRadius = 6f;
            BurningTrigger.GetComponent<CircleCollider2D>().radius = light.pointLightOuterRadius;
        }
    }
    void UpdateLightRangeNode(float deltaRange) {
        if (light.pointLightOuterRadius + deltaRange >= 0) {
            int currentRange = (int) (light.pointLightOuterRadius);
            int newRange = (int) (light.pointLightOuterRadius + deltaRange);
            int start = Math.Max(currentRange, newRange);
            int end = Math.Min(currentRange, newRange);
            for (int y = -start; y <= start; y++) {
                for (int x = -start; x <= start; x++) {
                    if (Math.Abs(x) <= start && Math.Abs(x) >= end && Math.Abs(y) <= start && Math.Abs(y) >= end) {
                        int nodeX = (int) (x + transform.position.x);
                        int nodeY = (int) (y + transform.position.y);
                        globalvar.UpdateNode(nodeX, nodeY, NodeType.Light);
                    }
                }
            }
            float prev = light.pointLightOuterRadius;
            light.pointLightOuterRadius = prev + deltaRange;
            BurningTrigger.GetComponent<CircleCollider2D>().radius = light.pointLightOuterRadius;
        }
    }
}
