using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NavAgentMovement : MonoBehaviour
{
    [Header("Private")]
    public GameObject managers;

    public NavMeshAgent navMeshAgent;
    [FormerlySerializedAs("inputSysManager")]
    public InputSysMan inputSysMan;
    public Vector3 destination;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        inputSysMan = managers.GetComponentInChildren<InputSysMan>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        destination = transform.position + transform.right * inputSysMan.Input.x +
                      transform.forward * inputSysMan.Input.y;

        // Setting the Nav Mesh Agent's Destination Position
        navMeshAgent.destination = destination;
    }
}