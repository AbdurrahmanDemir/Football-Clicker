using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OfflineEarningsPopup : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI earningsText;
    [SerializeField] private Button claimButton;

    public void Configure(string earningsString)
    {
        earningsText.text = earningsString;
    }

    public Button GetClaimButton()
    {
        return claimButton;
    }
}
