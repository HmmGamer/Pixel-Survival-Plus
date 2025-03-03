using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region comments
    //const float SHIELD_DURITION = 0.8f;

    //[SerializeField] int _totalArrows;
    //[SerializeField] Transform _frontTransform;
    //[SerializeField] GameObject _shield;

    //[Header("Arrow Settings")]
    //[SerializeField] GameObject _ArrowPrefab;
    //[SerializeField] Transform _arrowSpawnTransform;
    //[SerializeField] float _shootingCooldown = 0.5f;

    //private int _remainingArrows;
    //private bool _canShoot = true;
    //private bool _hasShield;
    #endregion

    public static PlayerController instance;

    [Header("General Settings")]
    [SerializeField] int _defaultHp;
    [SerializeField] public int _defaultDamage;
    [SerializeField] int _defaultArmor;
    [SerializeField] float _recoverHpTime;
    [SerializeField] PlayerCanvasController _canvasController;

    private int _totalHp;
    private int _remainingHp;
    private int _totalArmor;

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
        //_remainingArrows = _totalArrows;
        _canvasController._SetHpText(_remainingHp);
        //_canvasController._SetArrowText(_remainingArrows);
        StartCoroutine(_AddHpOverTimeCoolDown());
    }
    private void OnEnable()
    {
        EquipmentManager._onStatsChange += _UpdateStats;
    }
    private void OnDisable()
    {
        EquipmentManager._onStatsChange -= _UpdateStats;
    }
    private void _UpdateStats()
    {
        _totalHp = EquipmentManager.instance._currentStats._extraHp + _defaultHp;
        
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E) && _canShoot)
        //{
        //    _ShootArrow();
        //    StartCoroutine(_ShotCoolDown());
        //}
    }
    public void _TakeDamage(int iDamage)
    {
        //if (_hasShield) return;

        _remainingHp -= iDamage;

        _canvasController._SetHpText(_remainingHp);

        if (_remainingHp <= 0)
        {
            _Death();
            return;
        }
        //if (iIsGetShield)
        //{
        //    StartCoroutine(_ShieldCooldown());
        //}
    }
    //private IEnumerator _ShotCoolDown()
    //{
    //    _canShoot = false;
    //    yield return new WaitForSeconds(_shootingCooldown);
    //    _canShoot = true;
    //}
    //public void _ShootArrow()
    //{
    //    if (_remainingArrows <= 0) return;

    //    _remainingArrows--;
    //    GameObject temp = Instantiate(_ArrowPrefab, _arrowSpawnTransform.position
    //        , Quaternion.identity);

    //    //bool _isInRightDirection = _frontTransform.position.x > transform.position.x;
    //    //temp.GetComponent<ArrowController>()._SetSpeedDirection(_isInRightDirection);

    //    _canvasController._SetArrowText(_remainingArrows);
    //}
    //private IEnumerator _ShieldCooldown()
    //{
    //    _hasShield = true;
    //    _shield.SetActive(true);
    //    yield return new WaitForSeconds(SHIELD_DURITION);
    //    _shield.SetActive(false);
    //    _hasShield = false;
    //}
    private void _Death()
    {
        gameObject.SetActive(false);
        // respawn somewhere else in the future
    }
    private IEnumerator _AddHpOverTimeCoolDown()
    {
        yield return new WaitForSeconds(_recoverHpTime);
        _AddHpOverTime();
    }
    private void _AddHpOverTime()
    {
        if (_remainingHp < _totalHp)
        {
            _remainingHp++;
        }
    }
    public void _ConsumeHpPotion(int iHpValue)
    {
        _remainingHp += iHpValue;
        if (_remainingHp > _totalHp)
        {
            _remainingHp = _totalHp;
        }
    }
}
