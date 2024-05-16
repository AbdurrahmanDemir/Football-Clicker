using Assets.SimpleLocalization.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLanguage : MonoBehaviour
{
    private void Awake()
    {
        LocalizationManager.Read();
        LocalizationManager.Language = "Türkçe";

        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                LocalizationManager.Language = "English";
                break;
            case SystemLanguage.Turkish:
                LocalizationManager.Language = "Türkçe";
                break;
            default:
                LocalizationManager.Language = "English";
                break;


        }
    }
    public void Language(string lan)
    {
        LocalizationManager.Language = lan;
    }
}
