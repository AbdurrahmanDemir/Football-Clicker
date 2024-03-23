using System;
using System.Collections;
using System.Collections.Generic;
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
    public float myTeamDefGen;
    public float myTeamMidGen;
    public float myTeamForGen;
    public float myTeamTotalGen;

    [Header("OPPONENT TEAM ELEMENTS")]
    public float opponentDefGen;
    public float opponentMidGen;
    public float opponentForGen;
    public float opponentTotalGen;

    [Header("ATTACK STATS")]
    float pass;
    float dribble;
    float shoot;
    public static Action onAttack;
    int attackMove;

    [Header("DEFENCE STATS")]
    float press;
    float grab;
    public static Action onDefence;
    int defenceMove;

    private void Start()
    {
        matchState = MatchState.attack;

        CalculateRate();
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

    void CalculateRate()
    {
        pass = (myTeamTotalGen / (myTeamTotalGen + opponentTotalGen))*100f;
        dribble = ((myTeamMidGen + myTeamForGen) / (myTeamMidGen + myTeamForGen + opponentMidGen + opponentDefGen))*100f;
        shoot = (myTeamForGen / (myTeamForGen + opponentDefGen)) * 100f;
        press = (myTeamTotalGen / (myTeamTotalGen + opponentTotalGen)) ;
        grab = (myTeamForGen / (myTeamForGen + opponentDefGen)) * 100f;

        Debug.Log(pass);
        Debug.Log(dribble);
        Debug.Log(shoot);
        Debug.Log(press);
        Debug.Log(grab);
        

    }

    void PressButton()
    {
        float RandomRate = 50f;

        if (RandomRate < press)
        {
            press += 10f;
            defenceMove--;
        }
        else
        {
            press -= 10f;
            defenceMove--;
        }
    }
    void GrabButton()
    {
        float RandomRate = 50f;

        if (RandomRate < grab)
        {
            matchState = MatchState.attack;
            defenceMove--;
        }
        else
        {
            Debug.Log("Baþarýsýz Kayma");
            defenceMove--;
        }
    }
    void PassButton()
    {
        float RandomRate = 50f;

        if (RandomRate < pass)
        {
            pass += 10f;
            dribble += 10f;
            shoot += 10f;
            attackMove--;
        }
        else
        {
            matchState = MatchState.defence;
            attackMove--;
        }
    }
    void DribbleButton()
    {
        float RandomRate = 50f;

        if (RandomRate < dribble)
        {
            shoot += 10f;
            attackMove--;
        }
        else
        {
            matchState = MatchState.defence;
            attackMove--;
        }
    }
    void ShootButton()
    {
        float RandomRate = 50f;

        if (RandomRate < shoot)
        {
            Debug.Log("GOOOOLL");
            attackMove--;
        }
        else
        {
            matchState = MatchState.defence;
            attackMove--;
        }
    }




}
