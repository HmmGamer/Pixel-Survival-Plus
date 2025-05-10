using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform _target;
    [SerializeField] Vector3 _offset;

    private void Update()
    {
        transform.position = _target.position + _offset;
    }
}
