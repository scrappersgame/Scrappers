﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    public Camera mainCam;
    private float shakeAmount = 0;
    private void Awake()
    {
        if(mainCam == null){
            mainCam = Camera.main;
        }
    }
    public void Shake(float amt, float length){
        shakeAmount = amt;
        InvokeRepeating("ShakeIt", 0, 0.01f);
        Invoke("StopShake", length);
    }
    void ShakeIt(){
        if(shakeAmount > 0){
            Vector3 camPos = mainCam.transform.position;
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;
        }
    }
    void StopShake(){
        CancelInvoke("ShakeIt");
    }
}