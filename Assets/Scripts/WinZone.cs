using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagList.player))
        {
            GameController.win?.Invoke();
        }
    }
}
