using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EventMatch : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject matchPanel;
    [SerializeField] private Slider eventMatchSlider;
    [SerializeField] private int trophy;
    [SerializeField] private TextMeshProUGUI trophyText;
    [SerializeField] private TextMeshProUGUI myGenText;
    [SerializeField] private TextMeshProUGUI oppenentGenText;
    [SerializeField] private TextMeshProUGUI myTeamNameText;
    [SerializeField] private TextMeshProUGUI oppenentTeamNameText;
    [SerializeField] private TextMeshProUGUI myGoalText;
    [SerializeField] private TextMeshProUGUI opponentGoalText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject startButton;
    [SerializeField] private Image myTeamLogo;
    [SerializeField] private Image opponentTeamLogo;
    [SerializeField] private string[] opponentTeamNames;
    [SerializeField] private string[] leaguesTitle;
    [SerializeField] private Slider[] leaguesSlider;
    [SerializeField] private GameObject[] leaguesCompletedImage;
    public UIShopManager uiManager;
    [Header("New Match")]
    public MatchEngine matchEngine;
    public GameObject matchScene;
    public GameObject matchEngineScene;
    public  bool isEventMatch;
    [SerializeField] private GameObject menuBar;

    private void Start()
    {
        LoadTrophy();
        eventMatchSlider.maxValue = 1000;
    }

    public void OpenMatchPanel()
    {
        if (DataManager.instance.GetTotalGen() > 0)
        {
            matchPanel.SetActive(true);
            startButton.SetActive(true);

            if (DataManager.instance.GetTotalGen() < 300)
            {
                int opponentDef = Random.Range(-100, 10) + DataManager.instance.GetDefGen();
                int opponentMid = Random.Range(-100, 10) + DataManager.instance.GetMidGen();
                int opponentFor = Random.Range(-100, 10) + DataManager.instance.GetForGen();

                if (opponentDef < 0)
                    opponentDef = 0;
                if (opponentMid < 0)
                    opponentMid = 0;
                if (opponentFor < 0)
                    opponentFor = 0;


                myGenText.text = DataManager.instance.GetTotalGen().ToString();
                oppenentGenText.text = (opponentDef + opponentMid + opponentFor).ToString();

                matchEngine.OpponentTeamConfig(opponentDef, opponentMid, opponentFor);
                matchEngine.MyTeamConfig(DataManager.instance.GetDefGen(), DataManager.instance.GetMidGen(), DataManager.instance.GetForGen());
                matchEngine.CalculateRate();
            }
            else if (DataManager.instance.GetTotalGen() >= 300 && DataManager.instance.GetTotalGen() < 1000)
            {
                int opponentDef = Random.Range(-150, 50) + DataManager.instance.GetDefGen();
                int opponentMid = Random.Range(-150, 50) + DataManager.instance.GetMidGen();
                int opponentFor = Random.Range(-150, 50) + DataManager.instance.GetForGen();

                if (opponentDef < 0)
                    opponentDef = 0;
                if (opponentMid < 0)
                    opponentMid = 0;
                if (opponentFor < 0)
                    opponentFor = 0;

                myGenText.text = DataManager.instance.GetTotalGen().ToString();
                oppenentGenText.text = (opponentDef + opponentMid + opponentFor).ToString();

                matchEngine.OpponentTeamConfig(opponentDef, opponentMid, opponentFor);
                matchEngine.MyTeamConfig(DataManager.instance.GetDefGen(), DataManager.instance.GetMidGen(), DataManager.instance.GetForGen());
                matchEngine.CalculateRate();
            }
            else if (DataManager.instance.GetTotalGen() >= 1000 && DataManager.instance.GetTotalGen() < 2000)
            {
                int opponentDef = Random.Range(-350, 200) + DataManager.instance.GetDefGen();
                int opponentMid = Random.Range(-350, 200) + DataManager.instance.GetMidGen();
                int opponentFor = Random.Range(-350, 200) + DataManager.instance.GetForGen();

                if (opponentDef < 0)
                    opponentDef = 0;
                if (opponentMid < 0)
                    opponentMid = 0;
                if (opponentFor < 0)
                    opponentFor = 0;

                myGenText.text = DataManager.instance.GetTotalGen().ToString();
                oppenentGenText.text = (opponentDef + opponentMid + opponentFor).ToString();

                matchEngine.OpponentTeamConfig(opponentDef, opponentMid, opponentFor);
                matchEngine.MyTeamConfig(DataManager.instance.GetDefGen(), DataManager.instance.GetMidGen(), DataManager.instance.GetForGen());
                matchEngine.CalculateRate();

            }
            else
            {
                int opponentDef = Random.Range(-500, 200) + DataManager.instance.GetDefGen();
                int opponentMid = Random.Range(-500, 200) + DataManager.instance.GetMidGen();
                int opponentFor = Random.Range(-500, 200) + DataManager.instance.GetForGen();

                if (opponentDef < 0)
                    opponentDef = 0;
                if (opponentMid < 0)
                    opponentMid = 0;
                if (opponentFor < 0)
                    opponentFor = 0;

                myGenText.text = DataManager.instance.GetTotalGen().ToString();
                oppenentGenText.text = (opponentDef + opponentMid + opponentFor).ToString();

                matchEngine.OpponentTeamConfig(opponentDef, opponentMid, opponentFor);
                matchEngine.MyTeamConfig(DataManager.instance.GetDefGen(), DataManager.instance.GetMidGen(), DataManager.instance.GetForGen());
                matchEngine.CalculateRate();
            }


        


            myGoalText.text = "0";
            opponentGoalText.text = "0";

            myTeamNameText.text = "My Team";
            int RandomNames = Random.Range(0, opponentTeamNames.Length);
            oppenentTeamNameText.text = opponentTeamNames[RandomNames];

            //myTeamLogo.sprite = null;
            //opponentTeamLogo.sprite = null;
        }
        else
        {
            UIManager.instance.eventPlayMatchError.SetActive(true);
        }

        
    }
    public void EventMatchPlay()
    {
        if (DataManager.instance.GetTotalGen()>0)
        {
            uiManager.LeaguePanelOpen();
            matchScene.SetActive(true);
            matchEngineScene.SetActive(true);
            startButton.SetActive(false);

            menuBar.SetActive(false);

            isEventMatch = true;

        }
        else
        {
            UIManager.instance.eventPlayMatchError.SetActive(true);
        }
    }
    public void EventMatchOver(int myGoal, int opponentGoal)
    {
        menuBar.SetActive(true);
        matchScene.SetActive(false);
        matchEngineScene.SetActive(false);
        uiManager.LeaguePanelClose();
        uiManager.EventsPanelOpen();
        isEventMatch = false;
        if (myGoal > opponentGoal)
        {
            myGoalText.text = myGoal.ToString();
            opponentGoalText.text = opponentGoal.ToString();
            timeText.text = "90'";
            resultText.text = "YOU WON";
            AddTrophy(5);

            if (trophy >= 0 && trophy < 50)
                DataManager.instance.AddGoldDouble(3000);
            if (trophy >= 50 && trophy < 200)
                DataManager.instance.AddGoldDouble(5000);
            if (trophy >= 200 && trophy < 500)
                DataManager.instance.AddGoldDouble(8000);
            if (trophy >= 5000 && trophy < 1000)
                DataManager.instance.AddGoldDouble(12000);
            if (trophy >= 1000 && trophy < 2000)
                DataManager.instance.AddGoldDouble(15000);
            if (trophy >= 2000 && trophy < 5000)
                DataManager.instance.AddGoldDouble(20000);

            int winNumber = PlayerPrefs.GetInt("WinNumber", 1);
            winNumber++;
            PlayerPrefs.SetInt("WinNumber", winNumber);
            LeagueControl();
            SupporterManager.instance.LoadFans();
        }
        else
        {
            myGoalText.text = myGoal.ToString();
            opponentGoalText.text = opponentGoal.ToString();
            timeText.text = "90'";
            resultText.text = "YOU LOST";
            AddTrophy(-2);
            LeagueControl();
        }
        
    }

    public void CloseMatchPanel()
    {
        matchPanel.SetActive(false);
        isEventMatch = false;

    }

    void LeagueControl()
    {
        if (trophy < 50)
        {
            leaguesSlider[0].value = trophy;
            leaguesSlider[0].maxValue = 50;
            return;
        }
        else
        {
            leaguesCompletedImage[0].gameObject.SetActive(true);
        }
        if(trophy >= 50 && trophy < 200)
        {
            leaguesSlider[1].value = trophy;
            leaguesSlider[1].maxValue = 200;
            return;

        }
        else
        {
            leaguesCompletedImage[0].gameObject.SetActive(true);
            leaguesCompletedImage[1].gameObject.SetActive(true);
        }
        if (trophy >= 200 && trophy < 500)
        {
            leaguesSlider[2].value = (float)trophy;
            leaguesSlider[2].maxValue = 500;
            return;

        }
        else
        {
            leaguesCompletedImage[0].gameObject.SetActive(true);
            leaguesCompletedImage[1].gameObject.SetActive(true);
            leaguesCompletedImage[2].gameObject.SetActive(true);

        }
        if (trophy >= 500 && trophy < 1000)
        {
            leaguesSlider[3].value = trophy;
            leaguesSlider[3].maxValue = 1000;
            return;
        }
        else
        {
            leaguesCompletedImage[0].gameObject.SetActive(true);
            leaguesCompletedImage[1].gameObject.SetActive(true);
            leaguesCompletedImage[2].gameObject.SetActive(true);
            leaguesCompletedImage[3].gameObject.SetActive(true);
        }
        if (trophy >= 1000 && trophy <= 2000)
        {
            leaguesSlider[4].value = trophy;
            leaguesSlider[4].maxValue = 2000;
            return;
        }
        else
        {
            leaguesCompletedImage[0].gameObject.SetActive(true);
            leaguesCompletedImage[1].gameObject.SetActive(true);
            leaguesCompletedImage[2].gameObject.SetActive(true);
            leaguesCompletedImage[3].gameObject.SetActive(true);
            leaguesCompletedImage[4].gameObject.SetActive(true);
        }
        if (trophy >=2000 && trophy <= 5000)
        {
            leaguesSlider[5].value = trophy;
            leaguesSlider[5].maxValue = 5000;
            return;
        }
        else
        {
            leaguesCompletedImage[0].gameObject.SetActive(true);
            leaguesCompletedImage[1].gameObject.SetActive(true);
            leaguesCompletedImage[2].gameObject.SetActive(true);
            leaguesCompletedImage[3].gameObject.SetActive(true);
            leaguesCompletedImage[4].gameObject.SetActive(true);
            leaguesCompletedImage[5].gameObject.SetActive(true);
        }

    }

    public void AddTrophy(int trophyy)
    {
        trophy += trophyy;
        trophyText.text = trophy.ToString();
        eventMatchSlider.value = trophy;
        PlayerPrefs.SetInt("trophy", trophy);
    }
    public void LoadTrophy()
    {
        if (PlayerPrefs.HasKey("trophy"))
        {
            trophy = PlayerPrefs.GetInt("trophy");
            
        }
        else
        {
            trophy = 0;
        }

        trophyText.text = trophy.ToString();
        eventMatchSlider.value = trophy;
        LeagueControl();
    }
}
