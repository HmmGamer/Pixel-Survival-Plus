using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    public static HomeController instance;
    [SerializeField] int _defaultHp;
    //[SerializeField] int _armor;

    [SerializeField, ReadOnly] int _currentHp;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        _currentHp = _defaultHp;
    }
    public void _TakeDamage(int iDamage)
    {
        //iDamage = AAA.HpTools._CalculateDamage(iDamage, _armor);

        _currentHp -= iDamage;
        if (_currentHp <= 0)
        {
            //GameManager.Instance._GameOver();
            gameObject.SetActive(false);
        }
    }
}
