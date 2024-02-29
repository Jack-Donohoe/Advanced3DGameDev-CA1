using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;
    CinemachineVirtualCamera camera;

    private void Start()
    {
        camera = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    void OnShoot()
    {
        GameObject bul = Instantiate(bullet, transform.position, Quaternion.identity);
        bul.GetComponent<Rigidbody>().AddForce(camera.transform.forward * 100);
    }
}
