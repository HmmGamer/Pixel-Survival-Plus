using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("General Settings")]
    [SerializeField] WaveDataBase _waveData;
    [SerializeField] int _defaultLevel;
    [SerializeField] bool _autoStart = false;

    [Header("Attachments")]
    [SerializeField] Transform _spawnPos;
    [SerializeField] Text _remainingEnemiesText;
    [SerializeField] Button _startNextWaveButton;

    private int _currentWaveIndex = 0;
    private bool _isSpawning = false;
    Pool _pool;

    #region Starter
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _pool = Pool._GetInstance(_PoolType.enemy);
    }
    private void Start()
    {
        _InitButtons();
        _currentWaveIndex = _defaultLevel;
        if (_autoStart)
            _StartNextWave();
    }
    private void _InitButtons()
    {
        _startNextWaveButton.onClick.AddListener(_StartNextWave);
    }
    #endregion
    #region Spawn System
    private IEnumerator _StartWave()
    {
        _isSpawning = true;

        WaveDataBase._AllWavesStruct currentWave = _waveData._allWaves[_currentWaveIndex];

        // Create a local copy of enemy counts
        int[] enemyCounts = new int[currentWave._waves.Length];
        for (int i = 0; i < currentWave._waves.Length; i++)
        {
            enemyCounts[i] = currentWave._waves[i]._count;
        }

        if (currentWave._finishEachEnemyFirst)
        {
            for (int i = 0; i < currentWave._waves.Length; i++)
            {
                yield return SpawnEnemies(currentWave._waves[i]);
            }
        }
        else
        {
            while (HasRemainingEnemies(enemyCounts))
            {
                for (int i = 0; i < currentWave._waves.Length; i++)
                {
                    if (enemyCounts[i] > 0)
                    {
                        yield return SpawnEnemy(currentWave._waves[i]._enemyPrefab);
                        enemyCounts[i]--;
                        yield return new WaitForSeconds(currentWave._spawnDelay);
                    }
                }
            }
        }
        _isSpawning = false;
        _ActivateNextWaveButton(true); // Enable the button when the wave is finished
    }
    private bool HasRemainingEnemies(int[] enemyCounts)
    {
        foreach (int count in enemyCounts)
        {
            if (count > 0) return true;
        }
        return false;
    }
    private IEnumerator SpawnEnemies(WaveDataBase._EachWaveStruct wave)
    {
        for (int i = 0; i < wave._count; i++)
        {
            yield return SpawnEnemy(wave._enemyPrefab);
            yield return new WaitForSeconds(_waveData._allWaves[_currentWaveIndex]._spawnDelay);
        }
    }
    private IEnumerator SpawnEnemy(GameObject enemyPrefab)
    {
        _pool._Instantiate(enemyPrefab, _spawnPos.position,Quaternion.identity);
        yield return null;
    }
    private bool HasRemainingEnemies(WaveDataBase._AllWavesStruct wave)
    {
        for (int i = 0; i < wave._waves.Length; i++)
        {
            if (wave._waves[i]._count > 0)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
    #region Wave Managment
    public void _ResetSpawner()
    {
        StopAllCoroutines();
        _currentWaveIndex = _defaultLevel;
        _isSpawning = false;
        _ActivateNextWaveButton(true); // Enable the button when resetting
    }
    public void _StartNextWave()
    {
        if (_isSpawning) return;
        _ActivateNextWaveButton(false); // Disable the button when starting a new wave
        StartCoroutine(_StartWave());
    }
    #endregion
    #region Ui Management
    private void _UpdateUi(int _currentEnemies)
    {
        _remainingEnemiesText.text = _currentEnemies.ToString();
    }
    private void _ActivateNextWaveButton(bool iActivation)
    {
        _startNextWaveButton.gameObject.SetActive(iActivation);
    }
    #endregion
}