using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyRewardsUI : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject panel;

    [Header(" Timer Elements ")]
    [SerializeField] private GameObject claimButton;
    [SerializeField] private GameObject timerContainer;
    [SerializeField] private TextMeshProUGUI timerText;

    private int seconds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            seconds /= 2;
    }

    public void ResetTimer()
    { 
        int timerSeconds = 60 * 60 * 24 - 1;
        InitializeTimer(timerSeconds);
    }

    public void InitializeTimer(int seconds)
    {
        claimButton.SetActive(false);
        timerContainer.SetActive(true);

        this.seconds = seconds;

        UpdateTimerText();

        InvokeRepeating("UpdateTimer", 0, 1);
    }

    private void UpdateTimer()
    {
        seconds--;
        UpdateTimerText();

        if (seconds <= 0)
            StopTimer();
    }

    private void StopTimer()
    {
        CancelInvoke("UpdateTimer");

        claimButton.SetActive(true);
        timerContainer.SetActive(false);
    }

    private void UpdateTimerText()
    {
        timerText.text = TimeSpan.FromSeconds(seconds).ToString();
    }

    public void AllRewardsClaimed()
    {
        claimButton.SetActive(false);
        timerContainer.SetActive(false);
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}
