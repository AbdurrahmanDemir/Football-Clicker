using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnnouncerPrefabs : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI announcerText;

    public void Config(string Text)
    {
        announcerText.text = Text;
    }
}
