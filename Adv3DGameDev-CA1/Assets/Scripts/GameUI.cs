using System;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI pickupText;

    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        healthText.text = "Health: " + player.playerHealth;
        ammoText.text = "Ammo: " + player.playerAmmo;
        pickupText.text = "Pickups: " + player.pickupCount;
    }

    private void Update()
    {
        healthText.text = "Health: " + player.playerHealth;
        ammoText.text = "Ammo: " + player.playerAmmo;
        pickupText.text = "Pickups: " + player.pickupCount;
    }
}
