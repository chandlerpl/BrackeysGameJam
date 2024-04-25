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
    [SerializeField]
    private float _minYawAngle = -180.0f;
    [SerializeField]
    private float _maxYawAngle = 180.0f;

    protected Quaternion characterTargetRotation;
    protected Quaternion cameraTargetRotation;

    protected Vector3 characterTargetVec;
    protected Vector3 cameraTargetVec;

    public Quaternion CharacterRotation { get => characterTargetRotation; }

    private PlayerInput playerInput;
    private Vector2 _cameraMovement;

    private bool _cameraRelease = false;
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        characterTargetRotation = transform.localRotation;
        cameraTargetRotation = cameraPivot.localRotation;

        playerInput.currentActionMap.FindAction("CameraMove").performed += CameraMove_performed;
        playerInput.currentActionMap.FindAction("CameraMove").canceled += CameraMove_canceled;

        //playerInput.currentActionMap.FindAction("CameraRelease").performed += CameraRelease_performed;

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

    private void CameraRelease_performed(InputAction.CallbackContext obj)
    {
        _cameraRelease = !_cameraRelease;

        if(!_cameraRelease)
        {
            cameraTargetVec.y = 0;
        }
    }

    void Update()
    {
        var pitch = cameraTargetVec.x + (_cameraMovement.y * _verticalSensitivity);

        pitch = Mathf.Clamp(pitch, _minPitchAngle, _maxPitchAngle);
        cameraTargetVec.x = pitch;
        var pitchRotation = Quaternion.Euler(-pitch, 0.0f, 0.0f);

        var yaw = _cameraMovement.x * _lateralSensitivity;
        
        if (!_cameraRelease)
        {
            yaw = characterTargetVec.y + yaw;

            characterTargetVec.y = yaw;
            characterTargetRotation = Quaternion.Euler(0.0f, yaw, 0.0f);
            cameraTargetRotation = pitchRotation;
        } else
        {
            yaw = cameraTargetVec.y + yaw;
            yaw = Mathf.Clamp(yaw, _minYawAngle, _maxYawAngle);

            cameraTargetVec.y = yaw;
            cameraTargetRotation = Quaternion.Euler(0.0f, yaw, 0.0f);
            cameraTargetRotation *= pitchRotation;
        }

        cameraPivot.localRotation = cameraTargetRotation;
    }
}
