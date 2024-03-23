using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MatchState
{
    attack,
    defence,
}
public class MatchEngine : MonoBehaviour
{
    MatchState matchState;

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
    public static Action onAttack;
    int attackMove=3;

    [Header("DEFENCE STATS")]
    float press;
    float grab;
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


    private void Start()
    {
        matchState = MatchState.attack;
        attackMoveText.text = attackMove.ToString();
        defenceMoveText.text=defenceMove.ToString();
    }

    private void Update()
    {
        if(attackMove <= 0)
        {
            matchState=MatchState.defence;
        }
        if (defenceMove <= 0)
        {
            matchState = MatchState.attack;
        }
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
                onAttack?.Invoke();
                break;
            case MatchState.defence:                
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

        RateTextUpdate();

        Debug.Log(pass);
        Debug.Log(dribble);
        Debug.Log(shoot);
        Debug.Log(press);
        Debug.Log(grab);
        

    }

    public void PressButton()
    {
        float RandomRate = 50f;

        if (RandomRate < press)
        {
            press += 10f;
            defenceMove--;
            attackMoveText.text = attackMove.ToString();
            defenceMoveText.text = defenceMove.ToString();
        }
        else
        {
            press -= 10f;
            defenceMove--;
            attackMoveText.text = attackMove.ToString();
            defenceMoveText.text = defenceMove.ToString();
        }
        RateTextUpdate();
    }
    public void GrabButton()
    {
        float RandomRate = 50f;

        if (RandomRate < grab)
        {
            matchState = MatchState.attack;
            defenceMove--;
            attackMoveText.text = attackMove.ToString();
            defenceMoveText.text = defenceMove.ToString();
        }
        else
        {
            Debug.Log("Baþarýsýz Kayma");
            defenceMove--;
            attackMoveText.text = attackMove.ToString();
            defenceMoveText.text = defenceMove.ToString();
        }
        RateTextUpdate();
    }
    public void PassButton()
    {
        float RandomRate = 50f;

        if (RandomRate < pass)
        {
            pass += 10f;
            dribble += 10f;
            shoot += 10f;
            attackMove--;
            attackMoveText.text = attackMove.ToString();
            defenceMoveText.text = defenceMove.ToString();
        }
        else
        {
            matchState = MatchState.defence;
            attackMove--;
            attackMoveText.text = attackMove.ToString();
            defenceMoveText.text = defenceMove.ToString();
        }
        RateTextUpdate();
    }
    public void DribbleButton()
    {
        float RandomRate = 50f;

        if (RandomRate < dribble)
        {
            shoot += 10f;
            attackMove--;
            attackMoveText.text = attackMove.ToString();
            defenceMoveText.text = defenceMove.ToString();
        }
        else
        {
            matchState = MatchState.defence;
            attackMove--;
            attackMoveText.text = attackMove.ToString();
            defenceMoveText.text = defenceMove.ToString();
        }
        RateTextUpdate();
    }
    public void ShootButton()
    {
        float RandomRate = 50f;

        if (RandomRate < shoot)
        {
            Debug.Log("GOOOOLL");
            attackMove--;
            attackMoveText.text = attackMove.ToString();
            defenceMoveText.text = defenceMove.ToString();
        }
        else
        {
            matchState = MatchState.defence;
            attackMove--;
            attackMoveText.text = attackMove.ToString();
            defenceMoveText.text = defenceMove.ToString();
        }
        RateTextUpdate();
    }

    void RateTextUpdate()
    {
        passRateText.text = pass.ToString();
        dribbleRateText.text = dribble.ToString();
        shootRateText.text = shoot.ToString();
        pressRateText.text = press.ToString();
        grabRateText.text = grab.ToString();
    }




}
