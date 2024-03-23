using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OfflineEarningsUI))]
public class OfflineEarningsManager : MonoBehaviour
{
    [Header(" Elements ")]
    private OfflineEarningsUI offlineEarningsUI;

    [Header(" Settings ")]
    [SerializeField] private int maxOfflineSeconds; 
    private DateTime lastDateTime;

    // Start is called before the first frame update
    void Start()
    {
        offlineEarningsUI = GetComponent<OfflineEarningsUI>();

        if (LoadLastDateTime())
            CalculateOfflineSeconds();
        else
            Debug.LogError("Unable to parse the last date time");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            return;

        SaveCurrentDateTime();
    }

    private void CalculateOfflineSeconds()
    {
        TimeSpan timeSpan = DateTime.Now.Subtract(lastDateTime);

        int offlineSeconds = (int)timeSpan.TotalSeconds;
        offlineSeconds = Mathf.Min(offlineSeconds, maxOfflineSeconds);

        CalculateOfflineEarnings(offlineSeconds);
    }

    private void CalculateOfflineEarnings(int offlineSeconds)
    {
        double offlineEarnings = offlineSeconds * UpgradeManager.instance.GetCarrotsPerSecond();

        if (offlineEarnings <= 0)
            return;

        Debug.Log("You've earned " + offlineEarnings);

        offlineEarningsUI.DisplayPopup(offlineEarnings);
    }

    private bool LoadLastDateTime()
    {        
        bool validDateTime  = DateTime.TryParse(PlayerPrefs.GetString("LastDateTime"), out lastDateTime);

        return validDateTime;
    }

    private void SaveCurrentDateTime()
    {
        DateTime now = DateTime.Now;

        PlayerPrefs.SetString("LastDateTime", now.ToString());
    }
}
