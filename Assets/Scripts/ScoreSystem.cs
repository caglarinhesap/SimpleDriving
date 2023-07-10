using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;

    public const string HIGH_SCORE_KEY = "HighScore";
    private float score;

    // Update is called once per frame
    void Update()
    {
        score += scoreMultiplier * Time.deltaTime;

        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    private void OnDestroy()
    {
        int currentHighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, Mathf.FloorToInt(score));
        }
    }
}
