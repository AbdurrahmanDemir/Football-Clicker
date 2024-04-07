using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Schema;
using System;

public class PitchManager : MonoBehaviour
{
    public static PitchManager instance;

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI carrotsText;
    [SerializeField] private TextMeshProUGUI cpsText;
    [SerializeField] private TextMeshProUGUI incrementPriceText;
    [SerializeField] private GameObject clubHousePanel;

    [Header(" Data ")]
    [SerializeField] private double totalCarrotsCount;
    [SerializeField] private int frenzyModeMultiplier;
    private int carrotIncrement;
    private double previousCarrotCount;

    [Header(" Data ")]
    [SerializeField] private UpgradeSO[] upgrades;

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

        carrotIncrement = GetCurrentMultiplier();

        InputManager.onPitchClicked += CarrotClickedCallback;

        Pitch.onFrenzyModeStarted += FrenzyModeStartedCallback;
        Pitch.onFrenzyModeStopped += FrenzyModeStoppedCallback;
    }

    private void OnDestroy()
    {
        InputManager.onPitchClicked -= CarrotClickedCallback;

        Pitch.onFrenzyModeStarted -= FrenzyModeStartedCallback;
        Pitch.onFrenzyModeStopped -= FrenzyModeStoppedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        InvokeRepeating("UpdateCpsText", 0, 1);
        incrementPriceText.text = (GetCurrentMultiplier() * 1000).ToString();

        totalGen();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            AddCarrots(totalCarrotsCount);
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
    public bool TryPurchase(double price)
    {
        if (price <= totalCarrotsCount)
        {
            totalCarrotsCount -= price;
            return true;
        }

        return false;
    }

    public void AddCarrots(double value)
    {
        totalCarrotsCount += value;

        UpdateCarrotsText();

        SaveData();
    }

    public void AddCarrots(float value)
    {
        AddCarrots((double)value);
    }

    private void CarrotClickedCallback()
    {
        totalCarrotsCount += carrotIncrement;

        UpdateCarrotsText();

        SaveData();
    }

    private void UpdateCarrotsText()
    {
        carrotsText.text = DoubleUtilities.ToIdleNotation(totalCarrotsCount);
    }

    private void UpdateCpsText()
    {
        double cps = totalCarrotsCount - previousCarrotCount;

        if (cps < 0)
            cps = UpgradeManager.instance.GetCarrotsPerSecond();

        cpsText.text = DoubleUtilities.ToIdleNotation(cps) + " cps";

        previousCarrotCount = totalCarrotsCount;
    }

    private void FrenzyModeStartedCallback()
    {
        carrotIncrement = frenzyModeMultiplier;
    }

    private void FrenzyModeStoppedCallback()
    {
        carrotIncrement = 1;
    }

    private void SaveData()
    {
        PlayerPrefs.SetString("Carrots", totalCarrotsCount.ToString());
    }

    private void LoadData()
    {
        double.TryParse(PlayerPrefs.GetString("Carrots"), out totalCarrotsCount);

        UpdateCarrotsText();
    }

    public int GetCurrentMultiplier()
    {
        carrotIncrement = PlayerPrefs.GetInt("Increment",1);
        return carrotIncrement;
    }
    public void UpgradeIncrement()
    {
        if (TryPurchase(PlayerPrefs.GetInt("Increment") * 1000))
        {
            carrotIncrement += 10;
            PlayerPrefs.SetInt("Increment", carrotIncrement);
            incrementPriceText.text = (GetCurrentMultiplier() * 1000).ToString();
        }
        else
        {
            Debug.Log("yetersiz");
        }
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
    public void ClubHouse()
    {
        if (!clubHousePanel.activeSelf)
            clubHousePanel.SetActive(true);
        else
            clubHousePanel.SetActive(false);
    }
}
