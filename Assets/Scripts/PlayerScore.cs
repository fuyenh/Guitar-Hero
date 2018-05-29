using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {


    Text playerScore;
    

    public void Start()
    {
        playerScore = GetComponent<Text>();
    }


    public void ShowScore(int score) {
        playerScore.text = "Score " + score.ToString();
    }
	
}
