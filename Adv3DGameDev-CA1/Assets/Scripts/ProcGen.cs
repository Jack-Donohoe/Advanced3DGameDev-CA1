using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProcGen : MonoBehaviour
{
    public GameObject wall, NPC_Type1, NPC_Type2, healthPack, ammoPack;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void GenerateFromFile()
    {
        TextAsset file = (TextAsset) Resources.Load("maze", typeof(TextAsset));
        string text = file.text;

        text = text.Replace(System.Environment.NewLine, "");
        for (int i = 0; i < text.Length; i++)
        {
            int column = i % 10;
            int row = i / 10;

            if (text[i] == '1')
            {
                GameObject wall = Instantiate(this.wall, new Vector3(45 - column * 10, 1f, 45 - row * 10), Quaternion.identity, this.transform);
            } else if (text[i] == '2')
            {
                GameObject NPC = Instantiate(NPC_Type1, new Vector3(45 - column * 10, 1.5f, 45 - row * 10), Quaternion.identity);
            } else if (text[i] == '3')
            {
                GameObject NPC = Instantiate(NPC_Type2, new Vector3(45 - column * 10, 1.5f, 45 - row * 10),
                    Quaternion.identity);
            } else if (text[i] == '8')
            {
                GameObject healthPack = Instantiate(this.healthPack, new Vector3(45 - column * 10, 1.5f, 45 - row * 10), Quaternion.identity);
                healthPack.name = "healthPack";
            } else if (text[i] == '9')
            {
                GameObject ammoPack = Instantiate(this.ammoPack, new Vector3(45 - column * 10, 1.5f, 45 - row * 10), Quaternion.identity);
                ammoPack.name = "ammoPack";
            }
        }
    }
}
