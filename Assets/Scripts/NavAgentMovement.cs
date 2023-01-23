using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentMovement : MonoBehaviour
{
    [Header("Private")]
    public GameObject managers;

    public NavMeshAgent navMeshAgent;
    public InputSysManager inputSysManager;
    public Vector3 destination;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        inputSysManager = managers.GetComponentInChildren<InputSysManager>();
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
        destination = transform.position + transform.right * inputSysManager.Input.x +
                      transform.forward * inputSysManager.Input.y;

        // Setting the Nav Mesh Agent's Destination Position
        navMeshAgent.destination = destination;
    }
}