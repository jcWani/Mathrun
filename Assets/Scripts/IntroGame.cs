using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroGame : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string sceneName1;
    [SerializeField] Slider volumeSlider;

    public AudioSource intro;
    // Start is called before the first frame update
    void Start()
    {
        intro.Play();

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Settings()
    {
        

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void storeButton()
    {
        SceneManager.LoadScene(sceneName1);
    }

    public void PlusVolume()
    {
        volumeSlider.value += 0.1f;
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    public void MinusVolume()
    {
        volumeSlider.value -= 0.1f;
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
