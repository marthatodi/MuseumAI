using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class AIPatrol : MonoBehaviour
{
    [Header("Waypoints")]       
    public Transform[] points;
    
    [Header("NavMeshAgent")]
    private NavMeshAgent agent;

    [Header("Animator")]
    public Animator anim;

    [Header("Player")]
    public Transform player;


    GameObject lWP;
    //private int destPoint = 0;
    enum AIAnimationState { Idle, Walk, Run };
    AIAnimationState animState = AIAnimationState.Idle;

    enum EnemyState { Chasing, Patrolling};
    EnemyState enemyState = EnemyState.Patrolling;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
       lWP = GameObject.FindWithTag("LastWP");
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
        EnemyAIState();
        GotoNextPoint();
        anim.SetTrigger("walkTrigger");
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        //Go to random point from the list
        int randomNumber = Random.Range(0, 13);
        agent.destination = points[randomNumber].position;

        /*
        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
        */
    }

    void Update()
    {
        
            float dist = Vector3.Distance(player.position, agent.transform.position);
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                 agent.speed=0;
                StartCoroutine(EnemyStop());
                GotoNextPoint();
            }
        
      
         if (dist < 6)  //Check if enemy is close by
              //Need to add logic on how to chase player (When you get too close to enemy or when the enemy sees you)
        {
             Debug.Log("*********Start chasing the Player! The dists is : "+lWP.transform.position+", "+agent.transform.position);
           
            if(!player.position.Equals(lWP.transform.position)){//oso den exei vgei apo to mouseio thn kunhgaei (-18.9, 0.6, -5.8)
            if(dist<1.30){
                agent.speed = 0;
                anim.SetTrigger("idleTrigger");
                return;
            }else{
            Debug.Log("Enemy is getting claused dist: : "+dist);
            ChasePlayer();
            }
            }
            else{
                 Debug.Log("She get out!!: : "+dist);
                 agent.speed = 0;
                anim.SetTrigger("idleTrigger");
                return;
           //  Start();
            }
           
            
             
        }
       
     //   ChasePlayer();
    }

    void EnemyAIState()
    {
        if (agent.speed == 1)
        {
            animState = AIAnimationState.Walk;
        }
        else if (agent.speed == 3)
        {
            animState = AIAnimationState.Run;
        }
        else
        {
            animState = AIAnimationState.Idle;
        }

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

    void ChasePlayer()
    {
        //if (Input.GetKeyDown(KeyCode.Z))    //TODO: Change theses to Chase Player Function;
       // {
             Debug.Log("*********Enemy chasing");
            agent.speed = 3;
            EnemyAIState();
            agent.SetDestination(player.position);
            enemyState = EnemyState.Chasing;
            
       // }
    }

    IEnumerator EnemyStop()
    {
        //Debug.Log("Enemy Started Coroutine at timestamp : " + Time.time);
        agent.speed = 0;
        anim.SetTrigger("idleTrigger");
        yield return new WaitForSeconds(3);
        //Debug.Log("Enemy Finished Coroutine at timestamp : " + Time.time);
        agent.speed = 1.5f;
        anim.SetTrigger("walkTrigger");
        
    }
}