using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    public bool isThisSingle; 
    Score score;
    public TextMeshProUGUI FinalLScore;
    public TextMeshProUGUI FinalRScore;
    void Start()
    {
        score = GameObject.Find("ScoreManager").GetComponent<Score>();
        if (isThisSingle)
        {
            FinalRScore.text = "" + score.RightCount;
        }
        else
        {
            FinalLScore.text = "" + score.LeftCount;
            FinalRScore.text = "" + score.RightCount;
        }
    }

}
