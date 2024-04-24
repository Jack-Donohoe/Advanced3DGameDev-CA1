using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Type2NPC : MonoBehaviour
{
    private int waypointCount;
    public GameObject[] waypoints;
    
    private Animator anim;
    private AnimatorStateInfo info;

    private bool isInTheFieldOfView;
    
    public int health;
    public int ammo;
    
    public enum State
    {
        Path,
        ChasePlayer,
        FindHealth,
        FindAmmo
    };

    public State npcState;

    public GameObject player;
    public GameObject bullet;

    private float shootCooldown = 3f;
    public Transform shootPoint;

    private bool hit;

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
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        waypointCount = Random.Range(0, waypoints.Length);
        fleeDistance = 25f;
        fleeTarget = Instantiate(new GameObject(), this.transform);
        fleeTarget.name = "Flee Target";
    }

    // Update is called once per frame
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        if (!hit)
        {
            npcState = State.Path;
        }

        shootCooldown -= Time.deltaTime;
        
        NewLook();
        Listen();
        Smell();
        CheckHealth();
        CheckAmmo();
        
        switch (npcState)
        {
            case State.Path:
            {
                GameObject target = waypoints[waypointCount];
                
                if (Vector3.Distance(transform.position, target.transform.position) < 1)
                {
                    waypointCount = Random.Range(0, waypoints.Length);
                    target = waypoints[waypointCount];
                }
                
                _agent.SetDestination(target.transform.position);
                break;
            }
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
                hit = false;
                healthPacks = GameObject.FindGameObjectsWithTag("HealthPack");
                if (healthPacks.Length != 0)
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
                hit = false;
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
        hit = true;
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
