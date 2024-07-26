using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.AI;
using Random = UnityEngine.Random;
using NavMeshPlus.Components;

public class GameController : MonoBehaviour
{
    public GameObject itemPref;
    public GameObject wolfPref;
    public GameObject treePref;
    public GameObject swolfPref;
    public GameObject ghostPref;
    public int worldWidth = 150;
    public int worldHeight = 100;
    Globalvar globalvar;
    // Start is called before the first frame update
    private void Awake() {
        globalvar = Globalvar.Instance();
    }
    NavMeshSurface navMeshSurface;
    void Start()
    {
        SpawnItemOvertime();
        SpawnWolfOvertime();
        //SpawnTreeOvertime();
        for (int i = 0; i < 100; i++) {
            SpawnObject(treePref, Random.Range(-worldWidth / 2, worldWidth / 2),  Random.Range(-worldHeight / 2, worldHeight / 2));
        }
        navMeshSurface = GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnItemOvertime() {
        int posX = (int) Globalvar.Instance().playerPos.x + Random.Range(20, 60);
        if (Math.Abs(posX) > worldWidth / 2) {
            posX += (posX > 0) ? -20 : 20;
        }
        int posY = (int) Globalvar.Instance().playerPos.y + Random.Range(20, 60);
        if (Math.Abs(posY) > worldHeight / 2) {
            posY += (posY > 0) ? -20 : 20;
        }
        SpawnItemRandom(itemPref, Random.Range(-posX, posX),  Random.Range(-posY, posY));
        Invoke("SpawnItemOvertime", 5f + Random.Range(-5, 15));
    }
    void SpawnWolfOvertime() {
        if (globalvar.EnemyCount < 2) {
            int posX = (int) Globalvar.Instance().playerPos.x + Random.Range(30, 40);
            int posY = (int) Globalvar.Instance().playerPos.y + Random.Range(30, 40);
            if (Math.Abs(posX) > worldWidth / 2 - 10) {
                posX += (posX > 0)? -20 : 20;
            }
            if (Math.Abs(posY) > worldHeight / 2 - 10) {
                posY += (posY > 0)? -20 : 20;
            }
            while (globalvar.GetNodeType(posX, posY) == NodeType.Light) {
                posX = (int) Globalvar.Instance().playerPos.x + Random.Range(30, 40);
                posY = (int) Globalvar.Instance().playerPos.y + Random.Range(30, 40);
                if (Math.Abs(posX) > worldWidth / 2 - 10) {
                    posX += (posX > 0)? -20 : 20;
                }
                if (Math.Abs(posY) > worldHeight / 2 - 10) {
                    posY += (posY > 0)? -20 : 20;
                }
            }
            GameObject pref = wolfPref;
            if (Globalvar.Instance().lifeTime > 240 && Globalvar.Instance().lifeTime <= 520) {
                int rand = Random.Range(1, 100);
                if (rand >= 51 && rand <= 100) {
                    pref = swolfPref;
                }
            } else if (Globalvar.Instance().lifeTime > 520) {
                int rand = Random.Range(1, 100);
                if (rand >= 21 && rand <= 70) {
                    pref = swolfPref;
                } else if (rand >= 71 && rand <= 100) {
                    pref = ghostPref;
                }
            }
            GameObject wolf = Instantiate(pref, new Vector2(Random.Range(-posX, posX),  Random.Range(-posY, posY)), Quaternion.identity);
            wolf.GetComponent<WolfController>().ChangeMaxHealth(1 + Globalvar.Instance().lifeTime / 60);
        }
        float time = 30f - Globalvar.Instance().lifeTime / 15 + Random.Range(-20, 10);
        if (time < 2.5f) {
            time = 2.5f;
        }
        Invoke("SpawnWolfOvertime", time);
    }
    bool SpawnTreeOvertime() {
        int posX = (int) Globalvar.Instance().playerPos.x + 20;
        int posY = (int) Globalvar.Instance().playerPos.y + 20;
        SpawnObject(treePref, Random.Range(-posX, posX),  Random.Range(-posY, posY));
        Invoke("SpawnTreeOvertime", 60f);
        return true;
    }
    public static bool SpawnObject(GameObject pref, float x, float y) {
        GameObject obj = Instantiate(pref, new Vector2(x, y), Quaternion.identity);
        return true;
    }
    public static bool SpawnItemRandom(GameObject pref, float x, float y) {
        int rand = Random.Range(1, 100);
        ItemType type = ItemType.Wood;
        if (rand >= 1 && rand <= 50) {
            type = ItemType.Wood;
        } else if (rand >= 51 && rand <= 60) {
            type = ItemType.HardWood;
        } else if (rand >= 61 && rand <= 94) {
            type = ItemType.Stick;
        } else if (rand >= 95 && rand <= 99) {
            type = ItemType.Apple;
        } else {
            type = ItemType.BigWood;
        }
        GameObject obj = Instantiate(pref, new Vector2(x, y), Quaternion.identity);
        obj.GetComponent<ItemController>().ChangeType(type);
        return true;
    }
}
