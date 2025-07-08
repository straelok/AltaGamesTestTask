using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static Action StartShooting;
    public static Action StopShooting;

    private void OnEnable()
    {
        GameController.win += OnWin;
        GameController.lose += OnLose;
    }

    private void OnDisable()
    {
        GameController.win -= OnWin;
        GameController.lose -= OnLose;
    }

    private void Update()
    {
        if (GameController.gameOver) return;
        if (Input.GetMouseButtonDown(0))
        {
            StartShooting?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopShooting?.Invoke();
        }
    }

    void OnWin()
    {
        StopShooting?.Invoke();
        this.enabled = false;
    }

    void OnLose()
    {
        StopShooting?.Invoke();
        this.enabled = false;
    }
}