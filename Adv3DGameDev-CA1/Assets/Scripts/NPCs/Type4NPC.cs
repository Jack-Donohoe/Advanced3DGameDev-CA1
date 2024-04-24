using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Type4NPC : MonoBehaviour
{
    private Animator anim;
    private AnimatorStateInfo info;

    private bool isInTheFieldOfView;
    
    public int health;
    public int ammo;
    
    public enum State
    {
        Idle,
        Flee,
        Ambush,
        FindHealth,
        BackToStart
    };

    public State npcState;

    public GameObject player;
    public GameObject bullet;

    private float shootCooldown = 3f;
    public Transform shootPoint;

    private NavMeshAgent _agent;
    public GameObject target;

    private float fleeDistance;

    private GameObject[] healthPacks;
    private GameObject[] ammoPacks;

    public GameObject ambushStart;
    public Vector3 startPos;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        fleeDistance = 10f;
        
        target = Instantiate(new GameObject(), this.transform);
        target.name = "Target";
        ambushStart = GameObject.FindWithTag("AmbushStart");
        startPos = transform.position;
        
        npcState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);

        shootCooldown -= Time.deltaTime;
        
        CheckHealth();

        if (Vector3.Distance(transform.position, player.transform.position) < fleeDistance)
        {
            npcState = State.Flee;
        }

        if (info.IsName("Start"))
        {
            npcState = State.BackToStart;
        }
        
        switch (npcState)
        {
            case State.Flee:
            {
                anim.SetTrigger("Fleeing");
                
                if (Vector3.Distance(transform.position, player.transform.position) < fleeDistance)
                {
                    Vector3 fleeDirection = (transform.position - player.transform.position).normalized;
                    Vector3 fleePos = transform.position + fleeDirection * fleeDistance;
                    Debug.Log(fleePos);
                    target.transform.position = fleePos;

                    _agent.SetDestination(target.transform.position);
                }
                
                break;
            }
            case State.Ambush:
            {
                _agent.isStopped = false;
                target = ambushStart;
            
                _agent.SetDestination(target.transform.position);
                _agent.speed = 5.5f;

                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance < 2.5f)
                {
                    anim.SetTrigger("Ambush");
                }
                break;
            }
            case State.FindHealth:
            {
                healthPacks = GameObject.FindGameObjectsWithTag("HealthPack");
                if (healthPacks.Length != 0 && ammo <= 2)
                {
                    target = healthPacks[Random.Range(0, healthPacks.Length)];
                    _agent.SetDestination(target.transform.position);
                }

                break;
            }
            case State.BackToStart:
            {
                _agent.isStopped = false;
                _agent.SetDestination(startPos);

                if (Vector3.Distance(transform.position, startPos) < 1) anim.SetTrigger("BackAtStart");
                break;
            }
        }
    }
    
    public void PlayerAmbush()
    {
        anim.SetTrigger("StartAmbush");
        npcState = State.Ambush;
    }

    void CheckHealth()
    {
        if (health <= 20)
        {
            npcState = State.FindHealth;
        }
    }

    void Shoot()
    {
        if (ammo > 0)
        {
            GameObject bul = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            bul.GetComponent<Rigidbody>().AddForce(transform.forward * 60f, ForceMode.Impulse);
            ammo -= 1;
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
    
    public void AddHealth(int healthToAdd)
    {
        health += healthToAdd;

        if (health > 100)
        {
            health = 100;
        }
    }

    public void AddAmmo(int ammoToAdd)
    {
        ammo += ammoToAdd;

        if (ammo > 25)
        {
            ammo = 25;
        }
    }
}
