using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PinBehavior : MonoBehaviour
{
    private bool isDown;
    private PlayerScore scoreObj;

    [SerializeField]
    private int scoreWorth = 50;

    public void Start(){
        scoreObj = FindObjectOfType<PlayerScore>();
        isDown = false;
    }

    private void OnTriggerEnter(Collider colr){
        if (colr.gameObject.CompareTag("Ground") && !isDown){
            scoreObj.addScore(scoreWorth);
            isDown = true;
        }
    }
}
