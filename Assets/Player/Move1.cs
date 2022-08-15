using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move1: MonoBehaviour
{
    float speedMove = 10;
    float rotateSpeed = 0;
    float force = 0;
    float speedJump = 0;
    float jump = 10;
    float jumpc = 1;
    Animator animator;
    Vector2 jingzhi = new Vector2(0, 0);
    BoxCollider2D jumpcllinder;
    Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        jumpcllinder = transform.GetComponents<BoxCollider2D>()[1];
        rig = transform.GetComponent<Rigidbody2D>();
        animator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float face = Input.GetAxisRaw("Horizontal");
        float j = Input.GetAxisRaw("Jump");


        if (jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            jumpc = 1;
            animator.SetBool("falling",false);
            animator.SetBool("idle",true);
        }
        if(h!=0)
        {
            rig.velocity = new Vector2(h * speedMove, rig.velocity.y);
            animator.SetFloat("running",Mathf.Abs(h));
        }
        if(face != 0)
        {
            transform.localScale = new Vector3( face , 1 , 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpc > 0)
        {
            Vector2 jp = new Vector2(0, 10);
            rig.velocity = jp;
            jumpc--;
            animator.SetBool("jumping",true);
            animator.SetBool("idle",false);
            animator.SetBool("falling",false);
        }

        if(rig.velocity.y < 0)
        {
            animator.SetBool("jumping",false);
            animator.SetBool("falling",true);
            animator.SetBool("idle",false);
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
