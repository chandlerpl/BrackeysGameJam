using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovement : MonoBehaviour
{
    public Transform cameraPivot;
    [SerializeField]
    private float _lateralSensitivity = 2.0f;
    [SerializeField]
    private float _verticalSensitivity = 2.0f;
    [SerializeField]
    private float _minPitchAngle = -90.0f;

    [SerializeField]
    private float _maxPitchAngle = 90.0f;

    protected Quaternion characterTargetRotation;
    protected Quaternion cameraTargetRotation;

    public Quaternion CharacterRotation { get => characterTargetRotation; }

    private PlayerInput playerInput;
    private Vector2 _cameraMovement;
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        characterTargetRotation = transform.localRotation;
        cameraTargetRotation = cameraPivot.localRotation;

        playerInput.currentActionMap.FindAction("CameraMove").performed += CameraMove_performed;
        playerInput.currentActionMap.FindAction("CameraMove").canceled += CameraMove_canceled;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CameraMove_canceled(InputAction.CallbackContext obj)
    {
        _cameraMovement = Vector2.zero;
    }

    private void CameraMove_performed(InputAction.CallbackContext obj)
    {
        _cameraMovement = obj.ReadValue<Vector2>();
    }


    void Update()
    {
        var yaw = _cameraMovement.x * _lateralSensitivity;
        var pitch = _cameraMovement.y * _verticalSensitivity;

        var yawRotation = Quaternion.Euler(0.0f, yaw, 0.0f);
        var pitchRotation = Quaternion.Euler(-pitch, 0.0f, 0.0f);

        characterTargetRotation *= yawRotation;

        cameraTargetRotation *= pitchRotation;
        cameraTargetRotation = ClampPitch(cameraTargetRotation);

        cameraPivot.localRotation = cameraTargetRotation;
    }

    protected Quaternion ClampPitch(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        var pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        pitch = Mathf.Clamp(pitch, _minPitchAngle, _maxPitchAngle);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

        return q;
    }
}
