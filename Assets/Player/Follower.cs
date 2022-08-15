using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    static int followerCount = 0;
    private GameObject target;
    private float selfSpeed = 0;
    private float YMAX = 1.8f;
    private float Xr = 3;
    private float Yr = 1.8f;
    private float juli;
    private float SELFX;
    private float SELFY;
    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (target)
        {
            SELFX = transform.position.x;
            SELFY = transform.position.y;
            juli = Vector2.Distance(transform.position,target.transform.position) - 1.5f;

            if (juli <= 0)
            {
                juli = 0;
            }

            selfSpeed = juli * 5;

            transform.position = Vector2.MoveTowards(transform.position,
                target.transform.position, selfSpeed * Time.deltaTime);
        }

    }

    public void setTarget(GameObject target)
    {
        this.target = target;
        Debug.Log(target);
        followerCount++;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
