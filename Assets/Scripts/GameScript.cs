using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    int highScore;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("highScore");
        highScore += 10;
        PlayerPrefs.SetInt("highScore", highScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
