using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] private IOSNotificationHandler iosNotificationHandler;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration; // in minutes

    private int energy;

    private const string ENERGY_KEY = "Energy";
    private const string ENERGY_READY_KEY = "EnergyReady";

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            return;
        }

        CancelInvoke();
        UpdateHighScore();
        UpdateEnergy();
    }

    private void Start()
    {
        OnApplicationFocus(true);
    }

    public void Play()
    {
        if (ConsumeEnergy())
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            StartCoroutine(BlinkEnergyText());
        }
    }

    private IEnumerator BlinkEnergyText()
    {
        energyText.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.25f);
        energyText.color = new Color32(255, 255, 255, 255);
    }

    private void UpdateHighScore()
    {
        int highScore = PlayerPrefs.GetInt(ScoreSystem.HIGH_SCORE_KEY, 0);
        highScoreText.text = "High Score " + highScore.ToString();
    }

    private void UpdateEnergy()
    {
        energy = PlayerPrefs.GetInt(ENERGY_KEY, maxEnergy);
        if (energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(ENERGY_READY_KEY, string.Empty);

            if (energyReadyString == string.Empty)
            {
                energy = maxEnergy;
                return;
            }

            DateTime energyReady = DateTime.Parse(energyReadyString);

            if (DateTime.Now > energyReady)
            {
                energy = maxEnergy;
                PlayerPrefs.SetInt(ENERGY_KEY, energy);
            }
            else
            {
                Invoke(nameof(UpdateEnergy), (energyReady - DateTime.Now).Seconds);
            }
        }

        energyText.text = "Energy:" + energy;
    }

    private bool ConsumeEnergy()
    {
        if (energy < 1)
        {
            return false;
        }

        energy--;
        PlayerPrefs.SetInt(ENERGY_KEY, energy);
        if (energy == 0)
        {
            DateTime energyReadyTime = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(ENERGY_READY_KEY, energyReadyTime.ToString());
#if UNITY_ANDROID
            androidNotificationHandler.ScheduleNotification(energyReadyTime);
#elif UNITY_IOS
            androidNotificationHandler.ScheduleNotification(energyRechargeDuration);
#endif
        }
        return true;
    }
}
