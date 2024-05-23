using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MatchManager : MonoBehaviour
{
    public static MatchManager instance;
    [Header("Elements")]
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private GameObject matchPanel;
    [SerializeField] private GameObject startButton;
    [SerializeField] private float timer;
    [SerializeField] private TextMeshProUGUI myGenText;
    [SerializeField] private TextMeshProUGUI oppenentGenText;
    [SerializeField] private TextMeshProUGUI myTeamNameText;
    [SerializeField] private TextMeshProUGUI oppenentTeamNameText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI myGoalText;
    [SerializeField] private TextMeshProUGUI opponentGoalText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Image myTeamLogo;
    [SerializeField] private Image opponentTeamLogo;
    [Header("Settings")]
    private int myGen;
    public int myGoal;
    public int opponentGoal;
    private bool isMatchStart = false;
    int opponentIndex;

    [Header("New Match")]
    MatchEngine matchEngine;
    public GameObject matchScene;


    [Header("Action")]
    public static Action onPlayMatch;



    [Header("Settings")]
    private TeamSO[] teams;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        matchEngine= GetComponent<MatchEngine>();
        teams= LeagueManager.instance.GetTeams();
    }
   
    public void PlayMatch(int oppenentIndex)
    {
        //TeamSO[] teams = LeagueManager.instance.GetTeams();
        TeamSO team = teams[oppenentIndex];


        matchPanel.SetActive(true);

        MatchPanel(oppenentIndex);

    }
    public void MatchPanel(int index)
    {
        TeamSO team = teams[index];

        myGenText.text = DataManager.instance.GetTotalGen().ToString();
        oppenentGenText.text = (team.teamDefGen+ team.teamMidGen+team.teamForGen).ToString();

        matchEngine.OpponentTeamConfig(team.teamDefGen,team.teamMidGen,team.teamForGen);
        matchEngine.MyTeamConfig(DataManager.instance.GetDefGen(), DataManager.instance.GetMidGen(), DataManager.instance.GetForGen());
        matchEngine.CalculateRate();

        myGoalText.text = "0";
        opponentGoalText.text = "0";

        timeText.text = "1'";

        myTeamNameText.text = "My Team";
        oppenentTeamNameText.text = team.teamName;

        myTeamLogo.sprite = null;
        opponentTeamLogo.sprite = team.teamLogo;

        startButton.SetActive(true);

        startButton.GetComponent<Button>().onClick.AddListener(() => StartButton(index));
    }
    public void StartButton(int index)
    {
        TeamSO team = teams[index];
        myGen = DataManager.instance.GetTotalGen();
        int opponentGen = team.teamDefGen + team.teamMidGen + team.teamForGen;
        opponentIndex=index;

        if (DataManager.instance.TryPurchaseGold(team.teamPrice))
        {
            isMatchStart = true;
            startButton.SetActive(false);
            matchScene.SetActive(true);
            onPlayMatch?.Invoke();

            if (!PlayerPrefs.HasKey("TutorialPanel3"))
            {
                tutorialManager.TutorialPanel3Open();
            }


            
        }
        else
        {
            Debug.Log("PARA YOK");
        }



    }

    public void MatchEnd(int myScore, int opponentScore)
    {
        if (myScore > opponentScore)
        {
            myGoal = myScore;
            opponentGoal = opponentScore;
            myGoalText.text = myGoal.ToString();
            opponentGoalText.text = opponentGoal.ToString();
            timeText.text = "90'";
            resultText.text = "KAZANDIN";

            string opponentKey = "Won_" + opponentIndex;
            if (!PlayerPrefs.HasKey(opponentKey))
            {
                LeagueManager.instance.SetLevel();
                SaveWinTeam(opponentKey);
            }
        }
        else
        {
            myGoal = myScore;
            opponentGoal = opponentScore;
            myGoalText.text = myGoal.ToString();
            opponentGoalText.text = opponentGoal.ToString();
            timeText.text = "90'";
            resultText.text = "KAYBETT�N";
        }
    }
    public void CloseMatchPanel()
    {
        matchPanel.SetActive(false);
        LeagueManager.instance.SpawnTeams(teams);
    }
    public void SaveWinTeam(string text)
    {

        PlayerPrefs.SetInt(text, 1);
    }
}
