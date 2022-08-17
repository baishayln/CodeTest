using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurronderController : MonoBehaviour
{
    // Start is called before the first frame update
    private int surrounderCount = 0;
    private float surrounderAngle;
    private float surrounderDistance = 3;
    private GameObject[] surrounders = new GameObject[10];
    private int surrounderUpLimit = 0;
    public GameObject surrounder;
    public GameObject target;
    private bool isScaleChange;
    private Vector3 targetLastLocalScale = Vector3.one;
    private GameObject surrounderPool;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            target = GameObject.Find("Player (1)");
        }

        transform.position = target.transform.position;

        if (Input.GetKeyDown(KeyCode.O))
        {
            addSurrounder();
        }

        if(target.transform.localScale != targetLastLocalScale && !(target.transform.localScale.x == -targetLastLocalScale.x && target.transform.localScale.y == -targetLastLocalScale.y && target.transform.localScale.z == -targetLastLocalScale.z))
        {
            setDistance();
        }
        targetLastLocalScale = target.transform.localScale;

    }
    public void setCount(int count)
    {
        surrounderCount = count;
        setAngle(surrounderCount);
    }
    private void setAngle(float count)
    {
        surrounderAngle = 6.283f/count;
    }
    private void addSurrounder()
    {
        setAngle(surrounderCount + 1);
        GameObject newsurrounder = GameObject.Instantiate(surrounder , transform.position , Quaternion.identity);
        newsurrounder.transform.SetParent(transform);
        surrounders[surrounderCount] = newsurrounder;
        countRotation();
        surrounderCount++;
    }
    private void addSurrounders(int num)
    {

    }
    private void removeSurrounder()
    {

    }
    private void removeSurrounders(int num)
    {

    }

    private void countRotation()
    {
        float nowAngle = 0;
        for( int i = 0 ; i <= surrounderCount ; i++ )
        {
            surrounders[i].GetComponent<Surround>().setSurround(3.1416f , nowAngle , target.transform , surrounderDistance);
            nowAngle += surrounderAngle;
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     target = other.gameObject;
    // }

    private void setDistance()
    {
        if (target.transform.localScale.x >= target.transform.localScale.y)
        {
            for( int i = 0 ; i < surrounderCount ; i++ )
            {
                surrounders[i].GetComponent<Surround>().setDistance(surrounderDistance * target.transform.localScale.x);
            }
        }
        else if (target.transform.localScale.x < target.transform.localScale.y)
        {
            for( int i = 0 ; i < surrounderCount ; i++ )
            {
                surrounders[i].GetComponent<Surround>().setDistance(surrounderDistance * target.transform.localScale.y);
            }
        }
    }

}
