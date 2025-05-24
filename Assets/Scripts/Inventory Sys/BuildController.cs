using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    public static BuildController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void _BuildObject(GameObject iPrefab)
    {
        PoolManager._instance._Instantiate(_PoolType.tower, iPrefab, transform.position, Quaternion.identity);
    }
}
