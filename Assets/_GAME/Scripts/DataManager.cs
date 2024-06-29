using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Schema;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private TextMeshProUGUI cpsText;
    [SerializeField] private TextMeshProUGUI incrementPriceText;

    [Header(" Data ")]
    public double totalGoldText;
    [SerializeField] private int gem;
    [SerializeField] private int frenzyModeMultiplier;
    private int goldIncrement;
    private double previousGoldCount;

    [Header(" Data ")]
    [SerializeField] private UpgradeSO[] upgrades;

    [Header("Action")]
    public static Action onUpgradeClubHouse;

    [Header("TeamData")]
    public int totalTeamGen;
    public int defGen;
    public int midGen;
    public int forGen;
    public TextMeshProUGUI totalTeamGenText;
    int savedElevenIndex;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();


        goldIncrement = GetCurrentMultiplier();

        InputManager.onPitchClicked += GoldClickedCallback;

    }

    private void OnDestroy()
    {
        InputManager.onPitchClicked -= GoldClickedCallback;

    }

    void Start()
    {
        
        InvokeRepeating("UpdateCpsText", 0, 1);
        incrementPriceText.text = (GetCurrentMultiplier() * 1000).ToString();

        totalGen();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            AddGoldDouble(totalGoldText);
    }
    public bool TryPurchaseGold(double price)
    {
        if (price <= totalGoldText)
        {
            totalGoldText -= price;
            return true;
        }

        return false;
    }
    public bool TryPurchaseGem(int price)
    {
        if (price <= gem)
        {
            gem -= price;
            return true;
        }

        return false;
    }

    public void AddGem(int value)
    {
        gem += value;

        UpdateGemText();

        SaveData();
    }

    public void AddGoldDouble(double value)
    {
        totalGoldText += value;

        UpdateGoldText();

        SaveData();
    }

    public void AddGoldFloat(float value)
    {
        AddGoldDouble((double)value);
    }

    private void GoldClickedCallback()
    {
        totalGoldText += goldIncrement;

        UpdateGoldText();

        SaveData();
    }

    private void UpdateGoldText()
    {
        goldText.text = DoubleUtilities.ToIdleNotation(totalGoldText);

    }
    private void UpdateGemText()
    {
        gemText.text = gem.ToString();
    }

    private void UpdateCpsText()
    {
        double cps = totalGoldText - previousGoldCount;

        if (cps < 0)
            cps = UpgradeManager.instance.GetCarrotsPerSecond();

        cpsText.text = DoubleUtilities.ToIdleNotation(cps) + " cps";

        previousGoldCount = totalGoldText;
    }

    private void SaveData()
    {
        PlayerPrefs.SetString("Carrots", totalGoldText.ToString());
        PlayerPrefs.SetInt("Gem", gem);
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey("Carrots"))
        {
        double.TryParse(PlayerPrefs.GetString("Carrots"), out totalGoldText);
        }
        else
        {
            AddGoldFloat(5000);
            AddGem(10);
        }
        int gems = PlayerPrefs.GetInt("Gem");
        gem = gems;
        UpdateGemText();
        UpdateGoldText();
    }

    public int GetCurrentMultiplier()
    {
        goldIncrement = PlayerPrefs.GetInt("Increment",1);
        return goldIncrement;
    }
    public void UpgradeClubHouse()
    {
        if (TryPurchaseGold(PlayerPrefs.GetInt("Increment") * 1000))
        {
            goldIncrement += 1;
            PlayerPrefs.SetInt("Increment", goldIncrement);
            incrementPriceText.text = (GetCurrentMultiplier() * 1000).ToString();
            onUpgradeClubHouse?.Invoke();
        }
        else
        {
            Debug.Log("yetersiz");
        }
    }
    public void totalGen()
    {
        totalTeamGen = 0; // totalTeamGen'i sýfýrla

        for (int i = 0; i < 11; i++)
        {
            if (PlayerPrefs.HasKey("Eleven" + i))
            {
                 savedElevenIndex = PlayerPrefs.GetInt("Eleven" + i);

                if (savedElevenIndex >= 0 && savedElevenIndex < upgrades.Length)
                {
                    int upgradeLevel = ShopManager.instance.GetUpgradeLevel(savedElevenIndex);
                    totalTeamGen += (upgrades[savedElevenIndex].gen + upgradeLevel);
                    totalTeamGenText.text = totalTeamGen.ToString();
                }
                else
                {
                    Debug.LogError("Hatalý eleven indeksi: " + savedElevenIndex);

                }
            }
            else
            {
                continue;
            }
            
        }
        defGen = 0;

        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey("Eleven" + i))
            {
                 savedElevenIndex = PlayerPrefs.GetInt("Eleven" + i);

                if (savedElevenIndex >= 0 && savedElevenIndex < upgrades.Length)
                {
                    int upgradeLevel = ShopManager.instance.GetUpgradeLevel(savedElevenIndex);
                    defGen += (upgrades[savedElevenIndex].gen + upgradeLevel);
                }
                else
                {
                    Debug.LogError("Hatalý eleven indeksi: " + savedElevenIndex);

                }
            }
            else
            {
                continue;
            }
        }

        midGen = 0;
        for (int i = 5; i < 8; i++)
        {
            if (PlayerPrefs.HasKey("Eleven" + i))
            {
                savedElevenIndex = PlayerPrefs.GetInt("Eleven" + i);

                if (savedElevenIndex >= 0 && savedElevenIndex < upgrades.Length)
                {
                    int upgradeLevel = ShopManager.instance.GetUpgradeLevel(savedElevenIndex);
                    midGen += (upgrades[savedElevenIndex].gen + upgradeLevel);
                }
                else
                {
                    Debug.LogError("Hatalý eleven indeksi: " + savedElevenIndex);

                }
            }
            else
            {
                continue;
            }
        }
        forGen = 0;
        for (int i = 8; i < 11; i++)
        {
            if (PlayerPrefs.HasKey("Eleven" + i))
            {
                savedElevenIndex = PlayerPrefs.GetInt("Eleven" + i);

                if (savedElevenIndex >= 0 && savedElevenIndex < upgrades.Length)
                {
                    int upgradeLevel = ShopManager.instance.GetUpgradeLevel(savedElevenIndex);
                    forGen += (upgrades[savedElevenIndex].gen + upgradeLevel);
                }
                else
                {
                    Debug.LogError("Hatalý eleven indeksi: " + savedElevenIndex);

                }
            }
            else
            {
                continue;
            }
        }

        Debug.Log(defGen);
        Debug.Log(midGen);
        Debug.Log(forGen);



    }
    public int GetTotalGen()
    {
        return totalTeamGen;
    }
    public int GetDefGen()
    {
        return defGen;
    }
    public int GetMidGen()
    {
        return midGen;
    }
    public int GetForGen()
    {
        return forGen;
    }
    
}
