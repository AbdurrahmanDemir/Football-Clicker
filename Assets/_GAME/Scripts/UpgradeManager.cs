using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;


    [Header(" Settings ")]
    [Tooltip("Value in hertz")]
    [SerializeField] private int addCarrotsFrequency;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("AddCarrots", 1, 1f / addCarrotsFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddCarrots()
    {
        //Debug.Log("Carrots per second = " + GetCarrotsPerSecond());

        double totalCarrots = GetCarrotsPerSecond();

        // At this point we have the amount of carrots we need to add every second
        PitchManager.instance.AddCarrots(totalCarrots / addCarrotsFrequency);
    }

    public double GetCarrotsPerSecond()
    {
        UpgradeSO[] upgrades = ShopManager.instance.GetUpgrades();

        if (upgrades.Length <= 1)
            return 0;

        double totalCarrots = 0;

        for (int i = 0; i < upgrades.Length; i++)
        {
            // Grab the amount of carrots for the upgrade
            double upgradeCarrots = upgrades[i].cpsPerLevel * ShopManager.instance.GetUpgradeLevel(i);
            totalCarrots += upgradeCarrots;
        }

        return totalCarrots;
    }
}
