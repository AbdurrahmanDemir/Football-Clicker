using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SupporterManager : MonoBehaviour
{
    public static SupporterManager instance;

    [Header("Settings")]
    [SerializeField] private int fansNumber;
    [SerializeField] private int fansLevels;
    [SerializeField] private TextMeshProUGUI fansNumberText;
    [SerializeField] private TextMeshProUGUI fansNumberMenuText;
    [SerializeField] private TextMeshProUGUI genNumberText;
    [SerializeField] private TextMeshProUGUI winNumberText;
    [SerializeField] private TextMeshProUGUI fanAdsText;

    [Header("Elements")]
    [SerializeField] private GameObject fansPanel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadFans();

        //if (!PlayerPrefs.HasKey("PlayerDataUpdateSpecial"))
        //{
        //    if (PlayerPrefs.GetInt("FansNumber") >= 90000)
        //    {
        //        if (PlayerPrefs.GetInt("FansNumber") >= 900000)
        //        {
        //            DataManager.instance.totalGoldText = 2000000;
        //            DataManager.instance.goldIncrement = 1;
        //            fansLevels = 1;
        //            PlayerPrefs.SetInt("FanLevel", fansLevels);
        //            PlayerPrefs.SetInt("Increment", DataManager.instance.goldIncrement);
        //            DataManager.instance.incrementPriceText.text = (DataManager.instance.GetCurrentMultiplier() * 1000).ToString();
        //            Debug.Log("çalýþþþtýýýýýý");
        //            PlayerPrefs.SetInt("PlayerDataUpdateSpecial", 1);

        //        }
        //        //else
        //        //{
        //        //    DataManager.instance.totalGoldText = 1000000;
        //        //    DataManager.instance.goldIncrement = 1;
        //        //    fansLevels = 1;
        //        //    PlayerPrefs.SetInt("FanLevel", fansLevels);
        //        //    PlayerPrefs.SetInt("Increment", DataManager.instance.goldIncrement);
        //        //    DataManager.instance.incrementPriceText.text = (DataManager.instance.GetCurrentMultiplier() * 1000).ToString();
        //        //    Debug.Log("çalýþþþtýýýýýý");
        //        //    PlayerPrefs.SetInt("PlayerDataUpdateSpecial", 1);
        //        //}
        //    }
        //}




    }

    public int GetFansNumber()
    {
        int gen = DataManager.instance.GetTotalGen() * 5;
        int winNumber = 56*20;
        int fanLevel = fansLevels * 50;
        fansNumber = gen + winNumber+fanLevel;
        PlayerPrefs.SetInt("FansNumber", fansNumber);
        return fansNumber;
    }
    public void LoadFans()
    {
        fansLevels = PlayerPrefs.GetInt("FanLevel", 0);
        winNumberText.text = PlayerPrefs.GetInt("WinNumber", 0).ToString();
        fansNumber = GetFansNumber();
        fansNumberText.text = fansNumber.ToString();
        fansNumberMenuText.text = fansNumber.ToString();
        genNumberText.text = DataManager.instance.GetTotalGen().ToString();
        fanAdsText.text = (1000 * fansLevels).ToString();
    }

    public void FansUpgrade()
    {
        if (DataManager.instance.TryPurchaseGold(1000*fansLevels))
        {
            fansLevels += 1;
            PlayerPrefs.SetInt("FanLevel", fansLevels);
            fansNumberText.text = GetFansNumber().ToString();
            genNumberText.text = DataManager.instance.GetTotalGen().ToString();
            fansNumberMenuText.text = fansNumber.ToString();
            fanAdsText.text = (1000 * fansLevels).ToString();

        }
        else
        {
            StartCoroutine(UIManager.instance.popUpCreat("NOT ENOUGH GOLD"));
        }
    }

    public void FansPanel()
    {
        if (fansPanel.activeSelf)
            fansPanel.gameObject.SetActive(false);
        else
            fansPanel.gameObject.SetActive(true);

    }

    public void FixFansLevel()
    {
        //fansLevels = 562;
        //PlayerPrefs.SetInt("FanLevel", fansLevels);
        //UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.SetActive(false);
        //PlayerPrefs.SetInt("FixPanel", 1);
    }

}
