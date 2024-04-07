using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Team", menuName = "Scriptable Objects/League/Team", order = 0)]
public class TeamSO : ScriptableObject
{
    public Sprite teamLogo;
    public string teamName;
    public int teamDefGen;
    public int teamMidGen;
    public int teamForGen;

    public int teamPrice;

}
