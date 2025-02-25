using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] float _walkSpeed = 5f;
    [SerializeField] float _runSpeed = 8f;
    [SerializeField] float _jumpForce = 10f;
    [SerializeField] int _inAirJumps = 1;
    //[SerializeField] float _slideOnWallGravity = 1;

    [Header("Attachments")]
    [SerializeField] Transform _feetTransform;
    //[SerializeField] Transform _frontTransform;
    //[SerializeField] Transform _backTransform;
    [SerializeField] PlayerCanvasController _canvasController;


    [Header("Others")]
    [SerializeField] float _jumpDelayTime = 0.5f;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] _Wall_Ground_Detection _DetectionSettings;

    [Header("Animation Settings")]
    [SerializeField] Animator _playerAnimator;

    private Rigidbody2D _rb;
    private bool _canWalk = true;
    private bool _isGrounded;
    //private bool _isOnFrontWall;
    //private bool _isOnBackWall;
    private bool _isRunning;
    private bool _isInAir;
    private bool _isNowJumping;
    private int _jumpsRemaining;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _jumpsRemaining = _inAirJumps;
    }
    private void OnDrawGizmos()
    {
        if (!_DetectionSettings._isShowGizmos) return;
        if (!(_feetTransform/* && _backTransform && _frontTransform*/))
        {
            Debug.LogError("feet field can't be null");
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawCube(_feetTransform.position, new Vector2(_DetectionSettings._GroundWidth, _DetectionSettings._GroundHight));
        //Gizmos.DrawCube(_backTransform.position, new Vector2(_DetectionSettings._WallWidth, _DetectionSettings._WallHight));
        //Gizmos.DrawCube(_frontTransform.position, new Vector2(_DetectionSettings._WallWidth, _DetectionSettings._WallHight));
    }
    void Update()
    {
        _HandleMovement();
        _HandleJump();
    }
    private void _HandleMovement()
    {
        if (!_canWalk) return;

        float _horizontalInput = Input.GetAxisRaw("Horizontal");
        _isRunning = Input.GetKey(KeyCode.LeftShift);
        float _currentSpeed = _isRunning ? _runSpeed : _walkSpeed;

        //if (_isOnFrontWall && (_horizontalInput > 0 || _horizontalInput < 0))
        //{
        //    _rb.velocity = new Vector2(_horizontalInput * (_currentSpeed * 0.1f)
        //        , Mathf.Max(_rb.velocity.y, -_slideOnWallGravity));
        //}
        //else
        //{
        _rb.velocity = new Vector2(_horizontalInput * _currentSpeed, _rb.velocity.y);
        //}

        _playerAnimator.SetFloat(A.Anim.playerSpeed, Math.Abs(_currentSpeed * _horizontalInput));
    }
    private void _HandleJump()
    {
        _isGrounded = Physics2D.OverlapBox(_feetTransform.position, new Vector2
            (_DetectionSettings._GroundWidth, _DetectionSettings._GroundHight), 0, _groundLayer);

        //_isOnBackWall = Physics2D.OverlapBox(_backTransform.position, new Vector2
        //    (_DetectionSettings._WallWidth, _DetectionSettings._WallHight), 0, _groundLayer);
        //_isOnFrontWall = Physics2D.OverlapBox(_frontTransform.position, new Vector2
        //    (_DetectionSettings._WallWidth, _DetectionSettings._WallHight), 0, _groundLayer);

        if (_isGrounded)
        {
            _jumpsRemaining = _inAirJumps;
            _isInAir = false;
            _canvasController._SetJumpImage(_jumpsRemaining);
        }
        else
            _isInAir = true;


        if (Input.GetKeyDown(KeyCode.Space) && ((_jumpsRemaining > 0)/* || _isOnFrontWall || _isOnBackWall*/))
        {
            if (_isInAir && !_isNowJumping)
            {
                _Jump();
            }
            else if (!_isInAir && !_isNowJumping)
            {
                StartCoroutine(_JumpWithDelay());
            }
        }
        _playerAnimator.SetBool(A.Anim.playerIsGrounded, _isGrounded);
        //_playerAnimator.SetBool(A.Anim.playerIsOnWall, _isOnFrontWall);
    }
    private void _Jump()
    {
        if (!_isGrounded)
            _jumpsRemaining--;

        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(new Vector2(0, _jumpForce * 100));

        _canvasController._SetJumpImage(_jumpsRemaining);
    }
    private IEnumerator _JumpWithDelay()
    {
        _playerAnimator.SetTrigger(A.Anim.playerIsJumping);
        _canWalk = false;
        _isNowJumping = true;
        yield return new WaitForSeconds(_jumpDelayTime);
        _canWalk = true;
        _isInAir = true;
        _isNowJumping = false;
        _Jump();
    }
    [Serializable]
    class _Wall_Ground_Detection
    {
        public bool _isShowGizmos = false;

        public float _GroundHight = 0.1f;
        public float _GroundWidth = 0.5f;

        //public float _WallHight = 0.65f;
        //public float _WallWidth = 0.1f;
    }
}