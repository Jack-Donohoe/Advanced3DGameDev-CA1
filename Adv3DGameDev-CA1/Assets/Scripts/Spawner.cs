using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float minX, maxX, minZ, maxZ;

    public GameObject healthPack, ammoPack;

    public GameObject[] npcsToSpawn;

    private float timer;

    private float spawnInterval = 15;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnInterval)
        {
            timer = 0;
            spawnInterval -= 5;
            if (spawnInterval < 0)
            {
                spawnInterval = 0;
            }
            
            foreach (GameObject npc in npcsToSpawn)
            {
                if (GameObject.FindGameObjectsWithTag(npc.tag).Length == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Instantiate(npc, new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minZ, maxZ)),
                            Quaternion.identity);
                    }
                }
            }

            if (GameObject.FindGameObjectsWithTag(healthPack.tag).Length == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Instantiate(healthPack, new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minZ, maxZ)),
                        Quaternion.identity);
                }
            }

            if (GameObject.FindGameObjectsWithTag(ammoPack.tag).Length == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Instantiate(ammoPack, new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minZ, maxZ)),
                        Quaternion.identity);
                }
            }
        }
    }
}
