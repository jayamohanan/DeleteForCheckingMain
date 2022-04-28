using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [HideInInspector] public int score;
    UIManager uiManager;
    void Awake()
    {
        score = PlayerPrefs.GetInt("Score");
    }
    private void OnApplicationPause(bool pause)
    {
#if (!UNITY_EDITOR)
        if(pause)
        {
            //if gamewon currentlevel will be incremented before this
            PlayerPrefs.SetInt("Score", (score));
        }
        else
        {
            score = PlayerPrefs.GetInt("Score");
        }
#endif
    }
    private void OnApplicationQuit()
    {
#if (UNITY_EDITOR)
        //if gamewon currentlevel will be incremented before this
        PlayerPrefs.SetInt("Score", (score));
#endif
    }
    public void AddScore(int s)
    {
        score += s;
        uiManager.SetScoreText(score);
    }

}
