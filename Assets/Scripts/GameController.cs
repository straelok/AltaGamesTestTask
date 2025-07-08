using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static Action win;
    public static Action lose;
    public static bool gameOver = false;

    private void Awake()
    {
        gameOver = false;
    }

    private void OnEnable()
    {
        win += OnWin;
        lose += OnLose;
    }

    private void OnDisable()
    {
        win -= OnWin;
        lose -= OnLose;
    }

    void OnWin()
    {
        gameOver = true;
    }

    void OnLose()
    {
        gameOver = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
