using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class dialogSystem : MonoBehaviour
{
    [Header("UI组件")]
    public Text text;
    public Text characterName;

    [Header("Text文件")]
    public TextAsset textFile;

    List<string> textList = new List<string>();
    private int nowTextLine;
    void Awake()
    {
        text = transform.GetChild(0).GetComponent<Text>();
        characterName = transform.GetChild(1).GetComponent<Text>();
    }
    void Start()
    {
        getLineFromFile(textFile);
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(textList[nowTextLine][0] == '[')
            {
                gameObject.SetActive(false);
                return;
            }
            nextDialog();
        }
    }

    // public void startDialog()
    // {
    //     characterName.text = textList[nowTextLine];
    //     text.text = textList[nowTextLine + 1];
    //     nowTextLine += 2;
    // }
    public void nextDialog()
    {
        characterName.text = textList[nowTextLine];
        text.text = textList[nowTextLine + 1];
        nowTextLine += 2;
    }
    public void setNowTextLine(int targetTextLine)
    {
        nowTextLine = targetTextLine;
        nextDialog();
    }
    // public void getDialogBubble(dialogueBubble Bubble)
    // {
    //     nowTextLine = Bubble;
    //     nextDialog();
    // }
    void getNowTextLine()
    {
        
    }
    void getLineFromFile(TextAsset textFile)
    {
        textList.Clear();
        
        var lineDate = textFile.text.Split('\n');

        foreach(var line in lineDate)
        {
            textList.Add(line);
        }
    }

}
