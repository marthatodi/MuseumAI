using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using JetBrains.Annotations;

public class Player : MonoBehaviour
{
    //Create your own logic here
    //You need to disable the PlayerPointClickMovement script
    [Header("Waypoints")]
    public Transform[] pointsImber;
    public Transform[] pointsSoure;

    [Header("NaMeshAgent")]
    public NavMeshAgent agent;

    [Header("Animator")]
    public Animator anim;

    [Header("Buttons Panel")]
    public GameObject buttonsPanel;
    public GameObject gameOverPanel;

    [Header("Player")]
    public Transform player;

    [Header("Enemy")]
    public Transform enemy;

    [Header("Text UI")]
    public Text text;

    private int destPoint = 0;
    private int buttonPressed = 0;
    private bool lastPoint = false;

    private float dist = 0;
    //private int next = 0;
    private string[] thoughtsList = { "Nice painting", "Boring", "Pretty cool!"};

    enum TypeOfPainting { Imber, Soure, Other };
    TypeOfPainting typeOfPainting;

    GameObject lWP;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim.SetTrigger("idleTrigger"); //Starting animation
        lWP = GameObject.FindWithTag("LastWP");
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(player.position, lWP.transform.position);
               dist = Vector3.Distance(player.position, enemy.position); //Returns the distance between a and b...Vector3.Distance(a,b) is the same as (a-b).magnitude

            if (dist < 8)  //Check if enemy is close by
            {
             if(dist<1.30){
              Debug.Log("*********The paintLover get caught !Game over! :( ");
              text.text = "F**ck!I get caught!";
              lastPoint = true;
              agent.speed = 0;
              //next = 0;
              anim.SetTrigger("idleTrigger");
              buttonsPanel.SetActive(false);
              gameOverPanel.SetActive(true);
              // gameOverPanel.SendMessageUpwards("You Lose!");
                 return;
           
             }else if(dist<6){//otan arxisei na thn kunigaei
                 text.text =  "Sh**t he saw me!Gotta get out fast! ";
                 lastPoint = true;
                 agent.destination = lWP.transform.position;
                 text.text =  "I missed:"+( pointsSoure.Length-destPoint);
                return;
                }
             else{
                Debug.Log("*********Enemy is closed dists : "+dist);
                 destPoint = (destPoint - 1) % pointsImber.Length;
                  if(destPoint==-1){//an den exei dei kanena mexri twra vriskei to epomeno
                   EnemyIsClose();
                  }else{ //alliws gurnaei ston proigoumeno pinaka
                      destPoint = (destPoint + 1) % pointsImber.Length; 
                  }
               
              //  EnemyIsClose();
             }
               
            }
          if (distance <= 1f)
          {
            Debug.Log("*********Last point");
            lastPoint = true;
            agent.speed = 0;
            //next = 0;
            anim.SetTrigger("idleTrigger");

            buttonsPanel.SetActive(false);
            gameOverPanel.SetActive(true);
          

          }
  
        
        if (lastPoint == false)
        {
              
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GotoNextPoint(buttonPressed);
            }

            
            
            //Debug.Log("Distance to other: " + dist);
        }
    }

    public void GotoNextPoint(int button)
    {
        buttonPressed = button;

        if (button == 1)
        {
            typeOfPainting = TypeOfPainting.Imber;
            buttonsPanel.SetActive(false);

            // Returns if no points have been set up
            if (pointsImber.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = pointsImber[destPoint].position;

            if (destPoint != 0)
            {
                StartCoroutine(PlayerStop());
            }
            else
            {
                anim.SetTrigger("runTrigger");
            }

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % pointsImber.Length;

        }
        else if (button == 2)
        {
            typeOfPainting = TypeOfPainting.Soure;
            //next += 1;
            buttonsPanel.SetActive(false);

            // Returns if no points have been set up
            if (pointsSoure.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = pointsSoure[destPoint].position;

            if (destPoint != 0)
            {
                StartCoroutine(PlayerStop());
            }
            else
            {
                anim.SetTrigger("runTrigger");
            }

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + 1) % pointsSoure.Length;
        }
    }

    public void EnemyIsClose()
    {
        Debug.Log("*********Enemy is closed!");
       agent.speed = 6;
        if (typeOfPainting == TypeOfPainting.Imber)
        {
            if (pointsImber.Length == 0)
                return;
         
            
            FindClosestCheckpoint(true);
       

        }
        else if(typeOfPainting == TypeOfPainting.Soure)
        {
            if (pointsSoure.Length == 0)
                return;
          
            
               FindClosestCheckpoint(true);
        }
        else
        {
            //TODO the same for other Waypoints
        }
    }

    private void FindClosestCheckpoint(bool isImber)
    {

        //BUG if the enemy destination is the waypoint that you are about to go, it gets stuck there.
        //Need to go to a waypoint that is (dist > 8f) for example
        if(dist<6){//otan arxisei na thn kunigaei
                text.text =  "Sh**t he saw me!Gotta get out fast! ";
                 lastPoint = true;
                 return;
                }
               else if(dist<1){
                     Debug.Log("*********The paintLover get caught !Game over! :( ");
              text.text = "F**ck!I get caught!";
             lastPoint = true;
              agent.speed = 0;
            //next = 0;
            anim.SetTrigger("idleTrigger");
              buttonsPanel.SetActive(false);
              gameOverPanel.SetActive(true);
           //   gameOverPanel.SendMessageUpwards("You Lose!");
            return;  
               }
        if (isImber)
        {
            if (pointsImber.Length == 0) return;
            float lastDist = 500f;//Vector3.Distance(this.transform.position, pointsImber[0].transform.position);
            for (int i = 1; i < pointsImber.Length - 1; i++)
            {
                float closestDist = Vector3.Distance(this.transform.position, pointsImber[i].position);
                if (lastDist > closestDist)
                {
                    agent.destination = pointsImber[i].position;
                    Debug.Log("Closest waypoint is: " + pointsImber[i].name);
                }
            }
        }
        else
        {
            if (pointsSoure.Length == 0) return;
            float lastDist = 500f;//Vector3.Distance(this.transform.position, pointsImber[0].transform.position);
            for (int i = 1; i < pointsSoure.Length - 1; i++)
            {
                float closestDist = Vector3.Distance(this.transform.position, pointsSoure[i].position);
                if (lastDist > closestDist)
                {
                   
                    agent.destination = pointsSoure[i].position;
                    Debug.Log("Closest waypoint is: " + pointsImber[i].name);
                }
            }
        }
        
    }


    IEnumerator PlayerStop()
    {
       // Debug.Log("Player Started Coroutine at timestamp : " + Time.time);

        var randomNumber = Random.Range(0, 2);

        agent.speed = 0;    //Navmesh agent speed (player speed)
        anim.SetTrigger("idleTrigger"); //Animation to play
        text.text = thoughtsList[randomNumber];   //Text to show when stoping

        yield return new WaitForSeconds(3); //Stop for 3 seconds

        //Debug.Log("Player Finished Coroutine at timestamp : " + Time.time);
        text.text = ""; //Text to show when stoping
        agent.speed = 4; //Navmesh agent speed (player speed)
        anim.SetTrigger("runTrigger");  //Animation to play
    }
}
