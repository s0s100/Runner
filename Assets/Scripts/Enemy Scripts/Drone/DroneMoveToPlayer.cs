using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMoveToPlayer : StateMachineBehaviour
{
    // Shift between camera center location and boss location
    public static float Y_SHIFT = -0.5f;
    public static float X_SHIFT = 5.0f;
    private static float MIN_REQUIRED_DIST = 0.1f;

    private float proneSpeed = 3.0f;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        GameObject droneLocation = animator.gameObject.transform.parent.gameObject;
        Vector3 bossLocation = droneLocation.transform.position;

        Vector3 targetLocation = Camera.main.transform.position;
        targetLocation += X_SHIFT * Vector3.right;
        targetLocation += Y_SHIFT * Vector3.up;

        Vector2 moveDirection = targetLocation - bossLocation;
        float targetBossDistance = Mathf.Abs(moveDirection.magnitude);

        if (targetBossDistance >= MIN_REQUIRED_DIST)
        {
            // Move to a target position
            moveDirection = moveDirection.normalized;
            Vector3 bossShift = moveDirection * proneSpeed * Time.deltaTime;

            droneLocation.transform.position += bossShift;
        }
        else
        {
            animator.SetTrigger("IsMoved");
        }
    }
}
