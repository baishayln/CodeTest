using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource mouseDown;
    void Start()
    {
        mouseDown = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        mouseDown.Play();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        mouseDown.Play();
    }
    public void QuitGame()
    {
        Application.Quit();
        mouseDown.Play();
    }
    
}
