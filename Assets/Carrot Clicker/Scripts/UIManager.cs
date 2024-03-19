using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;


    [Header("Elements")]
    [SerializeField] private Slider powerSlider;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        powerSlider.maxValue = 500;
        UpdateSlider();


    }
    public void UpdateSlider()
    {
        powerSlider.value = PitchManager.instance.GetTotalGen();
    }
}
