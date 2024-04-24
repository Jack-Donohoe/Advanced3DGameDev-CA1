using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    public enum FollowerType
    {
        Type5_Team,
        Type6_Mob
    }

    public FollowerType type;
    
    private GameObject leader;
    private Animator anim;
    private AnimatorStateInfo info; 
    
    private UnityEngine.AI.NavMeshAgent agent;
    private GameObject target;
    public int health;
    private int damage;
    
    // Start is called before the first frame update
    void Start()
    {
        if (type == FollowerType.Type5_Team)
        {
            leader = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            leader = GameObject.FindGameObjectWithTag("MobLeader");
        }

        anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        
        float distanceToLeader = Vector3.Distance(transform.position, leader.transform.position);
        if (distanceToLeader < 5)
        {
            anim.SetBool("NearLeader", true);
        }
        else
        {
            anim.SetBool("NearLeader", false);
        }

        if (info.IsName("Idle"))
        {
            agent.isStopped = true;
        }
        
        if(info.IsName("MoveTowardLeader"))
        {
            agent.isStopped = false;
            agent.SetDestination(leader.transform.position);
        }
        
        if(info.IsName("MoveToTarget"))
        {
            if (target != null)
            {
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distanceToTarget < 2.25f)
                {
                    anim.SetBool("NearTarget", true);
                    agent.isStopped = true;
                }
                else
                {
                    anim.SetBool("NearTarget", false);
                }
            }
            else
            {
                anim.SetBool("TargetDead", true);
            }
        }
        
        if(info.IsName("AttackTarget"))
        {
            if (target != null)
            {
                agent.isStopped = true;
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                if(info.normalizedTime %1f >= .98f)
                {
                    if (type == FollowerType.Type6_Mob)
                    {
                        damage = 10;
                    }
                    else
                    {
                        damage = 20;
                    }
                    
                    target.GetComponent<Follower>().TakeDamage(gameObject, damage);
                }
            }
            else
            {
                anim.SetBool("TargetDead", true);
            }
        }
    }

    public void Attack(GameObject attackTarget)
    {
        target = attackTarget;
        anim.SetTrigger("Defend");
    }

    public void Retreat()
    {
        anim.SetTrigger("Retreat");
    }

    public void TakeDamage(GameObject attacker, int damageTaken)
    {
        health -= damageTaken;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        
        Attack(attacker);
    }
}
