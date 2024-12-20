using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header(" Elements ")]
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private Transform upgradeButtonsParent;
    [SerializeField] private Transform myPlayersParents;

    [Header(" Data ")]
    [SerializeField] private UpgradeSO[] upgrades;

    [Header(" Actions ")]
    public static Action<int> onUpgradePurchased;
    public static Action onPlayerPurchased;
    public static Action onPlayerUpgrade;


    [Header("Settings")]
    [SerializeField] private Transform[] elevenPoints;
    [SerializeField] private TextMeshProUGUI[] elevenPointsText;
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
            int savedElevenIndex = PlayerPrefs.GetInt("Eleven" + i, -1); // -1 varsay�lan bir de�er
            if (savedElevenIndex != -1 && savedElevenIndex < upgrades.Length)
            {
                elevenPoints[i].GetComponent<SpriteRenderer>().sprite = upgrades[savedElevenIndex].bodyImage;
                elevenPointsText[i].text = upgrades[savedElevenIndex].title;
            }

        }
        if (!PlayerPrefs.HasKey("FreePlayer"))
        {
            SetInitialUpgradeLevels();
            PlayerPrefs.SetInt("FreePlayer", 1);
            PlayerPrefs.Save();
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
        {
            upgradeButtonInstance.GetBuyButton().gameObject.SetActive(false);
            upgradeButtonInstance.soldBackGround().gameObject.SetActive(true);

        }

        
        

        int genText = upgradeLevel + upgrade.gen;

        Sprite bodyIcon = upgrade.bodyImage;
        Sprite kitIcon = upgrade.kitImage;
        Sprite faceIcon = upgrade.faceImage;
        Sprite hairIcon = upgrade.hairImage;
        string title = upgrade.title;
        string gen = genText.ToString();
        string subtitle = string.Format("LEVEL {0}  (+{1}  Cps)", upgradeLevel, upgrade.cpsPerLevel);
        string price = GetUpgradePriceString(index);
        string pos = upgrade.pos.ToString();

        upgradeButtonInstance.Configure(bodyIcon,kitIcon,faceIcon,hairIcon, title, subtitle, price,gen,pos);

        upgradeButtonInstance.GetUpgradeButton().onClick.AddListener(() => UpgradeButtonClickedCallback(index,upgradeButtonInstance,upgrade));
        upgradeButtonInstance.GetElevenButton().onClick.AddListener(() => ElevenButtonClickedCallback(index));
    }
 

    private void UpgradeButtonClickedCallback(int upgradeIndex, UpgradeButton activeButton, UpgradeSO upgrade)
    {
        if (DataManager.instance.TryPurchaseGold(GetUpgradePrice(upgradeIndex)))
        {
            IncreaseUpgradeLevel(upgradeIndex);
            onPlayerUpgrade?.Invoke();


            if (upgradeIndex >= 0)
            {
                activeButton.GetBuyButton().gameObject.SetActive(false);
                onPlayerPurchased?.Invoke();
                activeButton.soldBackGround().gameObject.SetActive(true);

            }


            DataManager.instance.totalGen();
            UIManager.instance.UpdateSlider();

        }

        else
            StartCoroutine(UIManager.instance.popUpCreat("NOT ENOUGH GOLD"));
    }

    private void ElevenButtonClickedCallback(int elevenIndex)
    {
        if (elevenIndex >= 0 && elevenIndex < upgrades.Length)
        {
            UpgradeSO upgrade = upgrades[elevenIndex];

            switch (upgrade.pos)
            {
                case PlayerPos.GK:
                    elevenPoints[0].GetComponent<SpriteRenderer>().sprite = upgrade.bodyImage;
                    elevenPointsText[0].text = upgrade.title;
                    PlayerPrefs.SetInt("Eleven" + 0, elevenIndex);
                    break;
                case PlayerPos.LCB:
                    elevenPoints[1].GetComponent<SpriteRenderer>().sprite = upgrade.bodyImage;
                    elevenPointsText[1].text = upgrade.title;
                    PlayerPrefs.SetInt("Eleven" + 1, elevenIndex);
                    break;
                case PlayerPos.RCB:
                    elevenPoints[2].GetComponent<SpriteRenderer>().sprite = upgrade.bodyImage;
                    elevenPointsText[2].text = upgrade.title;
                    PlayerPrefs.SetInt("Eleven" + 2, elevenIndex);
                    break;
                case PlayerPos.LB:
                    elevenPoints[3].GetComponent<SpriteRenderer>().sprite = upgrade.bodyImage;

                    elevenPointsText[3].text = upgrade.title;
                    PlayerPrefs.SetInt("Eleven" + 3, elevenIndex);
                    break;
                case PlayerPos.RB:
                    elevenPoints[4].GetComponent<SpriteRenderer>().sprite = upgrade.bodyImage;
                    elevenPointsText[4].text = upgrade.title;
                    PlayerPrefs.SetInt("Eleven" + 4, elevenIndex);
                    break;
                case PlayerPos.LM:
                    elevenPoints[5].GetComponent<SpriteRenderer>().sprite = upgrade.bodyImage;
                    elevenPointsText[5].text = upgrade.title;
                    PlayerPrefs.SetInt("Eleven" + 5, elevenIndex);
                    break;
                case PlayerPos.RM:
                    elevenPoints[6].GetComponent<SpriteRenderer>().sprite = upgrade.bodyImage;
                    elevenPointsText[6].text = upgrade.title;
                    PlayerPrefs.SetInt("Eleven" + 6, elevenIndex);
                    break;
                case PlayerPos.CM:
                    elevenPoints[7].GetComponent<SpriteRenderer>().sprite = upgrade.bodyImage;
                    elevenPointsText[7].text = upgrade.title;
                    PlayerPrefs.SetInt("Eleven" + 7, elevenIndex);
                    break;
                case PlayerPos.LW:
                    elevenPoints[8].GetComponent<SpriteRenderer>().sprite = upgrade.bodyImage;
                    elevenPointsText[8].text = upgrade.title;
                    PlayerPrefs.SetInt("Eleven" + 8, elevenIndex);
                    break;
                case PlayerPos.RW:
                    elevenPoints[9].GetComponent<SpriteRenderer>().sprite = upgrade.bodyImage;
                    elevenPointsText[9].text = upgrade.title;
                    PlayerPrefs.SetInt("Eleven" + 9, elevenIndex);
                    break;
                case PlayerPos.ST:
                    elevenPoints[10].GetComponent<SpriteRenderer>().sprite = upgrade.bodyImage;
                    elevenPointsText[10].text = upgrade.title;
                    PlayerPrefs.SetInt("Eleven" + 10, elevenIndex);
                    break;
                default:
                    break;
            }
            DataManager.instance.totalGen();
            UIManager.instance.UpdateSlider();

        }
        else
        {
            Debug.LogError("Hatal� eleven indeksi!");
        }
    }



    private void IncreaseUpgradeLevel(int upgradeIndex)
    {
        int currentUpgradeLevel = GetUpgradeLevel(upgradeIndex);
        currentUpgradeLevel++;

        // Save the upgrade level
        SaveUpgradeLevel(upgradeIndex, currentUpgradeLevel);

        UpdateVisuals(upgradeIndex);

        onUpgradePurchased?.Invoke(upgradeIndex);
    }

    public void FreePlayer()
    {
        SetInitialUpgradeLevels();
    }

    public void SetInitialUpgradeLevels()
{
        // Index 0, 1 ve 2 i�in upgrade seviyelerini 1 olarak ayarl�yoruz
        for (int i = 0; i < 3; i++)
        {
            SaveUpgradeLevel(i, 1);

            // D��meyi aktif duruma getir
            UpgradeButton upgradeButton = upgradeButtonsParent.GetChild(i).GetComponent<UpgradeButton>();
            upgradeButton.GetBuyButton().gameObject.SetActive(false);
            upgradeButton.soldBackGround().gameObject.SetActive(true);

            // G�rselleri g�ncelle
            UpdateVisuals(i);
            ElevenButtonClickedCallback(i);
        }
    }

    public void shopPlayerBuy(int playerIndex)
    {
        SaveUpgradeLevel(playerIndex, 1);
        // D��meyi aktif duruma getir
        UpgradeButton upgradeButton = upgradeButtonsParent.GetChild(playerIndex).GetComponent<UpgradeButton>();
        upgradeButton.GetBuyButton().gameObject.SetActive(false);
        upgradeButton.soldBackGround().gameObject.SetActive(true);

        // G�rselleri g�ncelle
        UpdateVisuals(playerIndex);
        ElevenButtonClickedCallback(playerIndex);
    }

