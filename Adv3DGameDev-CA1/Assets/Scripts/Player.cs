using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerHealth;
    public int playerAmmo;
    public int pickupCount;
    
    public GameObject bullet;
    public CinemachineVirtualCamera camera;
    public Transform shootPoint;

    public float timer = 0f;

    private void Start()
    {
        playerHealth = DataManager.instance.playerHealth;
        playerAmmo = DataManager.instance.playerAmmo;
        pickupCount = DataManager.instance.pickupCount;
        timer = DataManager.instance.timer;
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.transform.position = DataManager.instance.playerPos;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (pickupCount == 10)
        {
            DataManager.instance.LoadNextLevel();
        }

        if (playerHealth <= 0 || timer >= 180f)
        {
            DataManager.instance.LoseGame();
        }
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

    public void SetPlayerParams(int health, int ammo, int pickups, Vector3 position)
    {
        playerHealth = health;
        playerAmmo = ammo;
        pickupCount = pickups;
        gameObject.transform.position = position;
    }
}
