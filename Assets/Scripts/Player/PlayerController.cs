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
    //
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
    //private void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.E) && _canShoot)
    //    //{
    //    //    _ShootArrow();
    //    //    StartCoroutine(_ShotCoolDown());
    //    //}
    //}
    #endregion

    public static PlayerController instance;

    [Header("General Settings")]
    [SerializeField] float _recoverHpTime;
    [SerializeField] PlayerCanvasController _canvasController;

    int _totalHp;
    int _remainingHp;
    int _totalArmor;

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
        EquipmentManager._onNewStats += _UpdateStats;
    }
    private void OnDisable()
    {
        EquipmentManager._onNewStats -= _UpdateStats;
    }
    public void _UpdateStats()
    {
        _totalHp = EquipmentManager.Instance._totalStats._hp;
        _totalArmor = EquipmentManager.Instance._totalStats._armor;
    }
    public void _TakeDamage(int iDamage)
    {
        //if (_hasShield) return;

        iDamage = AAA.HpTools._CalculateDamage(iDamage, _totalArmor);
        _remainingHp -= iDamage;

        _UpdateUi();
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
    private void _UpdateUi()
    {
        _canvasController._SetHpText(_remainingHp);
    }
    private void _Death()
    {
        gameObject.SetActive(false);
        GameManager.Instance._GameOver();
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
        _UpdateUi();
    }
}
