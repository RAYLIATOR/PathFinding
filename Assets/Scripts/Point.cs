using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    //agent
    GameObject agent;
    public float disToAgent;
    //target
    GameObject target;
    public float disToTarget;
    //Accumulated shortest distance
    public float aSD;
    public GameObject from;
    public bool visited;
    public bool passed;

	void Start ()
    {
        agent = GameObject.FindGameObjectWithTag("Agent");
        disToAgent = Vector3.Distance(transform.position, agent.transform.position);
        target = GameObject.FindGameObjectWithTag("Target");
        disToTarget = Vector3.Distance(transform.position, target.transform.position);
        aSD = float.MaxValue;
        from = null;
        visited = false;
        passed = false;
	}
	void Update ()
    {
        disToAgent = Vector3.Distance(transform.position, agent.transform.position);
        disToTarget = Vector3.Distance(transform.position, target.transform.position);
    }
}
