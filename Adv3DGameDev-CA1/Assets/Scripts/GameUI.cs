using System;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI pickupText;
    public TextMeshProUGUI timer;

    public GameObject dataMenu;

    private Player player;

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        healthText.text = "Health: " + player.playerHealth;
        ammoText.text = "Ammo: " + player.playerAmmo;
        pickupText.text = "Pickups: " + player.pickupCount;
        timer.text = "Time: " + (180 - Mathf.FloorToInt(player.timer % 60));
    }

    private void Update()
    {
        healthText.text = "Health: " + player.playerHealth;
        ammoText.text = "Ammo: " + player.playerAmmo;
        pickupText.text = "Pickups: " + player.pickupCount;
        timer.text = "Time: " + (180 - Mathf.FloorToInt(player.timer));

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleDataMenu();
        }
    }

    public void OnSaveButton()
    {
        DataManager.instance.SaveDataJSON();
    }

    public void OnLoadButton()
    {
        DataManager.instance.LoadDataJSON();
    }

    void ToggleDataMenu()
    {
        if (!dataMenu.activeSelf)
        {
            dataMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            player.gameObject.GetComponent<PlayerInput>().enabled = false;
        }
        else
        {
            dataMenu.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player.gameObject.GetComponent<PlayerInput>().enabled = true;
        }
    }
}