private void SetUpgradeLevel(int upgradeIndex, int level)
{
    // Upgrade seviyesini ayarl�yoruz
    SaveUpgradeLevel(upgradeIndex, level);

    // G�rselleri g�ncelliyoruz
    UpdateVisuals(upgradeIndex);

    // OnUpgradePurchased aksiyonunu tetikliyoruz
    onUpgradePurchased?.Invoke(upgradeIndex);
}


    private void UpdateVisuals(int upgradeIndex)
    {
        UpgradeButton upgradeButton = upgradeButtonsParent.GetChild(upgradeIndex).GetComponent<UpgradeButton>();

        UpgradeSO upgrade = upgrades[upgradeIndex];

        int upgradeLevel = GetUpgradeLevel(upgradeIndex);
        int genText = upgradeLevel + upgrade.gen;

        string gen = genText.ToString();

        string subtitle = string.Format("LEVEL {0} (+{1} Cps)", upgradeLevel, upgrade.cpsPerLevel);
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

    public void AutoAssignBestPlayers()
    {
        Dictionary<PlayerPos, (UpgradeSO upgrade, int totalGen)> bestPlayers = new Dictionary<PlayerPos, (UpgradeSO, int)>();

        // Initialize the dictionary with default values
        foreach (PlayerPos pos in Enum.GetValues(typeof(PlayerPos)))
        {
            bestPlayers[pos] = (null, 0);
        }

        // Find the best player for each position among purchased players
        for (int i = 0; i < upgrades.Length; i++)
        {
            int upgradeLevel = GetUpgradeLevel(i);
            if (upgradeLevel >= 1) // Player is purchased
            {
                var upgrade = upgrades[i];
                int totalGen = upgrade.gen + upgradeLevel; // Calculate the total gen value

                if (bestPlayers[upgrade.pos].upgrade == null ||
                    totalGen > bestPlayers[upgrade.pos].totalGen)
                {
                    bestPlayers[upgrade.pos] = (upgrade, totalGen);
                }
            }
        }

        // Assign the best players to their positions
        foreach (var kvp in bestPlayers)
        {
            if (kvp.Value.upgrade != null)
            {
                int upgradeIndex = Array.IndexOf(upgrades, kvp.Value.upgrade);
                switch (kvp.Key)
                {
                    case PlayerPos.GK:
                        elevenPoints[0].GetComponent<SpriteRenderer>().sprite = kvp.Value.upgrade.bodyImage;
                        elevenPointsText[0].text = kvp.Value.upgrade.title;
                        PlayerPrefs.SetInt("Eleven" + 0, upgradeIndex);
                        break;
                    case PlayerPos.LCB:
                        elevenPoints[1].GetComponent<SpriteRenderer>().sprite = kvp.Value.upgrade.bodyImage;
                        elevenPointsText[1].text = kvp.Value.upgrade.title;
                        PlayerPrefs.SetInt("Eleven" + 1, upgradeIndex);
                        break;
                    case PlayerPos.RCB:
                        elevenPoints[2].GetComponent<SpriteRenderer>().sprite = kvp.Value.upgrade.bodyImage;
                        elevenPointsText[2].text = kvp.Value.upgrade.title;
                        PlayerPrefs.SetInt("Eleven" + 2, upgradeIndex);
                        break;
                    case PlayerPos.LB:
                        elevenPoints[3].GetComponent<SpriteRenderer>().sprite = kvp.Value.upgrade.bodyImage;
                        elevenPointsText[3].text = kvp.Value.upgrade.title;
                        PlayerPrefs.SetInt("Eleven" + 3, upgradeIndex);
                        break;
                    case PlayerPos.RB:
                        elevenPoints[4].GetComponent<SpriteRenderer>().sprite = kvp.Value.upgrade.bodyImage;
                        elevenPointsText[4].text = kvp.Value.upgrade.title;
                        PlayerPrefs.SetInt("Eleven" + 4, upgradeIndex);
                        break;
                    case PlayerPos.LM:
                        elevenPoints[5].GetComponent<SpriteRenderer>().sprite = kvp.Value.upgrade.bodyImage;
                        elevenPointsText[5].text = kvp.Value.upgrade.title;
                        PlayerPrefs.SetInt("Eleven" + 5, upgradeIndex);
                        break;
                    case PlayerPos.RM:
                        elevenPoints[6].GetComponent<SpriteRenderer>().sprite = kvp.Value.upgrade.bodyImage;
                        elevenPointsText[6].text = kvp.Value.upgrade.title;
                        PlayerPrefs.SetInt("Eleven" + 6, upgradeIndex);
                        break;
                    case PlayerPos.CM:
                        elevenPoints[7].GetComponent<SpriteRenderer>().sprite = kvp.Value.upgrade.bodyImage;
                        elevenPointsText[7].text = kvp.Value.upgrade.title;
                        PlayerPrefs.SetInt("Eleven" + 7, upgradeIndex);
                        break;
                    case PlayerPos.LW:
                        elevenPoints[8].GetComponent<SpriteRenderer>().sprite = kvp.Value.upgrade.bodyImage;
                        elevenPointsText[8].text = kvp.Value.upgrade.title;
                        PlayerPrefs.SetInt("Eleven" + 8, upgradeIndex);
                        break;
                    case PlayerPos.RW:
                        elevenPoints[9].GetComponent<SpriteRenderer>().sprite = kvp.Value.upgrade.bodyImage;
                        elevenPointsText[9].text = kvp.Value.upgrade.title;
                        PlayerPrefs.SetInt("Eleven" + 9, upgradeIndex);
                        break;
                    case PlayerPos.ST:
                        elevenPoints[10].GetComponent<SpriteRenderer>().sprite = kvp.Value.upgrade.bodyImage;
                        elevenPointsText[10].text = kvp.Value.upgrade.title;
                        PlayerPrefs.SetInt("Eleven" + 10, upgradeIndex);
                        break;
                    default:
                        break;
                }
            }
        }
        DataManager.instance.totalGen();
        UIManager.instance.UpdateSlider();
    }




}
