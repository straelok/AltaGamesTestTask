using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagList.player))
        {
            GetComponent<Animator>().enabled = true;
        }
    }
}
