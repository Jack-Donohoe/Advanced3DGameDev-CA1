using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Type1NPC : MonoBehaviour
{
    public GameObject target;
    public GameObject[] waypoints;
    private int waypointCount = 0;
    
    private Animator anim;
    private AnimatorStateInfo info;

    private bool isInTheFieldOfView;
    
    public enum State
    {
        Path,
        ChasePlayer
    };

    public State npcState;

    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }

    // Update is called once per frame
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        npcState = State.Path;
        
        NewLook();
        Listen();
        Smell();
        
        switch (npcState)
        {
            case State.Path:
            {
                target = waypoints[waypointCount];
                
                if (Vector3.Distance(transform.position, target.transform.position) < 1)
                {
                    waypointCount++;
                    if (waypointCount > waypoints.Length - 1) waypointCount = 0;
                }
                
                GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
                break;
            }
            case State.ChasePlayer:
            {
                GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
                
                if (!isInTheFieldOfView)
                {
                    anim.SetBool("SeePlayer", false);
                }
                
                break;
            }
        }
    }
    
    void Listen()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < 3)
        {
            anim.SetBool("HearPlayer", true);
            npcState = State.ChasePlayer;
        }
        else
        {
            anim.SetBool("HearPlayer", false);
        }
    }
    
    void Smell()
    {
        GameObject[] allBC = GameObject.FindGameObjectsWithTag("Breadcrumb");
        
        float minDistance = 2;
        bool detectedBC = false;

        foreach (var BC in allBC)
        {
            if (Vector3.Distance(transform.position, BC.transform.position) < minDistance)
            {
                detectedBC = true;
                break;
            }
        }

        if (detectedBC)
        {
            anim.SetBool("SmellPlayer", true);
            npcState = State.ChasePlayer;
        }
        else
        {
            anim.SetBool("SmellPlayer", false);
        }
    }
    
    void NewLook()
    {
        Vector3 direction = (GameObject.Find("Player").transform.position - transform.position).normalized;

        isInTheFieldOfView = (Vector3.Dot(transform.forward.normalized, direction) > 0.7f);
        
        Debug.DrawRay(transform.position, direction * 100, Color.green);
        Debug.DrawRay(transform.position, transform.forward * 100, Color.cyan);
        
        Debug.DrawRay(transform.position, (transform.forward - transform.right) * 100, Color.red);
        Debug.DrawRay(transform.position, (transform.forward + transform.right) * 100, Color.red);
        
        Ray ray = new Ray();
        RaycastHit hit;

        ray.origin = transform.position + Vector3.up * 0.7f;
        string objectInSight = "";

        float castingDistance = 10;
        ray.direction = transform.forward * castingDistance;
        Debug.DrawRay(ray.origin, ray.direction * castingDistance, Color.red);

        if(Physics.Raycast(ray.origin, direction, out hit, castingDistance)){
            objectInSight = hit.collider.gameObject.name;
            if (objectInSight == "Player" && isInTheFieldOfView)
            {
                anim.SetBool("SeePlayer", true);
                npcState = State.ChasePlayer;
            }
            else
            {
                anim.SetBool("SeePlayer", false);
            }
        }
    }
}
