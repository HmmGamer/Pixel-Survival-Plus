using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float SHIELD_DURITION = 0.8f;
    public static PlayerController instance;

    [Header("General Settings")]
    [SerializeField] int _totalHp;
    [SerializeField] int _totalArrows;
    //[SerializeField] Transform _frontTransform;
    [SerializeField] GameObject _shield;
    [SerializeField] PlayerCanvasController _canvasController;

    [Header("Arrow Settings")]
    [SerializeField] GameObject _ArrowPrefab;
    [SerializeField] Transform _arrowSpawnTransform;
    [SerializeField] float _shootingCooldown = 0.5f;

    private int _remainingHp;
    private int _remainingArrows;
    private bool _canShoot = true;
    private bool _hasShield;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {
        _remainingHp = _totalHp;
        _remainingArrows = _totalArrows;
        _canvasController._SetHpText(_remainingHp);
        //_canvasController._SetArrowText(_remainingArrows);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _canShoot)
        {
            //_ShootArrow();
            //StartCoroutine(_ShotCoolDown());
        }
    }
    private IEnumerator _ShotCoolDown()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_shootingCooldown);
        _canShoot = true;
    }
    public void _ShootArrow()
    {
        if (_remainingArrows <= 0) return;

        _remainingArrows--;
        GameObject temp = Instantiate(_ArrowPrefab, _arrowSpawnTransform.position
            , Quaternion.identity);

        //bool _isInRightDirection = _frontTransform.position.x > transform.position.x;
        //temp.GetComponent<ArrowController>()._SetSpeedDirection(_isInRightDirection);

        _canvasController._SetArrowText(_remainingArrows);
    }
    public void _TakeDamage(int iDamage,bool iIsGetShield)
    {
        if (_hasShield) return;

        _remainingHp -= iDamage;

        _canvasController._SetHpText(_remainingHp);

        if (_remainingHp <= 0)
        {
            _Death();
            return;
        }
        if (iIsGetShield)
        {
            StartCoroutine(_ShieldCooldown());
        }
    }
    private IEnumerator _ShieldCooldown()
    {
        _hasShield = true;
        _shield.SetActive(true);
        yield return new WaitForSeconds(SHIELD_DURITION);
        _shield.SetActive(false);
        _hasShield = false;
    }
    private void _Death()
    {
        gameObject.SetActive(false);
        // respawn somewhere else in the future
    }
}
