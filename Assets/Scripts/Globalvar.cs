using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum NodeType {
    None,
    Tree,
    Light
}

public class Globalvar
{
    NodeType[,] nodeTypes;
    int enemyCount;
    public int EnemyCount {
        get => enemyCount;
        set => enemyCount = value;
    }
    public bool win = true;
    public Vector2 playerPos;
    public float lifeTime = 0;
    private static Globalvar instance;
    public static Globalvar Instance() {
        if (instance == null) {
            instance = new Globalvar();
        }
        return instance;
    }
    private Globalvar() {
        nodeTypes = new NodeType[151,101];
        for (int y = 0; y < 101; y++) {
            for (int x = 0; x < 151; x++) {
                nodeTypes[x,y] = NodeType.None;
            }
        }
    }
    public void UpdateNode(int x, int y, NodeType type) {
        nodeTypes[x + 75, y + 50] = type;
    }
    public NodeType GetNodeType(int x, int y) {
        Debug.Log(x + " " + y);
        return nodeTypes[x + 75, y + 50];
    }
}
