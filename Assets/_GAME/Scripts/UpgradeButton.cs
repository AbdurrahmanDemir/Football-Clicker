using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI genText;
    [SerializeField] private TextMeshProUGUI posText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button elevenButton;
    [SerializeField] private Button buyButton;

    public void Configure(Sprite icon, string title, string subtitle, string price,string gen,string pos)
    {
        iconImage.sprite = icon;
        titleText.text = title;
        genText.text = gen;
        posText.text = pos;

        UpdateVisuals(subtitle, price,gen);
    }

    public void UpdateVisuals(string subtitle, string price,string gen)
    {
        subtitleText.text = subtitle;
        priceText.text = price;
        genText.text = gen;
    }

    public Button GetUpgradeButton()
    {
        return upgradeButton;
    }
    public Button GetElevenButton()
    {
        return elevenButton;
    }
    public Button GetBuyButton()
    {
        return buyButton;
    }

    public TextMeshProUGUI GetGenText()
    {
        return genText;
    }
}
