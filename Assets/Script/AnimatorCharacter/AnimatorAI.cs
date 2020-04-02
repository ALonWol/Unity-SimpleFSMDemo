using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAI : MonoBehaviour
{
    public Animator animator {get;set;}

    public List<GameObject> targets {get;set;}

    public GameObject currentTarget {get;set;}

    public SphereCollider senseCollider {get;set;}

    public TopText topText {get;set;}

    public float rotateAngle {get;set;}

    public Spawner spawner {get;set;}

    public Vector3 moveDirection {get;set;}

    public float moveSpeed => 5.5f;

    public float rotateSpeed => 110;

    void Awake()
    {
        animator = GetComponent<Animator>();
        targets = new List<GameObject>();
        senseCollider = GetComponent<SphereCollider>();
        topText = GetComponentInChildren<TopText>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }

    // void Update()
    // {
        
    // }
}
