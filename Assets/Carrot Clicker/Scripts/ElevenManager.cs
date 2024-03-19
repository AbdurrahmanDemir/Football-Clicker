using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevenManager : MonoBehaviour
{
    public static ElevenManager instance;

    [Header("Settings")]
    [SerializeField] private Transform[] elevenPoints;

    [Header(" Data ")]
    [SerializeField] private UpgradeSO[] upgrades;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddEleven( int elevenIndex)
    {
        elevenPoints[1].GetComponent<SpriteRenderer>().sprite = upgrades[elevenIndex].icon;
    }

}
