using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame update

    public float cmoveRateX;
    public float cmoveRateY;
    private float startPointX;
    private float startPointY;
    
    private float camStartPointX;
    private float camStartPointY;
    private float mainCamY;
    public GameObject cam;
    
    public GameObject mainCam;

    void Start()
    {
        // cam = GameObject.Find("CM vcam1");
        // mainCam = GameObject.Find("Main Camera");
        cam = GameObject.Find("Main Camera (1)");
        startPointX = transform.position.x;
        startPointY = transform.position.y;
        camStartPointX = cam.transform.position.x;
        camStartPointY = cam.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // if(mainCam.transform.position.y - cam.transform.position.y < 1)
        // {
        //     //cmoveRateY = 0;
        //     transform.position = new Vector2(startPointX + (cam.transform.position.x-camStartPointX) * cmoveRateX , startPointY + (cam.transform.position.y-camStartPointY) * cmoveRateY);
        // }
        transform.position = new Vector2(startPointX + (cam.transform.position.x-camStartPointX) * cmoveRateX , startPointY + (cam.transform.position.y-camStartPointY) * cmoveRateY);

    }
}
