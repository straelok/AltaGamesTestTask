using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
    [SerializeField] float _lifeTime = 5f;
    [SerializeField] float _explodeRangeCoef = 1f;
    [SerializeField] GameObject _fxPrefab;

    Vector3 _direction;
    bool _isLaunched = false;
    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!_isLaunched) return;
        _rb.MovePosition(_rb.position + (_direction * _speed));
    }

    public void Launch(Vector3 direction)
    {
        _direction = direction;
        _isLaunched = true;
        StartCoroutine(CLifeTime());
    }

    IEnumerator CLifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagList.obstacle))
        {
            Explode(other);
        }
    }

    public void Explode(Collider other)
    {
        float radius = transform.localScale.x * _explodeRangeCoef;
        //debug zone
        /*

        GameObject debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        debugSphere.transform.position = transform.position;
        debugSphere.transform.localScale = Vector3.one * radius * 2f;
        debugSphere.GetComponent<Collider>().enabled = false;

        Destroy(debugSphere, 1f);
        */

        if (_fxPrefab != null)
        {
            Vector3 fxPosition = transform.position + Vector3.up * 0.25f;
            GameObject fx = Instantiate(_fxPrefab, fxPosition, Quaternion.identity);

            fx.transform.localScale = Vector3.one * radius * 2f;

            Destroy(fx, 2f);
        }

        Collider[] infestRadiusCollider = Physics.OverlapSphere(gameObject.transform.position, radius);
        foreach (Collider obj in infestRadiusCollider)
        {
            if (obj.CompareTag(TagList.obstacle))
            {
                Destroy(obj.gameObject);
                Destroy(gameObject);
            }
        }
    }
}