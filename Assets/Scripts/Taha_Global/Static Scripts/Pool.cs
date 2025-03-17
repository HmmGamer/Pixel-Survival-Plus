using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoBehaviour
{
    private static Dictionary<_PoolType, Pool> _instances = new Dictionary<_PoolType, Pool>();

    private Dictionary<GameObject, Stack<GameObject>> _pools = new Dictionary<GameObject, Stack<GameObject>>();
    [SerializeField] private int _preloadAmount = 5;
    [SerializeField] private _PoolType _poolType;

    private void Awake()
    {
        if (!_instances.ContainsKey(_poolType))
        {
            _instances[_poolType] = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("You have more than one pool with the same type: " + _poolType);
        }
    }
    public static Pool _GetInstance(_PoolType _poolType)
    {
        return _instances.TryGetValue(_poolType, out Pool _instance) ? _instance : null;
    }
    public GameObject _Instantiate(GameObject _iGameObject)
    {
        return _Instantiate(_iGameObject, Vector3.zero, Quaternion.identity);
    }
    public GameObject _Instantiate(GameObject _iGameObject, Vector3 _position, Quaternion _rotation)
    {
        if (!_pools.TryGetValue(_iGameObject, out Stack<GameObject> _stack))
        {
            _stack = new Stack<GameObject>();
            _pools[_iGameObject] = _stack;
            _Preload(_iGameObject, _stack);
        }
        if (_stack.Count > 0)
        {
            GameObject _pooledObject = _stack.Pop();
            _pooledObject.transform.SetPositionAndRotation(_position, _rotation);
            _pooledObject.SetActive(true);
            return _pooledObject;
        }
        GameObject _newObject = Instantiate(_iGameObject, _position, _rotation);
        _newObject.AddComponent<_PooledObject>()._prefab = _iGameObject;
        return _newObject;
    }
    public void _Despawn(GameObject _iGameObject)
    {
        _iGameObject.SetActive(false);
        _PooledObject _pooledComponent = _iGameObject.GetComponent<_PooledObject>();
        if (_pooledComponent != null && _pools.TryGetValue(_pooledComponent._prefab, out Stack<GameObject> _stack))
        {
            _stack.Push(_iGameObject);
        }
    }
    private void _Preload(GameObject _iGameObject, Stack<GameObject> _stack)
    {
        for (int i = 0; i < _preloadAmount; i++)
        {
            GameObject _newObject = Instantiate(_iGameObject);
            _newObject.SetActive(false);
            _newObject.AddComponent<_PooledObject>()._prefab = _iGameObject;
            _stack.Push(_newObject);
        }
    }
}
public class _PooledObject : MonoBehaviour
{
    public GameObject _prefab;
}
public enum _PoolType
{
    enemy, tower, item , bullet
}