using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioSource mouseDown;
    public AudioMixer audioMixer;
    void Start()
    {
        mouseDown = GetComponent<AudioSource>();
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        mouseDown.Play();
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        mouseDown.Play();
    }
    public void ReturnGame()
    {
        SceneManager.LoadScene(0);
        mouseDown.Play();
    }
    public void setVolume(float value)
    {
        audioMixer.SetFloat("Mainvolume", value);
    }
    
}
