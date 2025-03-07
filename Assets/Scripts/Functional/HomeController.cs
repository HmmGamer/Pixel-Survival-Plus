using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    public static HomeController instance;

    [SerializeField] int _defaultHp;
    [SerializeField] Text _hpText;
    //[SerializeField] int _armor;

    int _currentHp;

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
        _UpdateHpText();
    }
    public void _TakeDamage(int iDamage)
    {
        //iDamage = AAA.HpTools._CalculateDamage(iDamage, _armor);

        _currentHp -= iDamage;
        _UpdateHpText();
        if (_currentHp <= 0)
        {
            //GameManager.Instance._GameOver();
            gameObject.SetActive(false);
        }
    }
    private void _UpdateHpText()
    {
        _hpText.text = _currentHp.ToString();
    }
}
