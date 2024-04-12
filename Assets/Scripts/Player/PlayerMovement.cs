using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MouseMovement))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
//[RequireComponent(typeof())]
public class PlayerMovement : MonoBehaviour
{

    private PlayerInput playerInput;
    private Rigidbody rb;
    private MouseMovement mouseMovement;

    private Vector2 _playerMovement;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mouseMovement = GetComponent<MouseMovement>();
        playerInput = GetComponent<PlayerInput>();

        playerInput.currentActionMap.FindAction("Move").performed += Move_performed;
        playerInput.currentActionMap.FindAction("Move").canceled += Move_canceled;
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

        rb.velocity = dir;
    }
}
