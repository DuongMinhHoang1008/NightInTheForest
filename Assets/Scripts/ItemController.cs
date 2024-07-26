using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum ItemType {
    Null,
    Stick,
    Wood,
    HardWood,
    BigWood,
    Torch,
    Axe,
    Apple
}

public class ItemController : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemType type;
    public Sprite stickSprite;
    public Sprite woodSprite;
    public Sprite hardWoodSprite;
    public Sprite bigWoodSprite;
    public Sprite axeSprite;
    public Sprite appleSprite;
    public float lifeTime = 180f;
    public TextMeshProUGUI text;
    SpriteRenderer spriteRenderer;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch (type) {
            case ItemType.Stick:
                spriteRenderer.sprite = stickSprite;
                break;
            case ItemType.Wood:
                spriteRenderer.sprite = woodSprite;
                break;
            case ItemType.HardWood:
                spriteRenderer.sprite = hardWoodSprite;
                break;
            case ItemType.BigWood:
                spriteRenderer.sprite = bigWoodSprite;
                break;
            case ItemType.Apple:
                spriteRenderer.sprite = appleSprite;
                break;
        }
        CountDownTimer countDownTimer = gameObject.AddComponent<CountDownTimer>();
        countDownTimer.StartTimer(Despawn, lifeTime);
    }
    void Start()
    {
        if (text != null) {
            text.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool Despawn() {
        Destroy(gameObject);
        return true;
    }
    public void StopTimer() {
        Destroy(gameObject.GetComponent<CountDownTimer>());
    }
    public void StartTimer() {
        CountDownTimer countDownTimer = gameObject.AddComponent<CountDownTimer>();
        countDownTimer.StartTimer(Despawn, lifeTime);
    }
    public void ChangeType(ItemType typeChange) {
        type = typeChange;
        switch (typeChange) {
            case ItemType.Stick:
                spriteRenderer.sprite = stickSprite;
                break;
            case ItemType.Wood:
                spriteRenderer.sprite = woodSprite;
                break;
            case ItemType.HardWood:
                spriteRenderer.sprite = hardWoodSprite;
                break;
            case ItemType.Axe:
                spriteRenderer.sprite = axeSprite;
                break;
            case ItemType.BigWood:
                spriteRenderer.sprite = bigWoodSprite;
                break;
            case ItemType.Apple:
                spriteRenderer.sprite = appleSprite;
                break;
        }
    }
    public bool IsConsumable() {
        if (type == ItemType.Stick || type == ItemType.Wood || type == ItemType.HardWood || type == ItemType.Torch) {
            return true;
        }
        return false;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerController>() != null) {
            text.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<PlayerController>() != null) {
            text.enabled = false;
        }
    }
}
