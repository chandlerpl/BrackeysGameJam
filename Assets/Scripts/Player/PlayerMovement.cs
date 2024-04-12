using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MouseMovement))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
//[RequireComponent(typeof())]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Height")]
    public float normalHeight = 1f;
    public float crouchHeight = 0.5f;

    [Header("Player Speed")]
    public float crouchSpeed = 1.5f;
    public float walkSpeed = 2.5f;
    public float runSpeed = 4.5f;

    [SerializeField]
    private Animator animator;
    private PlayerInput playerInput;
    private Rigidbody rb;
    private MouseMovement mouseMovement;
    private CapsuleCollider col;

    private Vector2 _playerMovement;
    private float _speed;
    private bool _isSprinting;
    private bool _isCrouching;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mouseMovement = GetComponent<MouseMovement>();
        playerInput = GetComponent<PlayerInput>();
        col = GetComponent<CapsuleCollider>();

        playerInput.currentActionMap.FindAction("Move").performed += Move_performed;
        playerInput.currentActionMap.FindAction("Move").canceled += Move_canceled;
        playerInput.currentActionMap.FindAction("Sprint").performed += Sprint_performed;
        playerInput.currentActionMap.FindAction("Sprint").canceled += Sprint_performed;
        playerInput.currentActionMap.FindAction("Crouch").performed += Crouch_performed;
        playerInput.currentActionMap.FindAction("Crouch").canceled += Crouch_performed;

        _speed = walkSpeed;
    }
    private void Sprint_performed(InputAction.CallbackContext obj)
    {
        if (obj.phase == InputActionPhase.Performed)
        {
            _isSprinting = true;
            _speed = runSpeed;
            animator.SetBool("IsSprinting", true);
        }
        else
        {
            _isSprinting = false;
            _speed = walkSpeed;
            animator.SetBool("IsSprinting", false);
        }
    }

    private void Crouch_performed(InputAction.CallbackContext obj)
    {
        if (obj.phase == InputActionPhase.Performed)
        {
            _isCrouching = true;
            _speed = crouchSpeed;
            //transform.localScale = new Vector3(1, crouchHeight, 1);
            mouseMovement.cameraPivot.localPosition = new Vector3(mouseMovement.cameraPivot.localPosition.x, crouchHeight, mouseMovement.cameraPivot.transform.localPosition.z);
            col.height = 1f;
            col.center = new Vector3(0, .5f, 0);
            animator.SetBool("IsCrouching", true);
        }
        else
        {
            _isCrouching = false;
            _speed = walkSpeed;
            mouseMovement.cameraPivot.localPosition = new Vector3(mouseMovement.cameraPivot.localPosition.x, normalHeight, mouseMovement.cameraPivot.transform.localPosition.z);
            col.height = 2f;
            col.center = new Vector3(0, 1, 0);
            //transform.localScale = new Vector3(1, normalHeight, 1);
            animator.SetBool("IsCrouching", false);
        }
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        _playerMovement = Vector2.zero;
        animator.SetBool("IsMoving", false);
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        _playerMovement = obj.ReadValue<Vector2>();
        animator.SetBool("IsMoving", true);
    }

    private void FixedUpdate()
    {
        Vector3 dir = (transform.forward * _playerMovement.y) + (transform.right * _playerMovement.x);

        rb.velocity = dir * _speed;
        rb.rotation = mouseMovement.CharacterRotation;
    }
}
