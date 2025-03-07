using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] WaveDataBase _waveData;
    [SerializeField] Transform _spawnPos;
    [SerializeField] int _defaultLevel;

    [SerializeField] Text _remainingEnemiesText;
    [SerializeField] Button _startNextWaveButton;

    private int _currentWaveIndex = 0;
    private bool _isSpawning = false;

    #region Starter
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        _InitButtons();
        _currentWaveIndex = _defaultLevel;
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

        if (currentWave._finishEachWaveFirst)
        {
            for (int i = 0; i < currentWave._waves.Length; i++)
            {
                yield return SpawnEnemies(currentWave._waves[i]);
            }
        }
        else
        {
            while (HasRemainingEnemies(currentWave))
            {
                for (int i = 0; i < currentWave._waves.Length; i++)
                {
                    if (currentWave._waves[i]._count > 0)
                    {
                        yield return SpawnEnemy(currentWave._waves[i]._enemyPrefab);
                        currentWave._waves[i]._count--;
                        yield return new WaitForSeconds(currentWave._spawnDelay);
                    }
                }
            }
        }

        _isSpawning = false;
        _ActivateNextWaveButton(true); // Enable the button when the wave is finished
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
        Instantiate(enemyPrefab, _spawnPos.position, Quaternion.identity);
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