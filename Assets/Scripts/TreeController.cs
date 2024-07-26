using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public GameObject itemPref;
    HealthSystem health;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<HealthSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeHealth(float healthChange) {
        if (!health.IsInvincible) {
            Invoke("BeingHit", 0.25f);
            if (!health.ChangeHealth(healthChange)) {
                Invoke("ChopDown", 0.5f);
            }
        }
    }
    void ChopDown() {
        Destroy(gameObject);
    }
    void BeingHit() {
        GameController.SpawnItemRandom(itemPref, transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f));
        spriteRenderer.color = Color.gray;
        Invoke("ReturnColor", 0.25f);
    }
    void ReturnColor() {
        spriteRenderer.color = Color.white;
    }
}
