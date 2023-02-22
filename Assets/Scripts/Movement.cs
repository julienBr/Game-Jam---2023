using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController _controller;

    public Transform groundCheck;

    public LayerMask groundMask;

    private Vector3 _move;

    private Vector3 _input;

    public float slideSpeedIncrease;
    
    public float slideSpeedDecrease;

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

    private float _gravity;

    public float normalGravity;

    public float jumpHeight;

    private float _startHeight;

    private float _crouchHeight = 0.1f;

    private Vector3 _crouchingCenter = new Vector3(0, 1.9f, 0);

    private Vector3 _standingCenter = new Vector3(0, -0.44f, 0);

    private float _slideTimer;

    public float maxSlideTimer;
    
    
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _startHeight = transform.localScale.y;
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

    
    void Update()
    {
        HandleInput();

        if (_isGrounded && !_isSliding)
        {
            GroundedMovment();
        }
        else if (!_isGrounded)
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
        
        CheckGround();
        _controller.Move(_move * Time.deltaTime);
        ApplyGravity();
        
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

    void CheckGround()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
        if (_isGrounded)
        {
            _jumpCharges = 1;
        }
    }

    void ApplyGravity()
    {
        _gravity = normalGravity;
        _velocityY.y += _gravity * Time.deltaTime;
        _controller.Move(_velocityY * Time.deltaTime);

    }

    void Jump()
    {
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
}
