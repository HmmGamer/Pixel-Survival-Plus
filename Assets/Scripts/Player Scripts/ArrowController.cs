using System.Collections;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] int _damage = 1;
    [SerializeField] float _lifeTime = 3;
    [SerializeField] float _speed = 22f;

    [Header("Optional Settings")]
    [SerializeField] bool _showGizmos = false;
    [SerializeField] float _rayRange = 3f;

    private GameObject _immunePlayer;

    private void Start()
    {
        _CheckForProblems();
    }
    private void Update()
    {
        _Move();
        _CheckCollision();
        StartCoroutine(_DestroyCoolDown());
    }
    public void _SetSpeedDirection(bool iIsRightDirection)
    {
        if (!iIsRightDirection)
            _speed *= -1;
    }
    public void _SetImmunePlayer(GameObject player)
    {
        _immunePlayer = player;
    }
    private void _CheckCollision()
    {
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, transform.right, _rayRange);
        if (_hit.collider != null)
        {
            if (_hit.collider.gameObject == _immunePlayer) return;

            if (_hit.collider.CompareTag(A.Tags.player))
            {
                //_hit.collider.GetComponent<PlayerController>()._TakeDamage(_damage, false);
            }

            Destroy(gameObject);
        }
    }
    private void _Move()
    {
        Vector2 _newPosition = (Vector2)transform.position + (Vector2)transform.right
            * _speed * Time.deltaTime;

        transform.position = _newPosition;
    }
    IEnumerator _DestroyCoolDown()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }

    #region Editor Only
    private void _CheckForProblems()
    {
#if UNITY_EDITOR
        if (gameObject.tag != A.Tags.arrow)
            Debug.Log("The Arrow prefab needs Arrow tag");
#endif
    }
    private void OnDrawGizmos()
    {
        if (!_showGizmos) return;
        Gizmos.color = Color.red;
        Vector3 _direction = Vector3.right * _rayRange;
        Gizmos.DrawRay(transform.localPosition, _direction);
    }
    #endregion
}
public class __Off_FutureDevelopments_Comments2
{
    /* it is recommended to add a pooling system as soon as possible to improve performance
     */
}
