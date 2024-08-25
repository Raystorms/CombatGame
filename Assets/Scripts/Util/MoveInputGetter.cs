using UnityEngine;

namespace CombatGame.Util
{
    public static class MoveInputGetter
    {
        public static bool IsMovementInput => !Mathf.Approximately(Input.GetAxis("Vertical"), 0) || !Mathf.Approximately(Input.GetAxis("Horizontal"), 0);

        public static Vector3 GetMovementInput(Camera camera)
        {
            //Vertical
            Vector3 forward = camera.transform.forward;
            forward.y = 0;
            Vector3 verticalMovement = forward.normalized * Input.GetAxis("Vertical");

            //Horizontal
            Vector3 horizontalMovement = camera.transform.right * Input.GetAxis("Horizontal");

            //Input Movement
            return (verticalMovement + horizontalMovement).normalized;
        }
    }
}