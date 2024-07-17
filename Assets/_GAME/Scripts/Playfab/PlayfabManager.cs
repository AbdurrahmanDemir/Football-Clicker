using PlayFab.AuthenticationModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using Mono.CompilerServices.SymbolWriter;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;

    [SerializeField] private DataManager dataManager;

    public GameObject rankPrefabs;
    public Transform rankParent;

    [Header("Elements")]
    public GameObject nameWindow;
    public TMP_InputField nameInput; 


    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        Login();
        
    }

    void Login()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "[YourTitleID]"; 
        }

        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
            
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    public void OnSuccess(LoginResult result)
    {
        Debug.Log("Baþarýlý");
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
            TeamLogoController.instance.menuTeamName.text = name;
        }

        if (name == null)
        {
            nameWindow.SetActive(true);
            //TeamLogoController.instance.LoadTeamLogo(rankPrefabs.transform.GetChild(1)/*.transform.GetChild(0)*/);

        }
    }


    //public void GetPlayerCoinData()
    //{
    //    PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
    //}
    //void OnDataRecieved(GetUserDataResult result)
    //{
    //    if (result.Data != null && result.Data.ContainsKey("Coin"))
    //    {            
    //        dataManager.totalGoldText = double.Parse(result.Data["Coin"].Value) ;
    //    }
    //}

    //public void SavePlayerCoinData()
    //{
    //    var request = new UpdateUserDataRequest
    //    {
    //        Data = new Dictionary<string, string>
    //        {
    //            {"Coin", dataManager.totalGoldText.ToString() }
    //        }
    //    };

    //    PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    //}

    //void OnDataSend(UpdateUserDataResult result)
    //{
    //    Debug.Log("data gönderme baþarýlý");
    //}


    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
            
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        nameWindow.SetActive(false);


    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Update display name!");
        //leaderboardWindow.SetActive(false);
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Baþarýsýz");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName= "TeamGen",
                    Value=score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);

    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Leaderboard gönderme baþarýlý");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "TeamGen",
            StartPosition = 0,
            MaxResultsCount = 50
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in rankParent)
        {
            Destroy(item.gameObject);
        }

        foreach(var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rankPrefabs, rankParent);
            TextMeshProUGUI[] text = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            text[0].text = (item.Position+1).ToString();
            text[1].text = item.DisplayName;
            text[2].text= item.StatValue.ToString();


            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
}
