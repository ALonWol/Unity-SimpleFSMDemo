using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : StateMachineBehaviour
{
    private AnimatorAI animatorAI;

    private float timeEnteredState;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animatorAI == null) {
            animatorAI = animator.gameObject.GetComponent<AnimatorAI>();
        }

        timeEnteredState = Time.time;

        animatorAI.topText.SetText("CurrentBehavior:" + this.GetType().ToString());

        animator.Play("Attack01", animator.GetLayerIndex("Base Layer"));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var animatorState = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorState.IsName("Attack01")) {
            float length = animatorState.length;

            if (Time.time - timeEnteredState >= length) {
                animatorAI.spawner.KillGrunt(animatorAI.currentTarget);
                animatorAI.currentTarget = null;
                animator.Play("Idle", animator.GetLayerIndex("Behaviors"));
            }
        }
    }

    // public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    // }
}
