using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class dialogueBubble : MonoBehaviour
{
    [Header("这个角色/地点的目标对话行数")]
    public int targetTextLine = 1;
    private bool isRoomClear = true;
    private GameObject player;
    private GameObject playerBoth;
    private GameObject cam;
    private GameObject mainCam;
    private bool isTalk;
    private GameObject dialogTextUI;
    private GameObject dialogTextPanel;
    void Start()
    {
        dialogTextUI = GameObject.Find("DialogTextUI");
        dialogTextPanel = dialogTextUI.transform.GetChild(0).gameObject;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ShowDialogSystem());
        }
    }
    
    IEnumerator ShowDialogSystem()
    {
        isTalk = true;
        dialogTextPanel.SetActive(true);
        yield return null;
        dialogTextPanel.GetComponent<dialogSystem>().setNowTextLine(targetTextLine - 1);
        gameObject.SetActive(false);
    }
    public void endTalk()
    {
        isTalk = false;
        dialogTextPanel.SetActive(false);
    }

    public bool getIsTalk()
    {
        return isTalk;
    }

}
