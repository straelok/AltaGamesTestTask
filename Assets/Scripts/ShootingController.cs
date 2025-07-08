using UnityEngine;

[RequireComponent (typeof(PlayerCharacter))]
public class ShootingController : MonoBehaviour
{
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] Transform _spawnPosition;
    [SerializeField] float _chargeSpeed = 0.5f;

    bool _charging = false;
    GameObject _chargableProjectile;
    PlayerCharacter _character;

    private void Awake()
    {
        _character = GetComponent<PlayerCharacter>();
    }

    private void OnEnable()
    {
        InputManager.StartShooting += CreateProjectile;
        InputManager.StopShooting += LaunchProjectile;
    }

    private void OnDisable()
    {
        InputManager.StartShooting -= CreateProjectile;
        InputManager.StopShooting -= LaunchProjectile;
    }

    private void FixedUpdate()
    {
        if (!_charging) return;

        ProjectilePowering();
    }

    void CreateProjectile()
    {
        _chargableProjectile = Instantiate(_projectilePrefab, _spawnPosition.position, Quaternion.identity);
        _charging = true;
    }

    void ProjectilePowering()
    {
        _chargableProjectile.transform.position = _spawnPosition.position; // if player start move in charging process
        Vector3 scalingVector = new Vector3(_chargeSpeed, _chargeSpeed, _chargeSpeed);
        _chargableProjectile.transform.localScale += scalingVector;
        _character.RemoveScale(_chargeSpeed);
    }

    void LaunchProjectile()
    {
        if (!_charging) return;
        _charging = false;
        _chargableProjectile.GetComponent<Projectile>().Launch(transform.forward);
        _chargableProjectile = null;
    }
}
