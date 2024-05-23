using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Pitch : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Transform pitchRenderer;
    [SerializeField] private Image fillImage;

    public static Action onPitchClickedQuest;

    [Header(" Settings ")]
    [SerializeField] private float fillRate;
    private bool isFrenzyModeActive;

    private void Awake()
    {
        InputManager.onPitchClicked += CarrotClickedCallback;
    }

    private void OnDestroy()
    {
        InputManager.onPitchClicked -= CarrotClickedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void CarrotClickedCallback()
    {
        //// Animate the Carrot Renderer
        Animate();
        onPitchClickedQuest?.Invoke();

    }

    private void Animate()
    {
        pitchRenderer.localScale = Vector3.one * 1.3f;
        LeanTween.cancel(pitchRenderer.gameObject);
        LeanTween.scale(pitchRenderer.gameObject, Vector3.one * 1.1f, .15f).setLoopPingPong(1);
    }


}
