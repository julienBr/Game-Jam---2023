using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField] private float _jump;
    private Animator _camAnimator;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Rigidbody _body;
    private bool _lookBack;

    private void Awake()
    {
        _camAnimator = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _body = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _agent.SetDestination(_target.position);
        Move();
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(!_lookBack) LookBack();
            else LookForward();
        }
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift)) Slide();
    }

    private void Move()
    {
        transform.Translate(Input.GetAxis("Horizontal") * _speed * Time.deltaTime,0f, 0f);
    }

    private void LookBack()
    {
        _lookBack = true;
        _camAnimator.SetBool("LookBack", true);
    }

    private void LookForward()
    {
        _lookBack = false;
        _camAnimator.SetBool("LookBack", false);
    }

    private void Jump()
    {
        _body.AddForce(0f, _jump, 0f);
    }

    private void Slide()
    {
        _animator.SetTrigger("Slide");
    }
}