using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToNextRoom : MonoBehaviour
{
    
    private bool isRoomClear = true;
    private GameObject player;
    private GameObject playerBoth;
    private GameObject cam;
    private GameObject menu;
    private GameObject healthBar;
    private GameObject mainCam;
    private GameObject DialogTextUI;
    private Scene currentScene;
    public AudioSource BGM1;
    public AudioSource BGM2;
    public AudioSource BGM3;
    private GameObject zhuanchang;
    private float zhuanchangTimer = -20;
    private bool start = false;
    private bool cplt = false;
    private float alpha = 0.9f;
    private float timer;
    void Start()
    {
        player = GameObject.Find ("Player");
        cam = GameObject.Find ("CM vcam1");
        mainCam = GameObject.Find ("Main Camera");
        menu = GameObject.Find ("Canvas");
        healthBar = GameObject.Find ("HealthBar");
        DialogTextUI = GameObject.Find ("DialogTextUI");
        zhuanchang = GameObject.Find ("zhuanchang");
        BGM1 = player.GetComponents<AudioSource>()[1];
        BGM2 = player.GetComponents<AudioSource>()[2];
        BGM3 = player.GetComponents<AudioSource>()[3];
        alpha = 0;
        // playerBoth = GameObject.Find ("PlayerBothPoint");
        // player.transform.position = playerBoth.transform.position;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // zhuanchangTimer = 2f;
            start = true;
        }
        if (alpha < 0.95f && start)
        {
            alpha += 0.04f;
            zhuanchang.GetComponent<Image>().color = new Color(0 , 0 , 0 , alpha);
            if(alpha >= 0.95f && start)
            {
                cplt = true;
                start = false;
            }
        }
        if (cplt)
        {
            StartCoroutine(MovePlayerToNextScene());
            cplt = false;
        }
    }
    IEnumerator MovePlayerToNextScene()
    {
        int nowScene = SceneManager.GetActiveScene().buildIndex;
        int newScene = Random.Range(0,4) + 2;
        int chapter = Globle.getRoom()/3;

        if(chapter >=2 ) chapter = 2;
        newScene = newScene + chapter * 4;

        if (Globle.getRoom() == 9)
        {
            newScene = 14;
        }
        else 
        {
            while(newScene == nowScene)
            {
                newScene = Random.Range(0,4) + 2;
                newScene = newScene + chapter * 4;
            }
        }

        if (Globle.getRoom() == 3)
        {
            BGM1.enabled = false;
            BGM2.enabled = true;
            BGM2.Play();
        }
        else if (Globle.getRoom() == 6)
        {
            BGM2.enabled = false;
            BGM3.enabled = true;
            BGM3.Play();
        }

        currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.MoveGameObjectToScene(player,SceneManager.GetSceneByBuildIndex(newScene));
        SceneManager.MoveGameObjectToScene(cam,SceneManager.GetSceneByBuildIndex(newScene));
        SceneManager.MoveGameObjectToScene(mainCam,SceneManager.GetSceneByBuildIndex(newScene));
        SceneManager.MoveGameObjectToScene(menu,SceneManager.GetSceneByBuildIndex(newScene));
        SceneManager.MoveGameObjectToScene(healthBar,SceneManager.GetSceneByBuildIndex(newScene));
        SceneManager.MoveGameObjectToScene(DialogTextUI,SceneManager.GetSceneByBuildIndex(newScene));
        Globle.goRoom();
        Globle.toNextRoom();
        SceneManager.UnloadSceneAsync(currentScene);

    }

}
