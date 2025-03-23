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
        Pool._GetInstance(_PoolType.tower)._Instantiate(iPrefab, transform.position, Quaternion.identity);
    }
}
