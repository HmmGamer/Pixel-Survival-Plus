using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField] ItemData _currentShopData;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void _BuyItem()
    {

    }
    public void _SetDataOnSelect(ItemData iData)
    {
        _currentShopData = iData;
    }
}
