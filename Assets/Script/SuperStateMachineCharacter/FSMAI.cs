using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMAI : SuperStateMachine
{
    public enum EntityState {
        Idle,
        Move,
        Rotate,
        Attack
    }

    Animator animator;

    List<GameObject> targets;

    GameObject currentTarget;

    SphereCollider senseCollider;

    TopText topText;

    Spawner spawner;

    private void Awake() {
        animator = GetComponent<Animator>();
        targets = new List<GameObject>();
        senseCollider = GetComponent<SphereCollider>();
        topText = GetComponentInChildren<TopText>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();

        currentState = EntityState.Idle;
    }

#region Idle
    void Idle_EnterState() {
        Debug.Log("Idle_EnterState");
        topText.SetText("CurrentState:" + currentState.ToString());
        animator.Play("Idle");
    }

    void Idle_UpdateState() {
        FindTargets();

        if ((currentTarget == null || currentTarget.activeSelf) && targets.Count > 0) {
            currentTarget = targets[0];
            targets.Remove(currentTarget);
        }

        if (currentTarget != null) {
            var dir = (currentTarget.transform.position - transform.position).normalized;
            rotateAngle = 90f - Mathf.Atan2(dir.z, dir.x) * 57.29578f/*PI / 180*/;
            rotateAngle = (rotateAngle + 360) % 360;
            if (Mathf.Abs(transform.rotation.eulerAngles.y - rotateAngle) > 1f) {
                currentState = EntityState.Rotate;
                return;
            }

            if (Vector3.Distance(transform.position, currentTarget.transform.position) < 1.5) {
                currentState = EntityState.Attack;
            } else {
                currentState = EntityState.Move;
            }
        }
    }

    void Idle_ExitState() {

    }
#endregion

#region Move
    Vector3 moveDirection;
    float moveSpeed = 5;
    void Move_EnterState() {
        topText.SetText("CurrentState:" + currentState.ToString());

        if (currentTarget != null) {
            moveDirection = (currentTarget.transform.position - transform.position).normalized;

            animator.Play("Walk");
            // animator.SetBool("IsWalking", true);
        }
    }

    void Move_UpdateState() {
        if (currentTarget == null) {
            currentState = EntityState.Idle;
            return;
        }

        if (Vector3.Distance(transform.position, currentTarget.transform.position) > 1.5) {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        } else {
            currentState = EntityState.Attack;
        }
    }

    void Move_ExitState() {
        // animator.SetBool("IsWalking", false);
    }
#endregion

#region Rotate
    float rotateAngle;
    float rotateSpeed => 110;
    void Rotate_EnterState() {
        topText.SetText("CurrentState:" + currentState.ToString());

        if (currentTarget == null) {
            currentState = EntityState.Idle;
            return;
        }

        animator.Play("Walk");
        // animator.SetBool("IsWalking", true);
    }

    void Rotate_UpdateState() {
        if (Mathf.Abs(transform.rotation.eulerAngles.y - rotateAngle) > 1f) {
            float angle = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, rotateAngle, rotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        } else {
            currentState = EntityState.Move;
        }
    }

    void Rotate_ExitState() {

    }
#endregion

#region Attack
    void Attack_EnterState() {
        topText.SetText("CurrentState:" + currentState.ToString());

        animator.Play("Attack01");
        // animator.SetBool("IsAttacking", true);
    }

    void Attack_UpdateState() {
        var animatorState = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorState.IsName("Attack01")) {
            float length = animatorState.length;

            if (Time.time - timeEnteredState >= length) {
                spawner.KillGrunt(currentTarget);
                currentTarget = null;
                currentState = EntityState.Idle;
            }
        }
    }

    void Attack_ExitState() {
        // animator.SetBool("IsAttacking", false);
    }
#endregion


    void FindTargets() {
        if (targets.Count != 0) {
            return;
        }

        var hits = Physics.SphereCastAll(transform.position, senseCollider.radius, transform.forward, 100, 1 << LayerMask.NameToLayer("Target"));
        foreach (var hit in hits) {
            var go = hit.collider.gameObject;
            if (!targets.Contains(go)) {
                targets.Add(go);
            }
        }
        // Debug.Log("wtl-----------FindTargets count:" + targets.Count);
    }
}
