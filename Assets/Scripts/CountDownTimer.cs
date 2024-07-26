using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    public float timerTime = 5.0f;
    float countDown;
    // Start is called before the first frame update
    Func<bool> func;

    // Update is called once per frame
    void Update()
    {
        if (countDown >= 0) {
            countDown -= Time.deltaTime;
            if (countDown < 0) {
                func();
                Destroy(this);
            }
        }
    }
    public void StartTimer(Func<bool> f) {
        func = f;
        countDown = timerTime;
    }
    public void StartTimer(Func<bool> f, float time) {
        timerTime = time;
        func = f;
        countDown = timerTime;
    }
}
