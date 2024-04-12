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

    private PlayerInput playerInput;
    private Rigidbody rb;
    private MouseMovement mouseMovement;

    private Vector2 _playerMovement;
    private float _speed;
    private bool _isSprinting;
    private bool _isCrouching;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mouseMovement = GetComponent<MouseMovement>();
        playerInput = GetComponent<PlayerInput>();

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
        }
        else
        {
            _isSprinting = false;
            _speed = walkSpeed;
        }
    }

    private void Crouch_performed(InputAction.CallbackContext obj)
    {
        if (obj.phase == InputActionPhase.Performed)
        {
            _isCrouching = true;
            _speed = crouchSpeed;
            transform.localScale = new Vector3(1, crouchHeight, 1);
        }
        else
        {
            _isCrouching = false;
            _speed = walkSpeed;
            transform.localScale = new Vector3(1, normalHeight, 1);
        }
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        _playerMovement = Vector2.zero;
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        _playerMovement = obj.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 dir = (transform.forward * _playerMovement.y) + (transform.right * _playerMovement.x);

        rb.velocity = dir * _speed;
        rb.rotation = mouseMovement.CharacterRotation;
    }
}
