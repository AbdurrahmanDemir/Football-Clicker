using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    [Header(" Actions ")]
    public static Action onPitchClicked;
    public static Action<Vector2> onPitchClickedPosition;

    int totalTeamGen;
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
            ManageTouches();

        
        if (Input.GetMouseButtonDown(0))
            ThrowRaycast(Input.mousePosition);
    }

    private void ManageTouches()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
                ThrowRaycast(touch.position);
        }
    }

    private void ThrowRaycast(Vector2 touchPosition)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(touchPosition));

        if (hit.collider == null)
            return;

        Debug.Log("We hit a goal ! ");


        onPitchClicked?.Invoke();

        onPitchClickedPosition?.Invoke(hit.point);
    }
}
