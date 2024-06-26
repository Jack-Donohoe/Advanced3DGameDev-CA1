using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5f)
        {
            timer = 0f;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(10);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Type1"))
        {
            other.gameObject.GetComponent<Type1NPC>().TakeDamage(10);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Type2"))
        {
            other.gameObject.GetComponent<Type2NPC>().TakeDamage(10);
            other.gameObject.GetComponent<Type2NPC>().Hit();
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Type3"))
        {
            other.gameObject.GetComponent<Type3NPC>().TakeDamage(10);
            other.gameObject.GetComponent<Type3NPC>().Hit();
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Type4"))
        {
            other.gameObject.GetComponent<Type4NPC>().TakeDamage(10);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Type6"))
        {
            other.gameObject.GetComponent<Follower>().TakeDamage(GameObject.FindGameObjectWithTag("Player"),10);
            Destroy(gameObject);
        } else if (other.gameObject.CompareTag("MobLeader"))
        {
            other.gameObject.GetComponent<Type6_MobLeader>().TakeDamage(10);
            Destroy(gameObject);
        }
    }
}
