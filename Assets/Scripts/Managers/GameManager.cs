using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameObject _GameOverCanvas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void _GameOver()
    {
        //EnemySpawner.Instance._ResetSpawner();
        _GameOverCanvas.gameObject.SetActive(true);
    }
}
