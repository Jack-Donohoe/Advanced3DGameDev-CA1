using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerHealth;
    public int playerAmmo;
    public int pickupCount;
    
    public GameObject bullet;
    public CinemachineVirtualCamera camera;
    public Transform shootPoint;

    private void Start()
    {
        playerHealth = 100;
        playerAmmo = 10;
        pickupCount = 0;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
    }

    public void AddHealth(int healthToAdd)
    {
        playerHealth += healthToAdd;

        if (playerHealth > 100)
        {
            playerHealth = 100;
        }
    }

    public void AddAmmo(int ammoToAdd)
    {
        playerAmmo += ammoToAdd;

        if (playerAmmo > 25)
        {
            playerAmmo = 25;
        }
    }

    void OnShoot()
    {
        if (playerAmmo > 0)
        {
            GameObject bul = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            bul.GetComponent<Rigidbody>().AddForce(camera.transform.forward * 60f, ForceMode.Impulse);
            playerAmmo -= 1;
        }
    }
}
