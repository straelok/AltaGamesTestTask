using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] Transform _target;

    Vector3 _offset;

    private void Start()
    {
        if (_target == null)
        {
            Debug.LogError("CameraMover: Target not assigned.");
            enabled = false;
            return;
        }

        _offset = transform.position - _target.position;
    }

    private void LateUpdate()
    {
        transform.position = _target.position + _offset;
    }
}
