using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        eventMatchSlider.maxValue = 100;
    }

    public void OpenMatchPanel()
    {
        if (DataManager.instance.GetTotalGen() > 1000)
        {
            matchPanel.SetActive(true);
            startButton.SetActive(true);
            int opponentDef = Random.Range(-100, 50) + DataManager.instance.GetDefGen();
            int opponentMid = Random.Range(-100, 50) + DataManager.instance.GetMidGen();
            int opponentFor = Random.Range(-100, 50) + DataManager.instance.GetForGen();

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

            myGoalText.text = "0";
            opponentGoalText.text = "0";

            myTeamNameText.text = "My Team";
            oppenentTeamNameText.text = "Opponent Team";

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
        if (DataManager.instance.GetTotalGen()>1000)
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
        }
        else
        {
            myGoalText.text = myGoal.ToString();
            opponentGoalText.text = opponentGoal.ToString();
            timeText.text = "90'";
            resultText.text = "YOU LOST";
            AddTrophy(-2);
        }
        
    }

    public void CloseMatchPanel()
    {
        matchPanel.SetActive(false);
        isEventMatch = false;

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
    }
}
