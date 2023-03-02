using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform _cameraPosition;
    private Animator _animator;

    private void OnEnable()
    {
        PlayerMovement.CameraCrouch += CrouchCamera;
        PlayerMovement.CameraLook += LookCamera;
    }

    private void OnDisable()
    {
        PlayerMovement.CameraCrouch -= CrouchCamera;
        PlayerMovement.CameraLook -= LookCamera;
    }

    private void Start() { _animator = GetComponent<Animator>(); }

    private void Update() { transform.position = _cameraPosition.position; }
    
    private void CrouchCamera(bool camera)
    {
        if (!camera) { _animator.SetTrigger("Crouch"); }
    }

    private void LookCamera(bool camera)
    {
        _animator.SetBool("LookBack", camera);
    }
}