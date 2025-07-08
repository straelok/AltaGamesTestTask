using System.Collections;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] float _minimalScaleToLose = 0.2f;
    [SerializeField] float _scaleLosingCoef = 0.5f;
    [SerializeField] PlayerPath _playerPath;
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _checkDistance = 3f;
    [SerializeField] Transform _modelTransform;
    [SerializeField] float _jumpAmplitude = 0.2f;
    [SerializeField] float _jumpFrequency = 4f;
    [SerializeField] float _jumpFadeDuration = 0.3f;
    [SerializeField] Renderer _modelRenderer;

    Vector3 _modelBaseLocalPosition;
    Coroutine _jumpRoutine;
    bool _isMoving = false;

    private void Awake()
    {
        _modelBaseLocalPosition = _modelTransform.localPosition;
        UpdateColorByScale();

        if (_playerPath == null)
        {
            Debug.LogError("Set Player Path in PlayerCharacter");
        }
    }

    public void RemoveScale(float value)
    {
        Vector3 scalingVector = new Vector3(value, value, value);
        gameObject.transform.localScale -= scalingVector * _scaleLosingCoef;
        _playerPath.SetXScale(transform.localScale.x);
        if (transform.localScale.x < _minimalScaleToLose)
        {
            GameController.lose?.Invoke();
        }
        UpdateColorByScale();
    }

    private void Update()
    {
        MoveCharacter();
    }

    void MoveCharacter()
    {
        if (GameController.gameOver)
        {
            if (_jumpRoutine != null)
            {
                StopCoroutine(_jumpRoutine);
                _jumpRoutine = null;
            }
            return;
        }

        Vector3 originalHalfExtents = transform.localScale * 0.5f;
        Vector3 reducedHalfExtents = originalHalfExtents * 0.6f;
        Vector3 direction = transform.forward;
        Vector3 origin = transform.position + Vector3.up * originalHalfExtents.y;

        bool isBlocked = false;

        if (Physics.BoxCast(origin, reducedHalfExtents, direction, out RaycastHit hit, transform.rotation, _checkDistance))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                isBlocked = true;
            }
        }

        if (isBlocked)
        {
            if (_isMoving)
            {
                _isMoving = false;
                StopJumping();
            }
            return;
        }

        transform.position += direction * _moveSpeed * Time.deltaTime;

        if (!_isMoving)
        {
            _isMoving = true;
            StartJumping();
        }
    }

    void UpdateColorByScale()
    {
        if (_modelRenderer == null) return;

        float current = transform.localScale.x;
        float t = Mathf.InverseLerp(_minimalScaleToLose, 1f, current);
        Color color = Color.Lerp(Color.red, Color.green, t);
        _modelRenderer.material.color = color;
    }


    void StartJumping()
    {
        if (_jumpRoutine != null)
            StopCoroutine(_jumpRoutine);
        _jumpRoutine = StartCoroutine(JumpRoutine());
    }

    void StopJumping()
    {
        if (_jumpRoutine != null)
            StopCoroutine(_jumpRoutine);
        _jumpRoutine = StartCoroutine(JumpFadeOutRoutine());
    }

    IEnumerator JumpRoutine()
    {
        float time = 0f;
        while (true)
        {
            time += Time.deltaTime * _jumpFrequency;
            float offset = Mathf.Abs(Mathf.Sin(time)) * _jumpAmplitude;
            _modelTransform.localPosition = _modelBaseLocalPosition + Vector3.up * offset;
            yield return null;
        }
    }

    IEnumerator JumpFadeOutRoutine()
    {
        float elapsed = 0f;
        float startOffset = _modelTransform.localPosition.y - _modelBaseLocalPosition.y;
        while (elapsed < _jumpFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = 1f - (elapsed / _jumpFadeDuration);
            float offset = startOffset * t;
            _modelTransform.localPosition = _modelBaseLocalPosition + Vector3.up * offset;
            yield return null;
        }

        _modelTransform.localPosition = _modelBaseLocalPosition;
    }


    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;

        Vector3 originalHalfExtents = transform.localScale * 0.5f;
        Vector3 reducedHalfExtents = originalHalfExtents * 0.6f;
        Vector3 direction = transform.forward;
        Vector3 origin = transform.position + Vector3.up * originalHalfExtents.y;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(origin, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(direction * (_checkDistance * 0.5f), new Vector3(reducedHalfExtents.x * 2, reducedHalfExtents.y * 2, _checkDistance));
#endif

    }
}