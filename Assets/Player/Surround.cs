using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surround : MonoBehaviour
{
    private Transform target;
    private float distance;
    private float angleSpeed;
    private float startAngle;
    private float angle;
    private Vector2 localPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //每一帧根据角速度进行，并且转向target
        if (transform.parent != null)
        {
            angle += angleSpeed * Time.deltaTime;
            rotateSelf(angle);
            if (transform.parent.localScale.x < 0)
            {
                transform.up = new Vector2( -transform.localPosition.x , transform.localPosition.y );
            }
            else
            {
                transform.up = transform.localPosition;
            }
        }
    }

    public void setSurround(float angleSpd , float sttAngle , Transform tgt , float dsts)
    {
        angleSpeed = angleSpd;
        distance = dsts;
        angle = sttAngle;
        target = tgt;
        // if (transform.parent == null)
        // {
        //     transform.SetParent(target);
        // }
    }

    private void rotateSelf(float angle)
    {
        if (transform.parent.localScale.x < 0)
        {
            transform.localPosition = new Vector2(-distance * Mathf.Sin(angle) , distance * Mathf.Cos(angle));
        }
        else
        {
            transform.localPosition = new Vector2(distance * Mathf.Sin(angle) , distance * Mathf.Cos(angle));
        }
        // if (transform.parent.localScale.x < 0)
        // {
        //     transform.localScale = new Vector3(transform.parent.localScale.x, 1, 1);
        // }
    }

    public void setDistance(float dsts)
    {
        distance = dsts;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        target = other.transform;
    }

}
