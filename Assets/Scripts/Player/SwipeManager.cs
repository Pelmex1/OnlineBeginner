using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using JetBrains.Annotations;

public class SwipeManager : MonoBehaviour
{
    [SerializeField] private float deadZone = 10f;
    private Vector2 tapPosition;
    private Vector2 swipeDelta;
    private bool isSwiping;
    private readonly bool isMobile = Application.isMobilePlatform;
    private EventBus eventBus;
    private void Start()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();
    }
    private void Update()
    {
        if (!isMobile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSwiping = true;
                tapPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
                ResetSwipe();
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isSwiping = true;
                    tapPosition = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    ResetSwipe();
                }
            }
        }
        CheckSwipe();
    }

    private void CheckSwipe()
    {
        swipeDelta = Vector2.zero;

        if (isSwiping)
        {
            if (!isMobile && Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - tapPosition;
            else if (Input.touchCount > 0)
                swipeDelta = Input.GetTouch(0).position - tapPosition;
        }
        if (swipeDelta.magnitude > deadZone)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                Vector2 vector;
                if (swipeDelta.x > 0)
                    vector = Vector2.right;
                else
                    vector = Vector2.left;
                eventBus?.Invoke(new Signals(vector));
            }
            ResetSwipe();
        }
    }

    private void ResetSwipe()
    {
        isSwiping = false;
        tapPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
    }
}
