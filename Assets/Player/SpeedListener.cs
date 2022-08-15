using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedListener : MonoBehaviour
{
    private float YSpeed;
    private float XSpeed;
    private Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // if(rig.velocity.y - YSpeed > 8 && gameObject.GetComponent<AirCondition>().getIsRolling())
        // {
        //     Debug.Log("Y Duang~");
        // }
        // if(rig.velocity.x - YSpeed > 8 && gameObject.GetComponent<AirCondition>().getIsRolling())
        // {
        //     Debug.Log("X Duang~");
        // }
        YSpeed = rig.velocity.y;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") && gameObject.GetComponent<AirCondition>().getIsRolling() && YSpeed > 8)
        {
            Debug.Log(YSpeed);
        }
    }
    
}
