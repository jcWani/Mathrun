using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charSelector : MonoBehaviour
{
    public int currentCharIndex;
    public GameObject[] chars;


    // Start is called before the first frame update
    void Start()
    {
        currentCharIndex = PlayerPrefs.GetInt("selectedChar", 0);

        foreach (GameObject chara in chars)
            chara.SetActive(false);

        chars[currentCharIndex].SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
