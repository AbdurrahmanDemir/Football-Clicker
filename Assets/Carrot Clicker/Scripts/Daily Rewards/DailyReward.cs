using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DailyReward
{
    public DailyRewardType rewardType;
    public double amount;
    public Sprite icon;
    public int upgradeIndex;
}