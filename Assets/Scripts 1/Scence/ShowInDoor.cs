using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowInDoor : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject enterTag;
    private GameObject[] allObj;
    private int enemyCount;
    private Scene currentScene;
    private float timer = 0;
    private float banTimer = 1;
    new private Collider2D collider2D;
    void Start()
    {
        banTimer = 1;
        enterTag = this.gameObject.transform.GetChild(0).gameObject;
        currentScene = SceneManager.GetActiveScene();
        allObj = currentScene.GetRootGameObjects();
        Globle.enemyReturn();
        countEnemy();
        collider2D = transform.GetComponent<BoxCollider2D>();
        // StartCoroutine(EnemyCount());
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1 && banTimer <= 0)
        {
            Globle.enemyReturn();
            countEnemy();
            if(Globle.getEnemy() == 0 && collider2D.IsTouchingLayers(LayerMask.GetMask("Player")))
            {
                enterTag.SetActive(true);
            }
            timer = 0;
        }
        if (banTimer > 0)
        {
            banTimer -=Time.deltaTime;
        }
    }
    void countEnemy()
    {
        currentScene = SceneManager.GetActiveScene();
        allObj = currentScene.GetRootGameObjects();
        foreach(GameObject obj in allObj)
        {
            if(obj.CompareTag("Enemy"))
            {
                Globle.enemyAdd();
            }
            if(obj.CompareTag("Boss"))
            {
                Globle.enemyAdd();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        countEnemy();
        if(other.CompareTag("Player") && Globle.getEnemy() == 0 && banTimer <= 0)
        {
            enterTag.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && banTimer <= 0)
        {
            enterTag.SetActive(false);
        }
    }

    // void OnTriggerStay2D(Collider2D other)
    // {
    //     if(other.CompareTag("Player") && Globle.getEnemy() == 0)
    //     {
    //         enterTag.SetActive(true);
    //     }
    //     else
    //     {
    //         enterTag.SetActive(false);
    //     }
    // }



    // IEnumerator EnemyCount()
    // {
    //     while(true)
    //     {
    //         Globle.enemyReturn();
    //         currentScene = SceneManager.GetActiveScene();
    //         allObj = currentScene.GetRootGameObjects();
    //         foreach(GameObject obj in allObj)
    //         {
    //             if(obj.CompareTag("Enemy"))
    //             {
    //                 Globle.enemyAdd();
    //             }
    //             if(obj.CompareTag("Boss"))
    //             {
    //                 Globle.enemyAdd();
    //             }
    //         }
    //         if(Globle.getEnemy() == 0 && collider2D.IsTouchingLayers(LayerMask.GetMask("Player")))
    //         {
    //             enterTag.SetActive(true);
    //         }
    //         timer = 0;
    //         yield return new WaitForSeconds(1f);
    //     }
    // }
    
}
