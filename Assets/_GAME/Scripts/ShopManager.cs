using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header(" Elements ")]
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private Transform upgradeButtonsParent;
    [SerializeField] private GameObject elevenPanel;

    [Header(" Data ")]
    [SerializeField] private UpgradeSO[] upgrades;

    [Header(" Actions ")]
    public static Action<int> onUpgradePurchased;


    [Header("Settings")]
    [SerializeField] private Transform[] elevenPoints;
    int playerIndex;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SpawnButtons();
        for (int i = 0; i < elevenPoints.Length; i++)
        {
            int savedElevenIndex = PlayerPrefs.GetInt("Eleven" + i, -1); // -1 varsayýlan bir deðer
            if (savedElevenIndex != -1 && savedElevenIndex < upgrades.Length)
            {
                elevenPoints[i].GetComponent<SpriteRenderer>().sprite = upgrades[savedElevenIndex].icon;
            }
        }
    }

    private void SpawnButtons()
    {
        for (int i = 0; i < upgrades.Length; i++)
            SpawnButton(i);      
    }

    private void SpawnButton(int index)
    {
        UpgradeButton upgradeButtonInstance = Instantiate(upgradeButton, upgradeButtonsParent);

        UpgradeSO upgrade = upgrades[index];

        int upgradeLevel = GetUpgradeLevel(index);

        if (upgradeLevel == 0)
            upgradeButtonInstance.GetBuyButton().gameObject.SetActive(true);
        else
            upgradeButtonInstance.GetBuyButton().gameObject.SetActive(false);

        
        

        int genText = upgradeLevel + upgrade.gen;

        Sprite icon = upgrade.icon;
        string title = upgrade.title;
        string gen = genText.ToString();
        string subtitle = string.Format("level{0} (+{1} Cps)", upgradeLevel, upgrade.cpsPerLevel);
        string price = GetUpgradePriceString(index);
        string pos = upgrade.pos.ToString();

        upgradeButtonInstance.Configure(icon, title, subtitle, price,gen,pos);

        upgradeButtonInstance.GetUpgradeButton().onClick.AddListener(() => UpgradeButtonClickedCallback(index,upgradeButtonInstance,upgrade));
        upgradeButtonInstance.GetElevenButton().onClick.AddListener(() => ElevenButtonClickedCallback(index));
    }

    private void UpgradeButtonClickedCallback(int upgradeIndex, UpgradeButton activeButton, UpgradeSO upgrade)
    {
        if (PitchManager.instance.TryPurchase(GetUpgradePrice(upgradeIndex)))
        {
            IncreaseUpgradeLevel(upgradeIndex);


            if (upgradeIndex >= 0)
                activeButton.GetBuyButton().gameObject.SetActive(false);


            PitchManager.instance.totalGen();
            UIManager.instance.UpdateSlider();

        }
            
        else
            Debug.Log("You're too poor for the upgrade ! ");
    }
    private void ElevenButtonClickedCallback(int elevenIndex)
    {
        if (elevenIndex >= 0 && elevenIndex < upgrades.Length)
        {
            UpgradeSO upgrade = upgrades[elevenIndex];

            switch (upgrade.pos)
            {
                case PlayerPos.GK:
                    elevenPoints[0].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
                    PlayerPrefs.SetInt("Eleven" + 0, elevenIndex);
                    break;
                case PlayerPos.LCB:
                    elevenPoints[1].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
                    PlayerPrefs.SetInt("Eleven" + 1, elevenIndex);
                    break;
                case PlayerPos.RCB:
                    elevenPoints[2].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
                    PlayerPrefs.SetInt("Eleven" + 2, elevenIndex);
                    break;
                case PlayerPos.LB:
                    elevenPoints[3].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
                    PlayerPrefs.SetInt("Eleven" + 3, elevenIndex);
                    break;
                case PlayerPos.RB:
                    elevenPoints[4].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
                    PlayerPrefs.SetInt("Eleven" + 4, elevenIndex);
                    break;
                case PlayerPos.LM:
                    elevenPoints[5].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
                    PlayerPrefs.SetInt("Eleven" + 5, elevenIndex);
                    break;
                case PlayerPos.RM:
                    elevenPoints[6].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
                    PlayerPrefs.SetInt("Eleven" + 6, elevenIndex);
                    break;
                case PlayerPos.CM:
                    elevenPoints[7].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
                    PlayerPrefs.SetInt("Eleven" + 7, elevenIndex);
                    break;
                case PlayerPos.LW:
                    elevenPoints[8].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
                    PlayerPrefs.SetInt("Eleven" + 8, elevenIndex);
                    break;
                case PlayerPos.RW:
                    elevenPoints[9].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
                    PlayerPrefs.SetInt("Eleven" + 9, elevenIndex);
                    break;
                case PlayerPos.ST:
                    elevenPoints[10].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
                    PlayerPrefs.SetInt("Eleven" + 10, elevenIndex);
                    break;
                default:
                    break;
            }
            PitchManager.instance.totalGen();
            UIManager.instance.UpdateSlider();

        }
        else
        {
            Debug.LogError("Hatalý eleven indeksi!");
        }
    }

    //public void AddEleven(int index)
    //{

    //    if (playerIndex != -1 && playerIndex < upgrades.Length)
    //    {
    //        UpgradeSO upgrade = upgrades[playerIndex];

    //        if (index >= 0 && index < elevenPoints.Length)
    //        {
    //            Debug.Log("çalýþtý");
    //            elevenPoints[index].GetComponent<SpriteRenderer>().sprite = upgrade.icon;
    //            elevenPanel.SetActive(false);
    //            PlayerPrefs.SetInt("Eleven" + index, playerIndex);

    //            PitchManager.instance.totalGen();
    //        }
    //        else
    //        {
    //            Debug.LogError("Hatalý eleven pozisyonu!");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Geçerli bir eleven oyuncusu seçilmemiþ!");
    //    }
    //}



    private void IncreaseUpgradeLevel(int upgradeIndex)
    {
        int currentUpgradeLevel = GetUpgradeLevel(upgradeIndex);
        currentUpgradeLevel++;

        // Save the upgrade level
        SaveUpgradeLevel(upgradeIndex, currentUpgradeLevel);

        UpdateVisuals(upgradeIndex);

        onUpgradePurchased?.Invoke(upgradeIndex);
    }

    private void UpdateVisuals(int upgradeIndex)
    {
        UpgradeButton upgradeButton = upgradeButtonsParent.GetChild(upgradeIndex).GetComponent<UpgradeButton>();

        UpgradeSO upgrade = upgrades[upgradeIndex];

        int upgradeLevel = GetUpgradeLevel(upgradeIndex);
        int genText = upgradeLevel + upgrade.gen;

        string gen = genText.ToString();

        string subtitle = string.Format("lvl{0} (+{1} Cps)", upgradeLevel, upgrade.cpsPerLevel);
        string price = GetUpgradePriceString(upgradeIndex);

        upgradeButton.UpdateVisuals(subtitle, price,gen);
    }

    public void RewardUpgrade(int upgradeIndex, int levels)
    {
        for (int i = 0; i < levels; i++)
            IncreaseUpgradeLevel(upgradeIndex);        
    }

    private string GetUpgradePriceString(int upgradeIndex)
    {
        return DoubleUtilities.ToIdleNotation(GetUpgradePrice(upgradeIndex));
    }

    private double GetUpgradePrice(int upgradeIndex)
    {
        return upgrades[upgradeIndex].GetPrice(GetUpgradeLevel(upgradeIndex));
    }

    public int GetUpgradeLevel(int upgradeIndex)
    {
        return PlayerPrefs.GetInt("Upgrade" + upgradeIndex);
    }

    private void SaveUpgradeLevel(int upgradeIndex, int upgradeLevel)
    {
        PlayerPrefs.SetInt("Upgrade" + upgradeIndex, upgradeLevel);
    }

    public UpgradeSO[] GetUpgrades()
    {
        return upgrades;
    }
}
