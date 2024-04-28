using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialPanels;

    private void Start()
    {
        for (int i = 0; i < tutorialPanels.Length; i++)
        {
            tutorialPanels[i].SetActive(false);
        }

        if (!PlayerPrefs.HasKey("FirstLogin"))
        {
            tutorialPanels[0].SetActive(true);
        }
    }
    public void TutorialPanel1Close()
    {
        PlayerPrefs.SetInt("FirstLogin", 1);
        tutorialPanels[0].SetActive(false);
    }
    public void TutorialPanel2Open()
    {
        tutorialPanels[1].SetActive(true);
    }
    public void TutorialPanel2Close()
    {
        PlayerPrefs.SetInt("TutorialPanel2", 1);
        tutorialPanels[1].SetActive(false);
    }

    public void TutorialPanel3Open()
    {
        tutorialPanels[2].SetActive(true);
    }
    public void TutorialPanel3Close()
    {
        PlayerPrefs.SetInt("TutorialPanel3", 1);
        tutorialPanels[2].SetActive(false);
    }


}
