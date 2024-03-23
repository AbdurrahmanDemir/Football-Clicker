using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MatchManager : MonoBehaviour
{
    public static MatchManager instance;
    [Header("Elements")]
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
    private int myGoal;
    private int opponentGoal;
    private bool isMatchStart=false;

    [Header("New Match")]
    MatchEngine matchEngine;
    [SerializeField] private GameObject matchScene;





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

        myGenText.text = PitchManager.instance.GetTotalGen().ToString();
        oppenentGenText.text = team.teamGen.ToString();

        matchEngine.OpponentTeamConfig(team.teamGen, team.teamGen, team.teamGen);
        matchEngine.MyTeamConfig(PitchManager.instance.GetTotalGen(), PitchManager.instance.GetTotalGen(), PitchManager.instance.GetTotalGen());
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
        myGen = PitchManager.instance.GetTotalGen();
        int opponentGen = team.teamGen;

        if (PitchManager.instance.TryPurchase(team.teamPrice))
        {
            isMatchStart = true;
            startButton.SetActive(false);
            matchScene.SetActive(true);


            //if (myGen > opponentGen)
            //{
            //    myGoal = Random.Range(0, 10);
            //    opponentGoal = Random.Range(0, myGoal);
            //    myGoalText.text = myGoal.ToString();
            //    opponentGoalText.text = opponentGoal.ToString();
            //    timeText.text = "90'";
            //    resultText.text = "KAZANDIN";
            //    LeagueManager.instance.SetLevel();
            //}
            //else
            //{
            //    opponentGoal = Random.Range(0, 10);
            //    myGoal = Random.Range(0, opponentGoal);
            //    myGoalText.text = myGoal.ToString();
            //    opponentGoalText.text = opponentGoal.ToString();
            //    timeText.text = "90'";
            //    resultText.text = "KAYBETTÝN";
            //}
        }
        else
        {
            Debug.Log("PARA YOK");
        }



    }
    public void CloseMatchPanel()
    {
        matchPanel.SetActive(false);

        LeagueManager.instance.SpawnTeams();
    }
}
