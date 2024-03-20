using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().AddAmmo(5);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Type2"))
        {
            other.gameObject.GetComponent<Type2NPC>().AddAmmo(5);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Type3"))
        {
            other.gameObject.GetComponent<Type3NPC>().AddAmmo(5);
            Destroy(gameObject);
        }
    }
}
