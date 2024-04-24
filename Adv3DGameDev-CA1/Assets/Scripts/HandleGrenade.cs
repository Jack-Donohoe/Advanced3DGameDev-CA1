using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGrenade : StateMachineBehaviour
{
    public GameObject grenade;
    private GameObject clone;

    private Rigidbody r;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
        clone = Instantiate(grenade, new Vector3(animator.rootPosition.x, 2, animator.rootPosition.z), Quaternion.identity);

        r = clone.GetComponent<Rigidbody>();
        r.AddForce(animator.gameObject.transform.forward * 2000);
    }
}
