using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Type6_MobLeader : MonoBehaviour
{
    private GameObject[] teamMembers;
    int wpIndex = 0;
    public GameObject[] wps;
    
    private Animator anim;
    private AnimatorStateInfo info;
    private UnityEngine.AI.NavMeshAgent agent;

    private GameObject player;

    public int health;
    
    // Start is called before the first frame update
    void Start()
    {
        teamMembers = GameObject.FindGameObjectsWithTag("Type6");

        wps = GameObject.FindGameObjectsWithTag("Waypoint");
        
        anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        
        player = GameObject.FindGameObjectWithTag("Player");
        
        anim.SetBool("Patrolling", true);
    }

    // Update is called once per frame
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        
        if (info.IsName("Idle"))
        {
            agent.isStopped = true;

            int currentMemberCount = 0;
            foreach (var member in teamMembers)
            {
                if (member != null)
                {
                    currentMemberCount++;
                }
            }

            if (currentMemberCount < teamMembers.Length)
            {
                TeamRetreat();
                anim.SetBool("Patrolling", true);
            }
        }

        if (info.IsName("Patrol"))
        {
            NewLook();
            Listen();
            
            if (Vector3.Distance(gameObject.transform.position, wps[wpIndex].transform.position) < 1.0f)
            {
                wpIndex++;
                if (wpIndex >= wps.Length)
                {
                    wpIndex = 0;
                }
            }

            agent.SetDestination(wps[wpIndex].transform.position);
            agent.isStopped = false;
        }
    }
    
    void NewLook()
    {
        Vector3 direction = (GameObject.Find("Player").transform.position - transform.position).normalized;

        bool isInTheFieldOfView = (Vector3.Dot(transform.forward.normalized, direction) > 0.7f);
        
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
                anim.SetBool("Patrolling", false);
                TeamAttack();
            }
        }
    }
    
    void Listen()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < 3)
        {
             anim.SetBool("Patrolling", false);
             TeamAttack();
        }
    }
    
    void TeamAttack()
    {
        for (int i = 0; i < teamMembers.Length; i++)
        {
            teamMembers[i].GetComponent<Follower>().Attack(player);
        }
    }

    void TeamRetreat()
    {
        foreach (var teamMember in teamMembers)
        {
            if (teamMember != null)
            {
                teamMember.GetComponent<Follower>().Retreat();
                anim.SetBool("Patrolling", true);
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
