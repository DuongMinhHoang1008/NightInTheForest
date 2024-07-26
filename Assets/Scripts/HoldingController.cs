using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingController : MonoBehaviour
{   
    public GameObject torchPref;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsHolding() {
        if (transform.childCount > 0) {
            return true;
        }
        return false;
    }
    public void PickUp(GameObject obj) {
        if (obj != null) {
            obj.transform.SetParent(transform);
            obj.transform.position = transform.position;
            obj.GetComponent<CircleCollider2D>().enabled = false;
            obj.GetComponent<ItemController>().StopTimer();
            obj.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }
    public void Drop() {
        if (IsHolding()) {
            GameObject child = transform.GetChild(0).gameObject;
            transform.DetachChildren();
            child.transform.position = transform.position + Vector3.down * 0.95f;
            child.GetComponent<CircleCollider2D>().enabled = true;
            if (child.GetComponent<ItemController>() != null) {
                child.GetComponent<ItemController>().StartTimer();
            }
            child.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }
    public void ThrowIntoFire() {
        if (IsHolding()) {
            ItemController item = transform.GetChild(0).gameObject.GetComponent<ItemController>();
            if (item.IsConsumable()) {
                GameObject child = transform.GetChild(0).gameObject;
                transform.DetachChildren();
                Destroy(child);
            }
        }
    }
    public void Discard() {
        if (IsHolding()) {
            GameObject child = transform.GetChild(0).gameObject;
            transform.DetachChildren();
            Destroy(child);
        }
    }
    public ItemType GetItemType() {
        if (IsHolding()) {
            GameObject child = transform.GetChild(0).gameObject;
            return child.GetComponent<ItemController>().type;
        }
        return ItemType.Null;
    }
    public void CraftAndUseItem(RaycastHit2D hit) {
        if (IsHolding()) {
            if (GetItemType() == ItemType.Stick) {
                GameObject child = transform.GetChild(0).gameObject;
                GameObject torch = Instantiate(torchPref, child.transform.position, Quaternion.identity);
                torch.GetComponent<CampFireController>().ChangeType(FireType.Torch);
                torch.AddComponent<ItemController>();
                torch.GetComponent<ItemController>().type = ItemType.Torch;
                torch.transform.SetParent(transform);
                torch.GetComponent<CircleCollider2D>().enabled = false;
                torch.GetComponent<ItemController>().StopTimer();
                torch.GetComponent<SpriteRenderer>().sortingOrder = 1;
                Destroy(child);
            } else if (GetItemType() == ItemType.HardWood) {
                GameObject child = transform.GetChild(0).gameObject;
                child.GetComponent<ItemController>().ChangeType(ItemType.Axe);
            } else if (GetItemType() == ItemType.BigWood) {
                GameObject child = transform.GetChild(0).gameObject;
                child.GetComponent<ItemController>().ChangeType(ItemType.Axe);
                GameObject campFire = Instantiate(torchPref, child.transform.position, Quaternion.identity);
                campFire.GetComponent<CampFireController>().ChangeType(FireType.CampFire);
                campFire.transform.SetParent(transform);
                campFire.GetComponent<CircleCollider2D>().enabled = false;
                campFire.GetComponent<SpriteRenderer>().sortingOrder = 1;
                Destroy(child);
            } else if (GetItemType() == ItemType.Axe) {
                if (hit.collider != null) {
                    GameObject tree = hit.collider.gameObject;
                    GameObject player = gameObject.transform.parent.gameObject;
                    player.GetComponent<PlayerController>().Chopping();
                    tree.GetComponent<TreeController>().ChangeHealth(-1);
                }
            }
        }
    }
    public bool IsConsumable() {
        ItemController item = transform.GetChild(0).gameObject.GetComponent<ItemController>();
        return item.IsConsumable();
    }
}
