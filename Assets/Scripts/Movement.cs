using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController _controller;

    public Transform groundCheck;

    public LayerMask groundMask;

    public LayerMask wallMask;

    private Vector3 _move;

    private Vector3 _input;

    public float slideSpeedIncrease;
    
    public float slideSpeedDecrease;
    
    public float wallRunSpeedIncrease;
    
    public float wallRunSpeedDecrease;

    private float _speed;

    public float runSpeed;

    public float sprintSpeed;

    public float crouchSpeed;

    public float airSpeed;

    private Vector3 _velocityY;

    private Vector3 _forwardDirection;

    private int _jumpCharges;

    private bool _isGrounded;
    
    private bool _isSprinting;
    
    private bool _isCrouching;

    private bool _isSliding;

    private bool _isWallRunning;

    private float _gravity;

    public float normalGravity;

    public float wallRunGravity;

    public float jumpHeight;

    private float _startHeight;

    private float _crouchHeight = 0.1f;

    private Vector3 _crouchingCenter = new Vector3(0, 1.9f, 0);

    private Vector3 _standingCenter = new Vector3(0, -0.44f, 0);

    private float _slideTimer;

    public float maxSlideTimer;

    private bool _hasWallRun = false;

    private bool _onLeftWall;

    private bool _onRightWall;

    private RaycastHit _leftWallHit;

    private RaycastHit _rightWallHit;

    private Vector3 _wallNormal;

    private Vector3 _lastWallNormal;

    public Camera playerCamera;

    private float _normalFov;

    public float specialFov;

    public float cameraChangeTime;

    public float wallRunTilt;

    public float tilt;
    
    
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _startHeight = transform.localScale.y;
        _normalFov = playerCamera.fieldOfView;
    }

    void IncreaseSpeed(float speedIncrease)
    {
        _speed += speedIncrease;
    }

    void DecreaseSpeed(float speedDecrease)
    {
        _speed -= speedDecrease * Time.deltaTime;
    }

    void HandleInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        _input = transform.TransformDirection(_input);
        _input = Vector3.ClampMagnitude(_input, 1f);

        if (Input.GetKey(KeyCode.Space) && _jumpCharges > 0)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Crouch();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            ExitCrouch();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _isGrounded)
        {
            _isSprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isSprinting = false;
        }
    }

    void CameraEffects()
    {
        float fov = _isWallRunning ? specialFov : _isSliding ? specialFov : _normalFov;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, cameraChangeTime * Time.deltaTime);

        if (_isWallRunning)
        {
            if (_onRightWall)
            {
                tilt = Mathf.Lerp(tilt, wallRunTilt, cameraChangeTime * Time.deltaTime);
            }
            if (_onLeftWall)
            {
                tilt = Mathf.Lerp(tilt, -wallRunTilt, cameraChangeTime * Time.deltaTime);
            }
        }

        if (!_isWallRunning)
        {
            tilt = Mathf.Lerp(tilt, 0f , cameraChangeTime * Time.deltaTime);
        }
    }
    
    void Update()
    {
        HandleInput();
        CheckWallRun();

        if (_isGrounded && !_isSliding)
        {
            GroundedMovment();
        }
        else if (!_isGrounded && !_isWallRunning)
        {
            Airmovement();
        }
        else if (_isSliding)
        {
            SlideMovement();
            DecreaseSpeed(slideSpeedDecrease);
            _slideTimer -= 1f * Time.deltaTime;
            if (_slideTimer < 0)
            {
                _isSliding = false;
            }
        }
        else if (_isWallRunning)
        {
            WallRunMovement();
            DecreaseSpeed(wallRunSpeedDecrease);
        }
        
        CheckGround();
        _controller.Move(_move * Time.deltaTime);
        ApplyGravity();
        CameraEffects();
        
    }

    void GroundedMovment()
    {
        _speed = _isSprinting ? sprintSpeed : _isCrouching ? crouchSpeed : runSpeed;
        if (_input.x != 0)
        {
            _move.x += _input.x * _speed;
        }
        else
        {
            _move.x = 0;
        }
        if (_input.z != 0)
        {
            _move.z += _input.z * _speed;
        }
        else
        {
            _move.z = 0;
        }

        _move = Vector3.ClampMagnitude(_move, _speed);
    }

    void Airmovement()
    {
        _move.x += _input.x * airSpeed;
        _move.z += _input.z * airSpeed;

        _move = Vector3.ClampMagnitude(_move, _speed);
    }

    void SlideMovement()
    {
        _move += _forwardDirection;
        _move = Vector3.ClampMagnitude(_move, _speed);
    }

    void WallRunMovement()
    {
        if (_input.z > (_forwardDirection.z - 10f) && _input.z < (_forwardDirection.z + 10f))
        {
            _move += _forwardDirection;
        }
        else if (_input.z < (_forwardDirection.z - 10f) && _input.z > (_forwardDirection.z + 10f))
        {
            _move.x = 0f;
            _move.z = 0f;
            ExitWallRun();
        }
        _move.x += _input.x * airSpeed;

        _move = Vector3.ClampMagnitude(_move, _speed);
    }

    void CheckGround()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
        if (_isGrounded)
        {
            _jumpCharges = 1;
            _hasWallRun = false;
        }
    }

    void CheckWallRun()
    {
        _onLeftWall = Physics.Raycast(transform.position, -transform.right, out _leftWallHit, 0.7f, wallMask);
        _onRightWall = Physics.Raycast(transform.position, transform.right, out _rightWallHit, 0.7f, wallMask);

        if ((_onRightWall || _onLeftWall) && !_isWallRunning)
        {
            TestWallRun();
        }
        if ((!_onRightWall || !_onLeftWall) && _isWallRunning)
        {
            ExitWallRun();
        }
    }

    void TestWallRun()
    {
        _wallNormal = _onLeftWall ? _leftWallHit.normal : _rightWallHit.normal;
        if (_hasWallRun)
        {
            float wallAngle = Vector3.Angle(_wallNormal, _lastWallNormal);
            if (wallAngle > 15)
            {
                WallRun();
            }
        }
        else
        {
            WallRun();
            _hasWallRun = true;
        }
    }

    void ApplyGravity()
    {
        _gravity = _isWallRunning ? wallRunGravity : normalGravity;
        _velocityY.y += _gravity * Time.deltaTime;
        _controller.Move(_velocityY * Time.deltaTime);

    }

    void Jump()
    {
        if (!_isGrounded && !_isWallRunning)
        {
            _jumpCharges -= 1;
        }
        else if (_isWallRunning)
        {
           ExitWallRun();
           IncreaseSpeed(wallRunSpeedIncrease);
        }
        _velocityY.y = Mathf.Sqrt(jumpHeight * -2f * normalGravity);
    }

    void Crouch()
    {
        _controller.height = _crouchHeight;
        _controller.center = _crouchingCenter;
        transform.localScale = new Vector3(transform.localScale.x, _crouchHeight, transform.localScale.z);
        _isCrouching = true;
        if (_speed > runSpeed)
        {
            _isSliding = true;
            _forwardDirection = transform.forward;
            if (_isGrounded)
            {
                IncreaseSpeed(slideSpeedIncrease);
            }

            _slideTimer = maxSlideTimer;
        }
    }

    void ExitCrouch()
    {
        _controller.height = _startHeight;
        _controller.center = _standingCenter;
        transform.localScale = new Vector3(transform.localScale.x, _startHeight, transform.localScale.z);
        _isCrouching = false;
        _isSliding = false;
    }

    void WallRun()
    {
        _isWallRunning = true;
        _jumpCharges = 1;
        IncreaseSpeed(wallRunSpeedIncrease);
        _velocityY = new Vector3(0f, 0f, 0f);

        _forwardDirection = Vector3.Cross(_wallNormal, Vector3.up);

        if (Vector3.Dot(_forwardDirection, transform.forward) < 0)
        {
            _forwardDirection = -_forwardDirection;
        }
    }

    void ExitWallRun()
    {
        _isWallRunning = false;
        _lastWallNormal = _wallNormal;
    }
}
