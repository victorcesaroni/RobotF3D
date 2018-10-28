using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Football : MonoBehaviour
{
    public GameObject blueGoal;
    public GameObject redGoal;
    public GameObject ball;
    public GameObject ballSpawn;
    public Text textScoreBlue;
    public Text textScoreRed;

    public int blueScore;
    public int redScore;

    // Use this for initialization
    void Start ()
    {
        blueScore = 0;
        redScore = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnScore(GameObject goal)
    {
        if (goal == redGoal.gameObject)
        {
            ScoreBlue();
        }
        else if (goal == blueGoal.gameObject)
        {
            ScoreRed();
        }
    }

    public void ScoreBlue()
    {
        blueScore++;
        textScoreBlue.text = "A " + blueScore;
        ball.transform.localPosition = ballSpawn.transform.localPosition;
        ball.transform.position = ballSpawn.transform.position;
    }

    public void ScoreRed()
    {
        redScore++;
        textScoreRed.text = redScore + " V";
        ball.transform.localPosition = ballSpawn.transform.localPosition;
        ball.transform.position = ballSpawn.transform.position;
    }
}
