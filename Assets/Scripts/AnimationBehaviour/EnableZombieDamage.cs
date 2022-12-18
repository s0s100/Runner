using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableZombieDamage : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Zombie zombie = animator.gameObject.GetComponent<Zombie>();
        zombie.AwakeZombie();
    }
}
