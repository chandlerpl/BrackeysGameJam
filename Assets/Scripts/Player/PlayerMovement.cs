using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MouseMovement))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
//[RequireComponent(typeof())]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Height")]
    public Vector3 normalCameraPosition = new Vector3(0, 1.5f, 0f);
    public Vector3 crouchCameraPosition = new Vector3(0, 1f, 0f);

    public float crouchingSpeed = 1.5f;
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

    private Inventory _inventory;

    private Vector2 _playerMovement;
    private float _speed;
    private bool _isSprinting;
    private bool _isCrouching;

    private Vector3 startPosition;

    private List<uint> _trapsPlayerHit = new();
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

        if (TryGetComponent(out _inventory))
        {
            playerInput.currentActionMap.FindAction("Interact").performed += Interact_performed;
        }

        _speed = walkSpeed;
        startPosition = transform.position;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        _inventory.Interact();
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
            animator.SetBool("IsCrouching", true);
            StartCoroutine(CrouchCharacter());
        }
        else
        {
            _isCrouching = false;
            animator.SetBool("IsCrouching", false);
            StartCoroutine(CrouchCharacter());
        }
    }

    private IEnumerator CrouchCharacter()
    {
        float time = 0;

        while (time < 1f)
        {
            yield return new WaitForEndOfFrame();

            time += Time.deltaTime * crouchingSpeed;

            if(_isCrouching)
            {
                _speed = crouchSpeed;
                mouseMovement.cameraPivot.localPosition = Vector3.Lerp(normalCameraPosition, crouchCameraPosition, time);
                col.height = Mathf.Lerp(1.8f, 1f, time);
                col.center = new Vector3(0, Mathf.Lerp(.9f, .5f, time), 0.1f);
            } else
            {
                _speed = walkSpeed;
                mouseMovement.cameraPivot.localPosition = Vector3.Lerp(crouchCameraPosition, normalCameraPosition, time);
                col.height = Mathf.Lerp(1f, 1.8f, time);
                col.center = new Vector3(0, Mathf.Lerp(.5f, .9f, time), 0.1f);
            }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TrapTrigger trap))
        {
            _trapsPlayerHit.Add(trap.UniqueID);
        }
    }

    public void RestartPlayer()
    {
        rb.position = startPosition;

        if(_trapsPlayerHit.Count > 0)
        {
            GameManager.Instance.TrapManager.GetTrap(_trapsPlayerHit[Random.Range(0, _trapsPlayerHit.Count)]).Trigger();

            _trapsPlayerHit.Clear();
        }
    }
}
