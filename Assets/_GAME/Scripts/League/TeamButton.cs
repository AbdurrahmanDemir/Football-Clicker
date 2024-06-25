using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamButton : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Image clubLogo;
    [SerializeField] private TextMeshProUGUI clubNameText;
    [SerializeField] private TextMeshProUGUI clubGenText;
    [SerializeField] private TextMeshProUGUI clubPriceText;
    [SerializeField] private TextMeshProUGUI clubWinningPrizeText;
    [SerializeField] private Button playButton;

    
    public void Config(Sprite clubL, string clubName, string clubGen,string clubPrice,string winPrize)
    {
        clubLogo.sprite = clubL;
        clubNameText.text = clubName;
        clubGenText.text = clubGen;
        clubPriceText.text = clubPrice;
        clubWinningPrizeText.text = winPrize;

    }

    public Button GetPlayButton()
    {
        return playButton;
    }
}
