using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MatchState
{
    attack,
    defence,
}
public class MatchEngine : MonoBehaviour
{
    MatchState matchState;

    [Header("MATCH STATS")]
    [SerializeField] private int myScore=0;
    [SerializeField] private int opponentScore=0;
    [Header("MY TEAM ELEMENTS")]
     float myTeamDefGen;
     float myTeamMidGen;
     float myTeamForGen;
     float myTeamTotalGen;

    [Header("OPPONENT TEAM ELEMENTS")]
     float opponentDefGen;
     float opponentMidGen;
     float opponentForGen;
     float opponentTotalGen;

    [Header("ATTACK STATS")]
    float pass;
    float dribble;
    float shoot;
    float firstPassRate;
    float firstDribbleRate;
    float firstShootRate;
    public static Action onAttack;
    int attackMove=3;

    [Header("DEFENCE STATS")]
    float press;
    float grab;
    float firstPressRate;
    float firstGrabRate;
    public static Action onDefence;
    int defenceMove=3;

    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI defenceMoveText;
    [SerializeField] private TextMeshProUGUI attackMoveText;
    [SerializeField] private TextMeshProUGUI pressRateText;
    [SerializeField] private TextMeshProUGUI grabRateText;
    [SerializeField] private TextMeshProUGUI passRateText;
    [SerializeField] private TextMeshProUGUI dribbleRateText;
    [SerializeField] private TextMeshProUGUI shootRateText;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI myScoreText;
    [SerializeField] private TextMeshProUGUI opponentScoreText;
    [SerializeField] private Slider moveAttackSlider;
    [SerializeField] private Slider moveDefenceSlider;
    [SerializeField] private TextMeshProUGUI moveSliderText;
    [SerializeField] private GameObject attackPanel;
    [SerializeField] private GameObject defencePanel;
    [SerializeField] private AnnouncerPrefabs announcerPrefabs;
    [SerializeField] private Transform announcerParents;
    [SerializeField] private TextMeshProUGUI announcerText;


    private void Start()
    {
        AttackState();
        moveAttackSlider.maxValue = 3;
        moveDefenceSlider.maxValue = 3;
        myScoreText.text = myScore.ToString();
        opponentScoreText.text = opponentScore.ToString();
    }

    private void Update()
    {
        if(attackMove <= 0)
        {
            DefenceState();
        }
        if (defenceMove <= 0)
        {
            AttackState();
        }

        if (myScore >= 3 || opponentScore > 3)
        {
            MatchManager.instance.matchScene.SetActive(false);
            MatchManager.instance.MatchEnd(myScore, opponentScore);
        }


        EnumState();
    }

    public void OpponentTeamConfig(float DefGen, float MidGen, float ForGen)
    {
        opponentDefGen = DefGen;
        opponentMidGen= MidGen;
        opponentForGen = ForGen;
        opponentTotalGen = opponentDefGen + opponentForGen + opponentMidGen;
    }
    public void MyTeamConfig(float DefGen, float MidGen, float ForGen)
    {
        myTeamDefGen = DefGen;
        myTeamMidGen = MidGen;
        myTeamForGen = ForGen;
        myTeamTotalGen = myTeamDefGen + myTeamForGen + myTeamMidGen;
    }

    public void EnumState()
    {
        switch (matchState)
        {
            case MatchState.attack:
                stateText.text = matchState.ToString();
                onAttack?.Invoke();
                break;
            case MatchState.defence:
                stateText.text = matchState.ToString();
                onDefence?.Invoke();
                break;
            default:
                break;
        }
    }

