using UnityEngine;

public class WallRide : MonoBehaviour
{
    [Header("WallRide")]
    [SerializeField] private LayerMask _wall;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private float _wallRideForce;
    [SerializeField] private float _maxWallRideTime;
    private float _wallRideTimer;

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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if ((_leftWall || _rightWall) && verticalInput > 0f && AboveGround())
        {
            
        }
    }
}