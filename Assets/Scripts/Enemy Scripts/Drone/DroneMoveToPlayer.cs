using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMoveToPlayer : StateMachineBehaviour
{
    // Shift between camera center location and boss location
    public static float Y_SHIFT = 3.0f;
    public static float X_SHIFT = 4.0f;
    private static float MIN_REQUIRED_DIST = 0.1f;

    private float droneSpeed = 3.0f;

    private PlayerController playerController;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        GameObject droneObject = animator.gameObject.transform.parent.gameObject;
        Vector3 bossLocation = droneObject.transform.position;

        Vector3 targetLocation = playerController.gameObject.transform.position;
        targetLocation += X_SHIFT * Vector3.right;
        targetLocation += Y_SHIFT * Vector3.up;

        Vector2 moveDirection = targetLocation - bossLocation;
        float targetBossDistance = Mathf.Abs(moveDirection.magnitude);

        if (targetBossDistance >= MIN_REQUIRED_DIST)
        {
            // Move to a target position
            moveDirection = moveDirection.normalized;
            Vector3 bossShift = moveDirection * droneSpeed * Time.deltaTime;

            droneObject.transform.position += bossShift;
        }
        else
        {
            animator.SetTrigger("IsMoved");
        }
    }
}
