using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class shopManager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string sceneName1;
    public AudioSource intro;
    public AudioSource buySound;
    public int currentCharIndex;
    public GameObject[] charModels;

    public charBluePrint[] characters;
    public Button buyButton;


    public Text totalCoinsText;
    // Start is called before the first frame update
    void Start()
    {
        intro.Play();

        foreach(charBluePrint character in characters)
        {
            if(character.price == 0)
            {
                character.isUnlock = true;
            }
            else
            {
                character.isUnlock = PlayerPrefs.GetInt(character.name, 0) == 0 ? false : true;
            }
        }

        currentCharIndex = PlayerPrefs.GetInt("selectedChar", 0);

        foreach (GameObject character in charModels)
            character.SetActive(false);

        charModels[currentCharIndex].SetActive(true);

        totalCoinsText.text = PlayerPrefs.GetInt("totalCoins").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        totalCoinsText.text = PlayerPrefs.GetInt("totalCoins").ToString();
        UpdateUI();
    }

    public void ChangeNext()
    {
        charModels[currentCharIndex].SetActive(false);

        currentCharIndex++;
        if(currentCharIndex == charModels.Length)
        {
            currentCharIndex = 0;
        }

        charModels[currentCharIndex].SetActive(true);
        charBluePrint c = characters[currentCharIndex];
        if (!c.isUnlock)
        {
            return;
        }

        PlayerPrefs.SetInt("selectedChar", currentCharIndex);
    }

    public void ChangePrevious()
    {
        charModels[currentCharIndex].SetActive(false);

        currentCharIndex--;
        if (currentCharIndex == -1)
        {
            currentCharIndex = charModels.Length - 1;
        }

        charModels[currentCharIndex].SetActive(true);

        charBluePrint c = characters[currentCharIndex];
        if (!c.isUnlock)
        {
            return;
        }

        PlayerPrefs.SetInt("selectedChar", currentCharIndex);
    }

    public void startBtn()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void homeBtn()
    {
        SceneManager.LoadScene(sceneName1);
    }

    public void UnlockChar()
    {
        buySound.Play();
        charBluePrint c = characters[currentCharIndex];
        PlayerPrefs.SetInt(c.name, 1);
        PlayerPrefs.SetInt("selectedChar", currentCharIndex);
        c.isUnlock = true;
        PlayerPrefs.SetInt("totalCoins", PlayerPrefs.GetInt("totalCoins") - c.price);
    }

    public void UpdateUI()
    {
        charBluePrint c = characters[currentCharIndex];
        if (c.isUnlock)
        {
            buyButton.gameObject.SetActive(false);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy-" + c.price;
            if(PlayerPrefs.GetInt("totalCoins", 0) >= c.price)
            {
                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }
        }
    }
}
