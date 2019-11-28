﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public Text scoreText;

    private int score = 0;

    public void addScore (int score)
    {
        this.score += score;
        UpdateText();
    }

    private void UpdateText()
    {
        scoreText.text = score.ToString();
    }
}
