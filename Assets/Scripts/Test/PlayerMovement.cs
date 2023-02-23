using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    //[SerializeField] private float _wallRideSpeed;
    [SerializeField] private float _groundDrag;
    
    [Header("Jumping")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    private bool _readyToJump;

    [Header("Crouching")]
    private float _startYScale;
    private bool _readyToCrouch;
    
    [Header("Ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _ground;
    private bool _isgrounded;
    
    [SerializeField] private Transform _orientation;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private Rigidbody _rb;
    public bool isWallRide;
    
    /*public MovementState state;

    public enum MovementState
    {
        
    }*/
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _readyToJump = true;
        _startYScale = transform.localScale.y;
        _readyToCrouch = true;
    }

    private void Update()
    {
        _isgrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _ground);
        MyInput();
        SpeedControl();
        _rb.drag = _isgrounded ? _groundDrag : 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space) && _readyToJump && _isgrounded)
        {
            _readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && _readyToCrouch && _isgrounded)
        {
            _readyToCrouch = false;
            Crouch();
        }
    }

    private void MovePlayer()
    {
        _moveDirection = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
        if(_isgrounded) _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        else if(!_isgrounded) _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f *_airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        if (flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitVel = flatVel.normalized * _moveSpeed;
            _rb.velocity = new Vector3(limitVel.x, _rb.velocity.y, limitVel.z);
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }

    private void Crouch()
    {
        StartCoroutine(Crouching());
    }

    private IEnumerator Crouching()
    {
        transform.localScale = new Vector3(transform.localScale.x, 0f, transform.localScale.z);
        _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        _readyToCrouch = true;
        transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
    }
}