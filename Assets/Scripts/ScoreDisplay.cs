using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI leftPaddleScoreText = null;
    [SerializeField] TextMeshProUGUI rightPaddleScoreText = null;

    private int scorePaddleLeft { get { return GameplayManager.LevelManager.LeftPaddleScore; } }
    private int scorePaddleRight { get { return GameplayManager.LevelManager.RightPaddleScore; } }

    public void Start()
    {
        GameplayManager.LevelManager.ScoreChanged += UpdateScoreTexts;
    }

    public void UpdateScoreTexts(object sender, EventArgs e)
    {
        leftPaddleScoreText.text = scorePaddleLeft.ToString();
        rightPaddleScoreText.text = scorePaddleRight.ToString();
    }
}
