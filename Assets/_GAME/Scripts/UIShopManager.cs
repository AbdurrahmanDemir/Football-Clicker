using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform elevenPanel;
    [SerializeField] private RectTransform shopPanel;
    [SerializeField] private RectTransform leaderboardPanel;
    [SerializeField] private RectTransform leaguePanel;
    [SerializeField] private GameObject firstElevenPanel;


    [Header(" Settings ")]
    private Vector2 elevenOpenedPos;
    private Vector2 elevenClosedPos;
    private Vector2 shopClosedPos;
    private Vector2 shopOpenedPos;
    private Vector2 leaderboardOpenedPos;
    private Vector2 leaderboardClosedPos;
    private Vector2 leagueOpenedPos;
    private Vector2 leagueClosedPos;

    [SerializeField] private TutorialManager tutorialManager;

    // Start is called before the first frame update
    void Start()
    {
        elevenOpenedPos = Vector2.zero;
        shopOpenedPos = Vector2.zero;
        leaderboardOpenedPos = Vector2.zero;
        leagueOpenedPos = Vector2.zero;

        elevenClosedPos = new Vector2(elevenPanel.rect.width, 0);
        shopClosedPos = new Vector2(elevenPanel.rect.width, 0);
        leaderboardClosedPos = new Vector2(elevenPanel.rect.width, 0);
        leagueClosedPos = new Vector2(elevenPanel.rect.width, 0);

        elevenPanel.anchoredPosition = elevenClosedPos;
        shopPanel.anchoredPosition = shopClosedPos;
        leaderboardPanel.anchoredPosition = leaderboardClosedPos;
        leaguePanel.anchoredPosition = leagueClosedPos;
            
    }

    public void ElevenPanelOpen()
    {        
        LeanTween.cancel(elevenPanel);
        LeanTween.move(elevenPanel, elevenOpenedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(shopPanel, shopClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaderboardPanel, leaderboardClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaguePanel, leagueClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);

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
    }

    public void ShopPanelClose()
    {
        LeanTween.cancel(shopPanel);
        LeanTween.move(shopPanel, shopClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
    }

    public void LeaderboardPanelOpen()
    {
        LeanTween.cancel(leaderboardPanel);
        LeanTween.move(leaderboardPanel, leagueOpenedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(elevenPanel, elevenClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(shopPanel, shopClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.move(leaguePanel, leagueClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);

        PlayfabManager.instance.SendLeaderboard(PitchManager.instance.totalTeamGen);
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
    }

    public void LeaguePanelClose()
    {
        LeanTween.cancel(leaguePanel);
        LeanTween.move(leaguePanel, leagueClosedPos, .3f).setEase(LeanTweenType.easeInOutSine);
    }

    public void FirstElevenPanel()
    {
        if (firstElevenPanel.activeSelf)
            firstElevenPanel.gameObject.SetActive(false);
        else
            firstElevenPanel.gameObject.SetActive(true);

    }
}
