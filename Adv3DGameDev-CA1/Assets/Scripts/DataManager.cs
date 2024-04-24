using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    private class DataToSave
    {
        public int health;
        public int ammo;
        public int pickups;
        public float timer;
        public Vector3 position;
        public string lastLevel;
    }

    private Player player;

    public int playerHealth;
    public int playerAmmo;
    public int pickupCount;
    public float timer;
    public Vector3 playerPos;
    
    public static DataManager instance { private set; get; }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetPlayer();
    }

    public void SaveDataJSON()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        DataToSave saveData = new DataToSave{health = player.playerHealth, ammo = player.playerAmmo, pickups = player.pickupCount, timer = player.timer, position = player.gameObject.transform.position, lastLevel = SceneManager.GetActiveScene().name};
        string jsonText = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.dataPath + "/game.json", jsonText);
    }

    public void LoadDataJSON()
    {
        string dataToRead = File.ReadAllText(Application.dataPath + "/game.json");
        DataToSave savedData = JsonUtility.FromJson<DataToSave>(dataToRead);
        
        SceneManager.LoadScene(savedData.lastLevel);
        playerHealth = savedData.health;
        playerAmmo = savedData.ammo;
        pickupCount = savedData.pickups;
        timer = savedData.timer;
        playerPos = savedData.position;
    }

    public void LoseGame()
    {
        ResetPlayer();
        SceneManager.LoadScene("Level1");
    }
    
    public void LoadNextLevel()
    {
        ResetPlayer();
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            SceneManager.LoadScene("Level2");
        }
        else
        {
            SceneManager.LoadScene("Level1");
        }
    }

    void ResetPlayer()
    {
        playerHealth = 100;
        playerAmmo = 25;
        pickupCount = 0;
        timer = 0f;
        playerPos = new Vector3(0f, 1.5f, 0f);
    }
}
