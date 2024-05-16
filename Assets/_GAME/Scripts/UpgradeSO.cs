using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Data", menuName = "Scriptable Objects/Upgrade Data", order = 0)]
public class UpgradeSO : ScriptableObject
{
    [Header(" General ")]
    public Sprite bodyImage;
    public Sprite kitImage;
    public Sprite faceImage;
    public Sprite hairImage;
    public string title;
    public int gen;
    public PlayerPos pos;
    [Header(" Settings ")]
    public double cpsPerLevel;
    public double basePrice;
    public float coefficient;

    public double GetPrice(int level)
    {
        return basePrice * Mathf.Pow(coefficient, level);
    }
}
public enum PlayerPos
{
    GK,
    LCB,
    RCB,
    LB,
    RB,
    LM,
    RM,
    CM,
    LW,
    RW,
    ST,
}
