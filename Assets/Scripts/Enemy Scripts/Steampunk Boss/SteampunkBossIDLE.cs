using UnityEngine;

// Defines next possible step
public class SteampunkBossIDLE : StateMachineBehaviour
{
    private static float MIN_MEELE_ATTACK_DIST = 3.0f;

    private float curTime = 0.0f;
    private float decisionTime = 1.0f;
    private GameObject playerObject;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        curTime = decisionTime;  
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        curTime -= Time.deltaTime;
        if (curTime <= 0.0f)
        {
            GameObject bossObject = animator.gameObject.transform.parent.gameObject;

            bool isClose = (bossObject.transform.position - playerObject.transform.position).magnitude < MIN_MEELE_ATTACK_DIST;
            // Debug.Log("Is " + (bossObject.transform.position - playerObject.transform.position).magnitude + " <" + MIN_MEELE_ATTACK_DIST + " = " + isClose);
            if (isClose)
            {
                RandomMeeleAttack(animator);
            } else
            {
                RandomLazerAttack(animator);
            }
        }
    }

    private void RandomMeeleAttack(Animator animator)
    {
        animator.SetTrigger("HandAttack");
        //bool isFist = Random.value < 0.5f;
        //if (isFist)
        //{
        //    animator.SetTrigger("HandAttack");
        //} else
        //{
        //    animator.SetTrigger("FireAttack");
        //}

    }

    private void RandomLazerAttack(Animator animator)
    {
        bool isLazerFront = Random.value < 0.5f;
        if (isLazerFront)
        {
            animator.SetTrigger("LazerFront");
        } else
        {
            animator.SetTrigger("LazerTopBottom");
        }
    }
}
