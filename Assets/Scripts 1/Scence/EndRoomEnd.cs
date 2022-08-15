using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomEnd : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject endMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        endMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
