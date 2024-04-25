using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineEarningsUI : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private OfflineEarningsPopup popup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayPopup(double earnings)
    {
        popup.Configure(DoubleUtilities.ToIdleNotation(earnings));

        popup.GetClaimButton().onClick.AddListener(() => ClaimButtonClickedCallback(earnings));

        //popup.gameObject.SetActive(true);

    }

    private void ClaimButtonClickedCallback(double earnings)
    {
        Debug.Log("Give the player " + earnings + " carrots");

        popup.gameObject.SetActive(false);

        PitchManager.instance.AddCarrots(earnings);

    }
}
