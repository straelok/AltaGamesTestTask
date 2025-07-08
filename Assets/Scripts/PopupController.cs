using UnityEngine;

public class PopupController : MonoBehaviour
{
    [SerializeField] GameObject _winPopup;
    [SerializeField] GameObject _losePopup;

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

    void OnWin()
    {
        _winPopup.SetActive(true);
    }

    void OnLose()
    {
        _losePopup.SetActive(true);
    }
}
