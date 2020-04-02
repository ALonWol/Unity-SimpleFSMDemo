using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : StateMachineBehaviour
{
    private AnimatorAI animatorAI;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animatorAI == null) {
            animatorAI = animator.gameObject.GetComponent<AnimatorAI>();
        }

        animatorAI.topText.SetText("CurrentState:" + this.GetType().ToString());

        if (animatorAI.currentTarget != null) {
            animatorAI.moveDirection = (animatorAI.currentTarget.transform.position - animatorAI.transform.position).normalized;

            animator.Play("Walk", animator.GetLayerIndex("Base Layer"));
        }

        animator.Play("Walk", animator.GetLayerIndex("Base Layer"));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animatorAI.currentTarget == null) {
            animator.Play("Idle", animator.GetLayerIndex("Behaviors"));
            return;
        }

        if (Vector3.Distance(animatorAI.transform.position, animatorAI.currentTarget.transform.position) > 1.5) {
            animatorAI.transform.position += animatorAI.moveDirection * animatorAI.moveSpeed * Time.deltaTime;
        } else {
            animator.Play("Attack", animator.GetLayerIndex("Behaviors"));
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    }
}
