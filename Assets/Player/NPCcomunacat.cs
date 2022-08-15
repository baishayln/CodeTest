using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCcomunacat : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject dialogueBubble;
    private int enemyCount;
    private float timer;
    new private Collider2D collider2D;
    void Start()
    {
        dialogueBubble = this.gameObject.transform.GetChild(0).gameObject;
        Globle.enemyReturn();
        collider2D = transform.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1)
        {
            if(Globle.getEnemy() == 0 && collider2D.IsTouchingLayers(LayerMask.GetMask("Player")) && !dialogueBubble.GetComponent<dialogueBubble>().getIsTalk())
            {
                dialogueBubble.SetActive(true);
            }
            timer = 0;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && Globle.getEnemy() == 0)
        {
            dialogueBubble.SetActive(true);
        }
    }
    // void OnTriggerStay2D(Collider2D other)
    // {
    //     if(other.CompareTag("Player") && Globle.getEnemy() == 0 && dialogueBubble.activeSelf)
    //     {
    //         dialogueBubble.SetActive(true);
    //     }
    // }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            dialogueBubble.SetActive(false);
        }
        if(dialogueBubble.GetComponent<dialogueBubble>().getIsTalk())
        {
            dialogueBubble.GetComponent<dialogueBubble>().endTalk();
        }
    }

}
