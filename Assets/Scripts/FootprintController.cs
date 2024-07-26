using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintController : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifetime = 5f;
    public float timeLeft;
    void Start()
    {
        Invoke("Disappear", lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
    }
    void Disappear() {
        Destroy(gameObject);
    }
}
