using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().AddHealth(10);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Type2"))
        {
            other.gameObject.GetComponent<Type2NPC>().AddHealth(10);
            Destroy(gameObject);
        }
    }
}
