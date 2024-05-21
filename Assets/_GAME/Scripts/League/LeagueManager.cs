using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class LeagueManager : MonoBehaviour
//{
//    public static LeagueManager instance;

//    [Header("Teams")]
//    [SerializeField] private TeamSO[] teams;

//    [Header("Settings")]
//    [SerializeField] private TeamButton playButton;
//    [SerializeField] private Transform teamsButtonsParent;
//    [SerializeField] private RectTransform[] levelButtonParents;
//    int currentLevel;

//    [Header("Elements")]
//    [SerializeField] private Sprite leagueTeamImage;

//    private void Awake()
//    {
//        if (instance == null)
//            instance = this;
//        else
//            Destroy(gameObject);
//    }
//    void Start()
//    {
//        SpawnTeams();
//        GetLevel();
//        Debug.Log(GetLevel());
//    }

//    public void SpawnTeams()
//    {
//        for (int i = 0; i < teams.Length; i++)
//        {
//            SpawnTeam(i, levelButtonParents[i]);
//        }
//    }
//    public void SpawnTeam(int index, Transform levelButtonParent)
//    {
//        TeamButton teamButtonInstance= Instantiate(playButton, levelButtonParent);
//        TeamSO team = teams[index];

//        if (GetLevel() + 1 > index)
//        {
//            teamButtonInstance.GetComponent<Button>().interactable = true;
//            teamButtonInstance.GetComponent<Image>().sprite = leagueTeamImage;
//        }
//        else
//        {
//            teamButtonInstance.GetComponent<Button>().interactable = false;
//        }


//        Sprite logo = team.teamLogo;
//        string name= team.teamName;
//        string gen = (team.teamDefGen+team.teamMidGen+ team.teamForGen).ToString();
//        string price= team.teamPrice.ToString();


//        teamButtonInstance.Config(logo, name, gen,price);

//        teamButtonInstance.GetPlayButton().onClick.AddListener(() => TeamButtonCallback(index));

//    }

//    public void TeamButtonCallback(int index)
//    {

//        MatchManager.instance.PlayMatch(index);
//    }

//    public TeamSO[] GetTeams()
//    {
//        return teams;
//    }

//    public int SetLevel()
//    {
//        PlayerPrefs.SetInt("Level", GetLevel() + 1);
//        return currentLevel;
//    }
//    public int GetLevel()
//    {
//        if (PlayerPrefs.HasKey("Level"))
//            currentLevel = PlayerPrefs.GetInt("Level");
//        else
//            currentLevel=0;

//        return currentLevel;
//    }
//}
public class LeagueManager : MonoBehaviour
{
    public static LeagueManager instance;

    [Header("Teams")]
    [SerializeField] private TeamSO[] teams;

    [Header("Settings")]
    [SerializeField] private TeamButton playButton;
    [SerializeField] private Transform teamsButtonsParent;
    [SerializeField] private RectTransform[] levelButtonParents;
    int currentLevel;

    [Header("Elements")]
    [SerializeField] private Sprite leagueTeamImage;

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
        SpawnTeams();
        Debug.Log(GetLevel());
    }

    public void SpawnTeams()
    {
        for (int i = 0; i < teams.Length; i++)
        {
            SpawnTeam(i, levelButtonParents[i]);
        }
    }
    public void SpawnTeam(int index, Transform levelButtonParent)
    {
        TeamButton teamButtonInstance = Instantiate(playButton, levelButtonParent);
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
        string name = team.teamName;
        string gen = (team.teamDefGen + team.teamMidGen + team.teamForGen).ToString();
        string price = team.teamPrice.ToString();


        teamButtonInstance.Config(logo, name, gen, price);

        teamButtonInstance.GetPlayButton().onClick.AddListener(() => TeamButtonCallback(index));

    }

    public void TeamButtonCallback(int index)
    {

        MatchManager.instance.PlayMatch(index);
    }

    public TeamSO[] GetTeams()
    {
        return teams;
    }

    public void SetLevel()
    {
        currentLevel = GetLevel() + 1;
        PlayerPrefs.SetInt("Level", currentLevel);
    }
    public int GetLevel()
    {
        if (PlayerPrefs.HasKey("Level"))
            currentLevel = PlayerPrefs.GetInt("Level");
        else
            currentLevel = 0;

        return currentLevel;
    }
}

