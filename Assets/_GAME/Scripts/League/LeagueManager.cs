using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeagueManager : MonoBehaviour
{
    public static LeagueManager instance;

    [Header("Teams")]
    [SerializeField] private TeamSO[] league1teams;
    [SerializeField] private TeamSO[] league2teams;

    [Header("Settings")]
    [SerializeField] private TeamButton playButton;
    [SerializeField] private Transform teamsButtonsParent;
    [SerializeField] private RectTransform[] levelButtonParents;
    int currentLevel;
    int currentLeague;

    [Header("Elements")]
    [SerializeField] private Sprite leagueTeamImage;
    [SerializeField] private TextMeshProUGUI leagueTitleText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }
    void Start()
    {
        GetLevel();
        GetLeagueLevel();


        switch (GetLeagueLevel())
        {
            case 1:
                SpawnTeams(league1teams);
                leagueTitleText.text = "Bronze League";
                break;
            case 2:
                SpawnTeams(league2teams);
                leagueTitleText.text = "Silver League";
                break;
        }


        Debug.Log(GetLevel());
        Debug.Log(GetLeagueLevel());
    }

    public void SpawnTeams(TeamSO[] teams)
    {
        for (int i = 0; i < teams.Length; i++)
        {
            if (levelButtonParents[i].childCount > 0)
            {
                for (int j = 0; j < levelButtonParents[i].childCount; j++)
                {
                    Destroy(levelButtonParents[i].GetChild(j).gameObject);
                }
            }
            SpawnTeam(i, levelButtonParents[i],teams);
        }
    }

    public void SpawnTeam(int index, Transform levelButtonParent, TeamSO[] teams)
    {
        TeamButton teamButtonInstance= Instantiate(playButton, levelButtonParent);
        TeamSO team = teams[index];

        if (GetLevel() + 1 > index)
        {
            teamButtonInstance.GetComponent<Button>().interactable = true;
            teamButtonInstance.GetComponent<Image>().sprite = leagueTeamImage;
        }
        else
        {
            teamButtonInstance.GetComponent<Button>().interactable = false;
        }
        

        Sprite logo = team.teamLogo;
        string name= team.teamName;
        string gen = (team.teamDefGen+team.teamMidGen+ team.teamForGen).ToString();
        string price= team.teamPrice.ToString();


        teamButtonInstance.Config(logo, name, gen,price);

        teamButtonInstance.GetPlayButton().onClick.AddListener(() => TeamButtonCallback(index));

    }

    public void TeamButtonCallback(int index)
    {

        MatchManager.instance.PlayMatch(index);
    }

    public TeamSO[] GetTeams()
    {
        //return league1teams;
        switch (GetLeagueLevel())
        {
            case 1:
                return league1teams;
                break;
            case 2:
                return league2teams;
                break;
            default: return league1teams;
        }
    }

    public void SetLevel()
    {
        currentLevel = GetLevel() + 1;
        PlayerPrefs.SetInt("Level", currentLevel);

        if (currentLevel == 11)
        {
            currentLeague= GetLeagueLevel() + 1;
            PlayerPrefs.SetInt("league", currentLeague);
            currentLevel = 0;
            PlayerPrefs.SetInt("Level", currentLevel);
        }
        
    }
    public int GetLevel()
    {
        if (PlayerPrefs.HasKey("Level"))
            currentLevel = PlayerPrefs.GetInt("Level");
        else
            currentLevel = 0;

        return currentLevel;
    }
    public int GetLeagueLevel()
    {
        if (PlayerPrefs.HasKey("league"))
            currentLeague = PlayerPrefs.GetInt("league");
        else
            currentLeague = 1;

        return currentLeague;
    }
}
