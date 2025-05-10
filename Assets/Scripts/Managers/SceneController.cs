using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] _AllScenes _nextScene;
    [SerializeField] bool _startOnEnable;

    private void OnEnable()
    {
        if (_startOnEnable)
            _StartNextScene();
    }
    public void _StartNextScene()
    {
        SceneManager.LoadScene((int)_nextScene);
    }

    public enum _AllScenes
    {
        StartScene = 0, MainScene = 1
    }
}
