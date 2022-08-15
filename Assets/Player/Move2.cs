using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2: MonoBehaviour
{
    float speedMove = 10;
    float rotateSpeed = 0;
    float force = 0;
    float speedJump = 0;
    float jump = 10;
    float jumpc = 2;
    Vector2 jingzhi = new Vector2(0, 0);
    bool isGround;
    BoxCollider2D jumpcllinder;
    Rigidbody2D rig;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        isGround = Physics2D.OverlapBox(jumpcllinder.offset, jumpcllinder.size, 0f, LayerMask.GetMask("Ground"));
    }
    void Start()
    {
        jumpcllinder = transform.GetComponent<BoxCollider2D>();
        rig = transform.GetComponent<Rigidbody2D>();
        Debug.Log(jumpcllinder.offset);
        Debug.Log(jumpcllinder.size);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float j = Input.GetAxis("Jump");
        float sp = 0;
        Debug.Log(isGround);
        if (isGround)
        {
            jumpc = 2;
        }
        if(h!=0)
        {
            transform.Translate(h * Vector2.right * speedMove * Time.deltaTime, Space.Self);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpc > 0)
        {
            Vector2 jp = new Vector2(0, 10);
            rig.velocity = jp;
            jumpc--;
            Debug.Log("跳了但没完全跳。");
            Debug.Log(jumpc);
        }



        //if (j!=0)
        //{
            //if (GetComponent<Rigidbody>().velocity != jingzhi)
            //{
                //transform.Translate(j * Vector3.up * speedJump * Time.deltaTime, Space.Self);
                //rig.AddForce(new Vector3( 0 , j * force , 0));
                //GetComponent<Rigidbody>().velocity = jp;
                //GetComponent<Rigidbody>().velocity;
            //}



            //if (transform.position.y == 0)
            //{
            //    GetComponent<Rigidbody>().velocity = jp;
            //}
        //}
    }
}
