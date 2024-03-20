using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            Debug.Log("Starting Ambush");
            GameObject[] snipers = GameObject.FindGameObjectsWithTag("Type4");

            foreach (GameObject npc in snipers)
            {
                npc.GetComponent<Type4NPC>().PlayerAmbush();
            }
        }
    }
}
