using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    public AudioSource mouseDown;
    public Text roomNumber;
    void Start()
    {
        roomNumber.text = Globle.getRoom().ToString();
        mouseDown = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReturnGame()
    {
        SceneManager.LoadScene(0);
        mouseDown.Play();
    }
}
