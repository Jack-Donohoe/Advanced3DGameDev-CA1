using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Type1NPC : MonoBehaviour
{
    public GameObject[] waypoints;
    private int waypointCount = 0;
    
    private Animator anim;
    private AnimatorStateInfo info;

    private bool isInTheFieldOfView;
    
    public int health;
    public int ammo;
    
    public enum State
    {
        Path,
        ChasePlayer
    };

    public State npcState;

    public GameObject player;
    public GameObject bullet;

    private float shootCooldown = 3f;
    public Transform shootPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        health = 50;
        ammo = 5;
    }

    // Update is called once per frame
    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        npcState = State.Path;

        shootCooldown -= Time.deltaTime;
        
        NewLook();
        Listen();
        Smell();
        
        switch (npcState)
        {
            case State.Path:
            {
                GameObject target = waypoints[waypointCount];
                
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
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                
                if (!isInTheFieldOfView)
                {
                    anim.SetBool("SeePlayer", false);
                }

                if (shootCooldown < 0 && Vector3.Distance(transform.position, player.transform.position) < 25f)
                {
                    Shoot();
                    shootCooldown = 3f;
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
