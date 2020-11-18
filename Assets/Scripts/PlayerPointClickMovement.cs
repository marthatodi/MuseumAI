using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerPointClickMovement : MonoBehaviour
{

    NavMeshAgent agent;
    public Animator anim;
    public Transform player;

    enum AIAnimationState { Idle, Walk, Run };
    AIAnimationState animState = AIAnimationState.Idle;

    //bool idle = false;
    //bool run  = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim.SetTrigger("idleTrigger");
        //idle = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position == agent.destination) // If the players position is equal to your mouse click position then play idle animation
        {
            anim.SetTrigger("idleTrigger");
        }
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.destination = hit.point;
                anim.SetTrigger("runTrigger");
            }
        }
    }

    //Not used...Left them as reference
    void PlayerAnimState()
    {
        if (agent.speed == 1)
        {
            animState = AIAnimationState.Walk;
        }
        else if (agent.speed == 4)
        {
            animState = AIAnimationState.Run;
        }
        else
        {
            animState = AIAnimationState.Idle;
        }
    }
    void PlayerAnimStateTrigger()
    {
        if (animState == AIAnimationState.Walk)
        {
            anim.SetTrigger("walkTrigger");
        }
        else if (animState == AIAnimationState.Run)
        {
            anim.SetTrigger("runTrigger");
        }
        else
        {
            anim.SetTrigger("idleTrigger");
        }
    }
}
