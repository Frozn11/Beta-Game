using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerInGame : MonoBehaviour
{


    public Text gameTimerText;
    float gameTimer = 0f;


    // Update is called once per frame
    void Update()
    {
        gameTimer += Time.deltaTime;

        int seconds = (int)(gameTimer % 60);
        int minutes = (int)(gameTimer / 60) % 60;
        int milliseconds = (int)(gameTimer % 1 * 1000);

        string timerStrin = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        gameTimerText.text = timerStrin;
    }
}
