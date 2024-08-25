using UnityEngine;

public class CharacterMovementControl : MonoBehaviour
{
    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private float _movementSpeed = 5;

    [SerializeField]
    private float _gravity = 20;


    // Update is called once per frame
    void Update()
    {
        var _moveSpeedDelta = _movementSpeed * Time.deltaTime;
        var movementInput = GetMovementInput();
        var moveDirection = new Vector3(movementInput.x * _moveSpeedDelta, -_gravity * Time.deltaTime, movementInput.z * _moveSpeedDelta);

        _characterController.Move(moveDirection);
    }

    public Vector3 GetMovementInput()
    {
        //Vertical
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        Vector3 verticalMovement = forward.normalized * Input.GetAxis("Vertical");

        //Horizontal
        Vector3 horizontalMovement = Camera.main.transform.right * Input.GetAxis("Horizontal");

        //Input Movement
        return (verticalMovement + horizontalMovement).normalized;
    }
}