    public void CalculateRate()
    {
        pass = (myTeamTotalGen / (myTeamTotalGen + opponentTotalGen))*100f;
        dribble = ((myTeamMidGen + myTeamForGen) / (myTeamMidGen + myTeamForGen + opponentMidGen + opponentDefGen))*100f;
        shoot = (myTeamForGen / (myTeamForGen + opponentDefGen)) * 100f;
        press = (myTeamTotalGen / (myTeamTotalGen + opponentTotalGen))*100f ;
        grab = (myTeamForGen / (myTeamForGen + opponentDefGen)) * 100f;

        firstPassRate = pass;
        firstDribbleRate = dribble;
        firstShootRate = shoot;
        firstPressRate = press;
        firstGrabRate = grab;

        RateTextUpdate();

        Debug.Log(pass);
        Debug.Log(dribble);
        Debug.Log(shoot);
        Debug.Log(press);
        Debug.Log(grab);
        

    }
    public void AttackState()
    {
        matchState = MatchState.attack;
        stateText.text = matchState.ToString();
        attackPanel.SetActive(true);
        defencePanel.SetActive(false);
        attackMove = 3;
        attackMoveText.text = attackMove.ToString();
        moveAttackSlider.value = attackMove;
    }
    public void DefenceState()
    {
        matchState = MatchState.defence;
        stateText.text = matchState.ToString();
        defencePanel.SetActive(true);
        attackPanel.SetActive(false);
        defenceMove = 3;
        defenceMoveText.text=defenceMove.ToString();
        moveDefenceSlider.value=defenceMove;
    }
    public void PressButton()
    {
        float RandomRate = UnityEngine.Random.Range(0, 100);
        Debug.Log(RandomRate);
        if (RandomRate < press)
        {
            DefenceRate(5f, 2f);
            defenceMove--;          
            MoveTextUpdate();
            AnnouncerText("PRES BAÞARILI OLUYOR");
        }
        else
        {
            DefenceRate(-5f, -2f);
            defenceMove--;
            MoveTextUpdate();
            AnnouncerText("PRES BAÞARILI OLUYOR");
        }
        RateTextUpdate();
    }
    public void GrabButton()
    {
        float RandomRate = UnityEngine.Random.Range(0, 100);
        Debug.Log(RandomRate);

        if (RandomRate < grab)
        {
            AttackState();
            MoveTextUpdate();
            AnnouncerText("TOP ÇALMA BAÞARILI ÞÝMDÝ ATAK SIRASI");
        }
        else
        {
            opponentScore += 1;
            opponentScoreText.text = opponentScore.ToString();
            press = firstPressRate;
            grab = firstGrabRate;
            AttackState();
            AnnouncerText("VE GOOL TOP ÇALMA BAÞARISIZ OLDU");
        }
        RateTextUpdate();
    }
    public void PassButton()
    {
        float RandomRate = UnityEngine.Random.Range(0, 100);
        Debug.Log(RandomRate);

        if (RandomRate < pass)
        {
            AttackRate(10f, 5f, 3f);
            attackMove--;
            AnnouncerText("GÜZEL PAS");
            MoveTextUpdate();
        }
        else
        {
            AttackRate(-5f, -5f, -3f);
            DefenceState();
            AnnouncerText("KÖTÜ PAS BAÞARISIZ");
            MoveTextUpdate();
        }
        RateTextUpdate();
    }
    public void DribbleButton()
    {
        float RandomRate = UnityEngine.Random.Range(0, 100);
        Debug.Log(RandomRate);

        if (RandomRate < dribble)
        {
            AttackRate(0f, 5f, 5f);
            attackMove--;
            AnnouncerText("MUHTEÞEM BÝR KOÞU");
            MoveTextUpdate();
        }
        else
        {
            AttackRate(-8f, -5f, -5f);
            DefenceState();
            AnnouncerText("DRÝBLÝNG BAÞARISIZ");
            MoveTextUpdate();
        }
        RateTextUpdate();
    }
    public void ShootButton()
    {
        float RandomRate = UnityEngine.Random.Range(0, 100);
        Debug.Log(RandomRate);

        if (RandomRate < shoot)
        {
            myScore += 1;
            myScoreText.text = myScore.ToString();
            AnnouncerText("OLALA HARÝKA BÝR GOL");
            pass = firstPassRate;
            dribble = firstDribbleRate;
            shoot = firstShootRate;
            DefenceState();
            MoveTextUpdate();
        }
        else
        {
            AttackRate(-10f, -8f, -5f);
            DefenceState();
            AnnouncerText("ÇOK KÖTÜ BÝR ÞUT");
            MoveTextUpdate();


        }
        RateTextUpdate();
    }

    void AttackRate(float passRate, float dribbleRate, float shootRate)
    {
        pass += passRate;
        dribble += dribbleRate;
        shoot += shootRate;
    }
    void DefenceRate(float pressRate, float grabRate)
    {
        press += pressRate;
        grab+= grabRate;
    }

    void RateTextUpdate()
    {
        passRateText.text = pass.ToString("F1");
        dribbleRateText.text = dribble.ToString("F1");
        shootRateText.text = shoot.ToString("F1");
        pressRateText.text = press.ToString("F1");
        grabRateText.text = grab.ToString("F1");
    }
    void MoveTextUpdate()
    {
        attackMoveText.text = attackMove.ToString();
        defenceMoveText.text = defenceMove.ToString();
        moveAttackSlider.value = attackMove;
        moveDefenceSlider.value = defenceMove;
    }

    void AnnouncerText(string text)
    {
        AnnouncerPrefabs announcer = Instantiate(announcerPrefabs, announcerParents);
        announcer.Config(text);
    }

    


}
