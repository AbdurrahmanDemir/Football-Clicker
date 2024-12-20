using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIShopManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform elevenPanel;
    [SerializeField] private RectTransform shopPanel;
    [SerializeField] private RectTransform leaderboardPanel;
    [SerializeField] private RectTransform leaguePanel;
    [SerializeField] private RectTransform eventsPanel;
    [SerializeField] private GameObject firstElevenPanel;
    [SerializeField] private GameObject leagueSlider;


    [Header(" Settings ")]
    private Vector2 elevenOpenedPos;
    private Vector2 elevenClosedPos;
    private Vector2 shopClosedPos;
    private Vector2 shopOpenedPos;
    private Vector2 leaderboardOpenedPos;
    private Vector2 leaderboardClosedPos;
    private Vector2 leagueOpenedPos;
    private Vector2 leagueClosedPos;
    private Vector2 eventsOpenedPos;
    private Vector2 eventsClosedPos;

    [SerializeField] private TutorialManager tutorialManager;

    // Start is called before the first frame update
    void Start()
    {
        elevenOpenedPos = Vector2.zero;
        shopOpenedPos = Vector2.zero;
        leaderboardOpenedPos = Vector2.zero;
        leagueOpenedPos = Vector2.zero;
        eventsOpenedPos = Vector2.zero;

        elevenClosedPos = new Vector2(elevenPanel.rect.width, 0);
        shopClosedPos = new Vector2(elevenPanel.rect.width, 0);
        leaderboardClosedPos = new Vector2(elevenPanel.rect.width, 0);
        leagueClosedPos = new Vector2(elevenPanel.rect.width, 0);
        eventsClosedPos= new Vector2(elevenPanel.rect.width, 0);

        elevenPanel.anchoredPosition = elevenClosedPos;
        shopPanel.anchoredPosition = shopClosedPos;
        leaderboardPanel.anchoredPosition = leaderboardClosedPos;
        leaguePanel.anchoredPosition = leagueClosedPos;
        eventsPanel.anchoredPosition = eventsClosedPos;
        leagueSlider.SetActive(false);
    }

    public void HousePanelOpen()
    {
        LeanTween.move(elevenPanel, elevenClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(shopPanel, shopClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaderboardPanel, leaderboardClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaguePanel, leagueClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(eventsPanel, eventsClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        leagueSlider.SetActive(false);
    }

    public void ElevenPanelOpen()
    {        
        LeanTween.cancel(elevenPanel);
        LeanTween.move(elevenPanel, elevenOpenedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(shopPanel, shopClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaderboardPanel, leaderboardClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaguePanel, leagueClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(eventsPanel, eventsClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        leagueSlider.SetActive(false);

        if (!PlayerPrefs.HasKey("TutorialPanel2"))
        {
            tutorialManager.TutorialPanel2Open();
        }
    }

    public void ElevenPanelClose()
    {
        LeanTween.cancel(elevenPanel);
        LeanTween.move(elevenPanel, elevenClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
    }

    public void ShopPanelOpen()
    {
        LeanTween.cancel(shopPanel);
        LeanTween.move(shopPanel, shopOpenedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(elevenPanel, elevenClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaderboardPanel, leaderboardClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaguePanel, leagueClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(eventsPanel, eventsClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        leagueSlider.SetActive(false);
    }

    public void ShopPanelClose()
    {
        LeanTween.cancel(shopPanel);
        LeanTween.move(shopPanel, shopClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
    }

    public void LeaderboardPanelOpen()
    {
        LeanTween.cancel(leaderboardPanel);
        LeanTween.move(leaderboardPanel, leaderboardOpenedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(elevenPanel, elevenClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(shopPanel, shopClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaguePanel, leagueClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(eventsPanel, eventsClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        leagueSlider.SetActive(false);

        PlayfabManager.instance.SendLeaderboard(SupporterManager.instance.GetFansNumber());
    }

    public void LeaderboardPanelClose()
    {
        LeanTween.cancel(leaderboardPanel);
        LeanTween.move(leaderboardPanel, leaderboardClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
    }

    public void LeaguePanelOpen()
    {
        LeanTween.cancel(leaguePanel);
        LeanTween.move(leaguePanel, leagueOpenedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(elevenPanel, elevenClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(shopPanel, shopClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaderboardPanel, leaderboardClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(eventsPanel, eventsClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        leagueSlider.SetActive(false);
    }

    public void LeaguePanelClose()
    {
        LeanTween.cancel(leaguePanel);
        LeanTween.move(leaguePanel, leagueClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
    }

    public void EventsPanelOpen()
    {
        LeanTween.cancel(eventsPanel);
        LeanTween.move(eventsPanel, eventsOpenedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaguePanel, leagueClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(elevenPanel, elevenClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(shopPanel, shopClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaderboardPanel, leaderboardClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        leagueSlider.SetActive(true);
    }
    public void EventsPanelClose()
    {
        LeanTween.cancel(eventsPanel);
        LeanTween.move(eventsPanel, eventsClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        leagueSlider.SetActive(false);
    }
    public void FirstElevenPanel()
    {
        if (firstElevenPanel.activeSelf)
            firstElevenPanel.gameObject.SetActive(false);
        else
            firstElevenPanel.gameObject.SetActive(true);

    }

   
}
