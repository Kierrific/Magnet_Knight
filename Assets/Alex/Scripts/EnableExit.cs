using UnityEngine;

public class EnableExit : StateMachineBehaviour
{
    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        if (stateInfo.IsTag("MeleeAttack"))
        {
            animator.SetBool("CanMove", true);
        }
    }
}
