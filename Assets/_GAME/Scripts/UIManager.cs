using LayerLab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;


    [Header("Elements")]
    [SerializeField] private Slider powerSlider;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject fixPanel;
    [SerializeField] private GameObject popUpName;
    [SerializeField] private GameObject inboxPanel;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject clubHousePanel;
    [SerializeField] private GameObject _popUpPrefabs;
    [SerializeField] private TextMeshProUGUI _popUpPrefabsText;
    public GameObject eventPlayMatchError;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        powerSlider.maxValue = 1000;
        UpdateSlider();
        //LaggedAPIUnity.Instance.CheckRewardAd();
    }
    public void UpdateSlider()
    {
        powerSlider.value = DataManager.instance.GetTotalGen();
    }

    public void GetSettingsPanel()
    {
        if (settingsPanel.activeSelf)
            settingsPanel.SetActive(false);
        else
            settingsPanel.SetActive(true);
    }
    public void GetInboxPanel()
    {
        if (inboxPanel.activeSelf)
            inboxPanel.SetActive(false);
        else
            inboxPanel.SetActive(true);
    }

    public void GetQuestPanel()
    {
        if (questPanel.activeSelf)
            questPanel.SetActive(false);
        else
            questPanel.SetActive(true);
    }
    public void NameChange()
    {
        popUpName.SetActive(true);
    }
    public void ClubHouse()
    {
        if (!clubHousePanel.activeSelf)
            clubHousePanel.SetActive(true);
        else
            clubHousePanel.SetActive(false);
    }
    public void DiscordLink()
    {
        Application.OpenURL("https://discord.gg/f2vDJdq6d5");
        if (!PlayerPrefs.HasKey("discordGift"))
        {
            DataManager.instance.AddGoldFloat(500);
            PlayerPrefs.SetInt("discordGift", 1);
        }
    }
    public void FacebookLink()
    {
        Application.OpenURL("https://www.facebook.com/people/Football-Clicker/61561667848741/");
        if (!PlayerPrefs.HasKey("discordGift"))
        {
            DataManager.instance.AddGoldFloat(500);
            PlayerPrefs.SetInt("discordGift", 1);

        }
    }

    public void FixPanel()
    {
        if (!PlayerPrefs.HasKey("FixPanel"))
        {
            fixPanel.SetActive(true);            
        }
        else
        {
            fixPanel.SetActive(false);
        }
    }
    public void PlayStoreLink()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.demir.FootballClicker");
    }

    public void GoldRewards()
    {
        //LaggedAPIUnity.Instance.PlayRewardAd();
        //LaggedAPIUnity.Instance.CheckRewardAd();
        DataManager.instance.AddGoldFloat(100);
    }
    public IEnumerator popUpCreat(string massage)
    {

        _popUpPrefabs.SetActive(true);
        _popUpPrefabsText.text = massage;

        yield return new WaitForSeconds(1.8f);
        _popUpPrefabs.SetActive(false);

    }
}
