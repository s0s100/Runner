using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFootsteps : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        PlayerController playerController = animator.gameObject.GetComponent<PlayerController>();
        playerController.SetFootstepParticleState(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        PlayerController playerController = animator.gameObject.GetComponent<PlayerController>();
        playerController.SetFootstepParticleState(false);
    }
}
