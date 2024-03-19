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
    public int myTeamDefGen;
    public int myTeamMidGen;
    public int myTeamForGen;
    public int myTeamTotalGen;

    [Header("OPPONENT TEAM ELEMENTS")]
    public int opponentDefGen;
    public int opponentMidGen;
    public int opponentForGen;
    public int opponentTotalGen;

    [Header("ATTACK STATS")]
    float pass;
    float dribble;
    float shoot;
    public static Action onAttack;

    [Header("DEFENCE STATS")]
    float press;
    float grab;
    public static Action onDefence;

    private void Start()
    {
        matchState = MatchState.attack;
        CalculateRate();
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
        press = (myTeamTotalGen / (myTeamTotalGen + opponentTotalGen)) * 100f;
        grab = (myTeamForGen / (myTeamForGen + opponentDefGen)) * 100f;
    }

    void PressButton()
    {

    }



}
