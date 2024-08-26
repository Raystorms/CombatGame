using Cinemachine;
using UnityEngine;

public class CameraControlSystem : MonoBehaviour
{
    [SerializeField]
    private Transform _cinemachineTarget;
    [SerializeField]
    private CinemachineVirtualCamera _cinemachineCamera;

    [SerializeField]
    private float _mouseSensitivity = 5.0f;
    [SerializeField]
    private Vector2 _cameraXMax = new Vector2(-50, 85);


    private Vector3 _cameraAngle;
    //_cameraAngleDelta currently not used anywhere, but i do this as a standard because in most game we can make use of this
    private Vector2 _cameraAngleDelta;
    public Vector2 CameraAngleDelta { get => _cameraAngleDelta; }

    // Start is called before the first frame update
    void Start()
    {
        if (_cinemachineTarget == null)
        {
            //Find Object, very dirty way of doing this, not reccomended
            _cinemachineTarget = FindObjectOfType<CinemachineTargetGroup>().transform;
        }

        if (_cinemachineCamera == null)
        {
            _cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            _cameraAngle.x -= Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
            _cameraAngle.y += Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
            _cameraAngleDelta.x = -Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;
            _cameraAngleDelta.y = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;

            _cameraAngle.x = Mathf.Clamp(_cameraAngle.x, _cameraXMax.x, _cameraXMax.y);

            _cinemachineTarget.eulerAngles = _cameraAngle;
            _cameraAngle.y %= 360f;
        }
    }
}
