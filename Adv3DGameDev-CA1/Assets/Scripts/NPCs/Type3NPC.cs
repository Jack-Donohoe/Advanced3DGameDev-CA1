using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Type3NPC : MonoBehaviour
{
    private Animator anim;
    private AnimatorStateInfo info;

    private bool isInTheFieldOfView;
    
    public int health;
    public int ammo;
    
    public enum State
    {
        ChasePlayer,
        FindHealth,
        FindAmmo
    };

    public State npcState;

    public GameObject player;
    public GameObject bullet;

    private float shootCooldown = 3f;
    public Transform shootPoint;

    private NavMeshAgent _agent;
    public GameObject fleeTarget;

    private float fleeDistance;

    private GameObject[] healthPacks;
    private GameObject[] ammoPacks;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        fleeDistance = 25f;
        fleeTarget = Instantiate(new GameObject(), this.transform);
        fleeTarget.name = "Flee Target";
    }

    // Update is called once per frame
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        npcState = State.ChasePlayer;

        shootCooldown -= Time.deltaTime;
        
        CheckHealth();
        CheckAmmo();
        
        switch (npcState)
        {
            case State.ChasePlayer:
            {
                _agent.SetDestination(player.transform.position);
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                
                if (!isInTheFieldOfView)
                {
                    anim.SetBool("SeePlayer", false);
                }

                if (shootCooldown < 0 && Vector3.Distance(transform.position, player.transform.position) < fleeDistance)
                {
                    Shoot();
                    shootCooldown = 3f;
                }

                break;
            }
            case State.FindHealth:
            {
                healthPacks = GameObject.FindGameObjectsWithTag("HealthPack");
                if (healthPacks.Length != 0 && ammo <= 2)
                {
                    GameObject target = healthPacks[Random.Range(0, healthPacks.Length)];
                    _agent.SetDestination(target.transform.position);
                }
                else
                {
                    Flee();
                }

                break;
            }
            case State.FindAmmo:
            {
                ammoPacks = GameObject.FindGameObjectsWithTag("AmmoPack");
                if (ammoPacks.Length != 0)
                {
                    GameObject target = ammoPacks[Random.Range(0, ammoPacks.Length)];
                    _agent.SetDestination(target.transform.position);
                }

                break;
            }
        }
    }

    void CheckHealth()
    {
        if (health <= 20)
        {
            npcState = State.FindHealth;
        }
    }

    void CheckAmmo()
    {
        if (ammo <= 2)
        {
            npcState = State.FindAmmo;
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

    void Flee()
    {
        Debug.Log(Vector3.Distance(transform.position, player.transform.position));
        if (Vector3.Distance(transform.position, player.transform.position) < fleeDistance)
        {
            Vector3 fleeDirection = (transform.position - player.transform.position).normalized;
            Vector3 fleePos = transform.position + fleeDirection * fleeDistance;
            Debug.Log(fleePos);
            fleeTarget.transform.position = fleePos;

            _agent.SetDestination(fleeTarget.transform.position);
        }
    }

    public void Hit()
    {
        anim.SetBool("Hit", true);
        npcState = State.ChasePlayer;
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
