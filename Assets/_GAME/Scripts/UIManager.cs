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
    [SerializeField] private GameObject inboxPanel;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject clubHousePanel;
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private TextMeshProUGUI popUpPanelText;

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

        LaggedAPIUnity.Instance.CheckRewardAd();
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
    public void PlayStoreLink()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.demir.FootballClicker");
    }

    public void GoldRewards()
    {
        LaggedAPIUnity.Instance.PlayRewardAd();
        LaggedAPIUnity.Instance.CheckRewardAd();
        DataManager.instance.AddGoldFloat(100);
    }
    public IEnumerator PopUpPanelOn(string text)
    {
        popUpPanel.SetActive(true);
        popUpPanelText.text = text;
        yield return new WaitForSeconds(3f);
        popUpPanel.SetActive(false);

    }
}
