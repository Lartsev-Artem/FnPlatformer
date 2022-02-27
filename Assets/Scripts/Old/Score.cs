using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Transform character;
    private int score;
    public int _Score { get { return score; } }
    private void Awake()
    {
        score = 0;
        character = FindObjectOfType<StatesCharachter>().transform;
    }

        void FixedUpdate()
    {

        if (character.position.x > score) { GetComponent<Text>().text = "Score: " + score.ToString(); score = (int)character.position.x; }


    }
}

