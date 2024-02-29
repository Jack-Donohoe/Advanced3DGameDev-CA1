using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProcGen : MonoBehaviour
{
    public GameObject wall, target, NPC;
    
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
                GameObject NPC = Instantiate(this.NPC, new Vector3(45 - column * 10, 1.5f, 45 - row * 10), Quaternion.identity);
            } else if (text[i] == '3')
            {
                //GameObject target = Instantiate(this.target, new Vector3(45 - column * 10, 1.5f, 45 - row * 10), Quaternion.identity);
                //target.name = "Target";
            }
        }
    }
}
