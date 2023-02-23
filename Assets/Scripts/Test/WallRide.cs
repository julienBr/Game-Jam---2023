using System;
using UnityEngine;

public class WallRide : MonoBehaviour
{
    [Header("WallRide")]
    [SerializeField] private LayerMask _wall;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private float _wallRideForce;
    [SerializeField] private float _maxWallRideTime;
    private float _wallRideTimer;
    private float _horizontalInput;
    private float _verticalInput;

    [Header("Detection")]
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _minJumpHeight;
    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;
    private bool _leftWall;
    private bool _rightWall;

    [Header("References")]
    [SerializeField] private Transform _orientation;
    private PlayerMovement _playerMovement;
    private Rigidbody _rb;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(_playerMovement.isWallRide) WallRideMovement();
    }

    private void CheckForWall()
    {
        _leftWall = Physics.Raycast(transform.position, -_orientation.right, out _leftWallHit, _wallCheckDistance,
            _wall);
        _rightWall = Physics.Raycast(transform.position, _orientation.right, out _rightWallHit, _wallCheckDistance,
            _wall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, _minJumpHeight, _ground);
    }

    private void StateMachine()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        if ((_leftWall || _rightWall) && _verticalInput > 0f && AboveGround())
        {
            if(!_playerMovement.isWallRide) StartWallRide();
        }
        else
        {
            if(_playerMovement.isWallRide) StopWallRide();
        }
    }

    private void StartWallRide()
    {
        _playerMovement.isWallRide = true;
    }

    private void WallRideMovement()
    {
        _rb.useGravity = false;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        Vector3 wallNormal = _rightWall ? _rightWallHit.normal : _leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
        _rb.AddForce(wallForward * _wallRideForce, ForceMode.Force);
    }

    private void StopWallRide()
    {
        _playerMovement.isWallRide = false;
    }
}