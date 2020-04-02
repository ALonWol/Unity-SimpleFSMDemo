using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateMachineBehaviour
{
    private AnimatorAI animatorAI;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animatorAI == null) {
            animatorAI = animator.gameObject.GetComponent<AnimatorAI>();
        }

        // default state's OnStateEnter() will be called multiple times
        // though we GetComponent<AnimatorAI>(), and animatorAI may also be null
        // so here we check it again with '?'
        animatorAI?.topText.SetText("CurrentBehavior:" + this.GetType().ToString());
        animator.Play("Idle", animator.GetLayerIndex("Base Layer"));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        FindTargets();

        if ((animatorAI?.currentTarget == null || animatorAI.currentTarget.activeSelf) && animatorAI?.targets.Count > 0) {
            animatorAI.currentTarget = animatorAI.targets[animatorAI.targets.Count - 1];
            animatorAI.targets.Remove(animatorAI.currentTarget);
        }

        if (animatorAI?.currentTarget != null) {
            var dir = (animatorAI.currentTarget.transform.position - animatorAI.transform.position).normalized;
            animatorAI.rotateAngle = 90f - Mathf.Atan2(dir.z, dir.x) * 57.29578f/*PI / 180*/;
            animatorAI.rotateAngle = (animatorAI.rotateAngle + 360) % 360;
            if (Mathf.Abs(animatorAI.transform.rotation.eulerAngles.y - animatorAI.rotateAngle) > 1f) {
                animator.Play("Rotate", animator.GetLayerIndex("Behaviors"));
                return;
            }

            if (Vector3.Distance(animatorAI.transform.position, animatorAI.currentTarget.transform.position) < 1.5) {
                animator.Play("Attack", animator.GetLayerIndex("Behaviors"));
            } else {
                animator.Play("Move", animator.GetLayerIndex("Behaviors"));
            }
        }
    }

    // public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    // }

    void FindTargets() {
        if (animatorAI?.targets.Count != 0) {
            return;
        }

        var hits = Physics.SphereCastAll(animatorAI.transform.position, animatorAI.senseCollider.radius, animatorAI.transform.forward, 100, 1 << LayerMask.NameToLayer("Target"));
        foreach (var hit in hits) {
            var go = hit.collider.gameObject;
            if (!animatorAI.targets.Contains(go)) {
                animatorAI.targets.Add(go);
            }
        }
        // Debug.Log("wtl-----------FindTargets count:" + animatorAI.targets.Count);
    }
}
