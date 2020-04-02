using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : StateMachineBehaviour
{
    private AnimatorAI animatorAI;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animatorAI == null) {
            animatorAI = animator.gameObject.GetComponent<AnimatorAI>();
        }

        animatorAI.topText.SetText("CurrentBehavior:" + this.GetType().ToString());

        if (animatorAI.currentTarget == null) {
            animator.Play("Idle", animator.GetLayerIndex("Behaviors"));
            return;
        }

        animator.Play("Walk", animator.GetLayerIndex("Base Layer"));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Mathf.Abs(animatorAI.transform.rotation.eulerAngles.y - animatorAI.rotateAngle) > 1f) {
            float angle = Mathf.MoveTowardsAngle(animatorAI.transform.rotation.eulerAngles.y, animatorAI.rotateAngle, animatorAI.rotateSpeed * Time.deltaTime);
            animatorAI.transform.rotation = Quaternion.Euler(0, angle, 0);
        } else {
            animator.Play("Move", animator.GetLayerIndex("Behaviors"));
        }
    }

    // public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    // }
}
