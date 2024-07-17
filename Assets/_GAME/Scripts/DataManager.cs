using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Schema;
using System;
using Unity.VisualScripting;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private TextMeshProUGUI cpsText;
    public TextMeshProUGUI incrementPriceText;
    public GameObject goldDoubleButton;
    public GameObject clubHouseButton;

    [Header(" Data ")]
    public double totalGoldText;
    [SerializeField] private int gem;
    [SerializeField] private int frenzyModeMultiplier;
    public int goldIncrement;
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

        // 2x bonusun aktif olup olmadýðýný kontrol ediyoruz.
        if (PlayerPrefs.GetInt("IsDoubleActive", 0) == 1)
        {
            DateTime startTime = DateTime.Parse(PlayerPrefs.GetString("DoubleStartTime", DateTime.Now.ToString()));
            TimeSpan elapsed = DateTime.Now - startTime;

            // Eðer 60 saniye dolmamýþsa, kalan süreyi hesaplýyoruz.
            if (elapsed.TotalSeconds < 60)
            {
                goldIncrement *= 2;
                goldDoubleButton.SetActive(false);
                clubHouseButton.SetActive(false);
                AddGem(0);
                StartCoroutine(ResumeGoldIncrement((float)(60 - elapsed.TotalSeconds)));
            }
            else
            {
                // 60 saniye dolmuþsa, deðerleri normal hale getiriyoruz.
                int oldGold = PlayerPrefs.GetInt("OldGoldIncrement", goldIncrement / 2);
                goldIncrement = oldGold;
                PlayerPrefs.SetInt("Increment", goldIncrement);
                PlayerPrefs.SetInt("IsDoubleActive", 0);
            }
        }

        if (!PlayerPrefs.HasKey("GemBugFix"))
        {
            Debug.Log("çalýþtý1");
            if (PlayerPrefs.HasKey("Gem"))
            {
                AddGem(-gem);
                PlayerPrefs.SetInt("GemBugFix", 1);
                Debug.Log("çalýþtý2");
            }

        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            AddGoldDouble(totalGoldText);
    }

    public IEnumerator ResumeGoldIncrement(float remainingTime)
    {
        yield return new WaitForSeconds(remainingTime);

        goldDoubleButton.SetActive(true);
        clubHouseButton.SetActive(true);
        int oldGold = PlayerPrefs.GetInt("OldGoldIncrement", goldIncrement / 2);
        goldIncrement = oldGold;
        PlayerPrefs.SetInt("Increment", goldIncrement);
        PlayerPrefs.SetInt("IsDoubleActive", 0); // 2x bonusun bittiðini iþaretliyoruz.
    }
    public void GoldIncrementDouble()
    {
        if (TryPurchaseGem(20))
        {
        StartCoroutine(GoldIncrement2X());

        }
        else
        {
            StartCoroutine(UIManager.instance.popUpCreat("NOT ENOUGH ENERGY"));
        }
    }

    public IEnumerator GoldIncrement2X()
    {
        int oldGold = goldIncrement;
        goldIncrement *= 2;
        PlayerPrefs.SetInt("Increment", goldIncrement);
        goldDoubleButton.SetActive(false);
        clubHouseButton.SetActive(false);
        AddGem(0);

        // Coroutine'in baþlama zamanýný kaydediyoruz.
        PlayerPrefs.SetString("DoubleStartTime", DateTime.Now.ToString());
        PlayerPrefs.SetInt("IsDoubleActive", 1);

        yield return new WaitForSeconds(60);

        goldDoubleButton.SetActive(true);
        clubHouseButton.SetActive(true);
        goldIncrement = oldGold;
        PlayerPrefs.SetInt("Increment", goldIncrement);
        PlayerPrefs.SetInt("IsDoubleActive", 0); // 2x bonusun bittiðini iþaretliyoruz.
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

    public void EnergyBuy20()
    {
        if (TryPurchaseGold(10000))
        {
            AddGem(30);
        }
        else
        {
            StartCoroutine(UIManager.instance.popUpCreat("NOT ENOUGH ENERGY"));
        }
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
        if (PlayerPrefs.GetInt("Increment") < 5)
        {
                incrementPriceText.text = (GetCurrentMultiplier() * 1000).ToString();
            if (TryPurchaseGold(PlayerPrefs.GetInt("Increment") * 1000))
            {
                goldIncrement += 1;
                PlayerPrefs.SetInt("Increment", goldIncrement);
                incrementPriceText.text = (GetCurrentMultiplier() * 1000).ToString();
                onUpgradeClubHouse?.Invoke();
            }
            else
            {
                StartCoroutine(UIManager.instance.popUpCreat("NOT ENOUGH GOLD"));
            }
        }
        else if (PlayerPrefs.GetInt("Increment") == 5)
        {
                incrementPriceText.text = 60000.ToString();
            if (TryPurchaseGold(PlayerPrefs.GetInt("Increment") * 10000))
            {
                goldIncrement += 1;
                PlayerPrefs.SetInt("Increment", goldIncrement);
                incrementPriceText.text = (GetCurrentMultiplier() * 10000).ToString();
                onUpgradeClubHouse?.Invoke();
            }
            else
            {
                StartCoroutine(UIManager.instance.popUpCreat("NOT ENOUGH GOLD"));
            }
        }
        else if(PlayerPrefs.GetInt("Increment")>=6)
        {
                incrementPriceText.text = (GetCurrentMultiplier() * 10000).ToString();
            if (TryPurchaseGold(PlayerPrefs.GetInt("Increment") * 10000))
            {
                goldIncrement += 1;
                PlayerPrefs.SetInt("Increment", goldIncrement);
                incrementPriceText.text = (GetCurrentMultiplier() * 10000).ToString();
                onUpgradeClubHouse?.Invoke();
            }
            else
            {
                StartCoroutine(UIManager.instance.popUpCreat("NOT ENOUGH GOLD"));
            }
        }
        else if(PlayerPrefs.GetInt("Increment") >= 10)
        {
                incrementPriceText.text = (GetCurrentMultiplier() * 100000).ToString();
            if (TryPurchaseGold(PlayerPrefs.GetInt("Increment") * 100000))
            {
                goldIncrement += 1;
                PlayerPrefs.SetInt("Increment", goldIncrement);
                incrementPriceText.text = (GetCurrentMultiplier() * 100000).ToString();
                onUpgradeClubHouse?.Invoke();
            }
            else
            {
                StartCoroutine(UIManager.instance.popUpCreat("NOT ENOUGH GOLD"));
            }

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

        SupporterManager.instance.LoadFans();

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
