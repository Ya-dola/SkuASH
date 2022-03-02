using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentMovement : MonoBehaviour
{
    [Header("Private")]
    public NavMeshAgent navMeshAgent;

    public CharacterMoveController charMoveCtrl;

    public Vector3 destination;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        charMoveCtrl = GetComponent<CharacterMoveController>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (charMoveCtrl.isMovementPressed)
            Move();
    }

    private void Move()
    {
        destination = transform.position + transform.right * charMoveCtrl.input.x +
                      transform.forward * charMoveCtrl.input.y;

        // Setting the Nav Mesh Agent's Destination Position
        navMeshAgent.destination = destination;
    }
}