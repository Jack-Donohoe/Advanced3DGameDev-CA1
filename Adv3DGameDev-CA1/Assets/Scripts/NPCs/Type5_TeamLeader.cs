using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type5_TeamLeader : MonoBehaviour
{
    private GameObject[] teamMembers;
    
    // Start is called before the first frame update
    void Start()
    {
        teamMembers = GameObject.FindGameObjectsWithTag("Type5");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TeamAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            TeamRetreat();
        }
    }

    void TeamAttack()
    {
        GameObject[] allTargets;

        allTargets = GameObject.FindGameObjectsWithTag("Type6");
        for (int i = 0; i < teamMembers.Length; i++)
        {
            teamMembers[i].GetComponent<Follower>().Attack(allTargets[i]);
        }
    }

    void TeamRetreat()
    {
        foreach (var teamMember in teamMembers)
        {
            teamMember.GetComponent<Follower>().Retreat();
        }
    }
}
