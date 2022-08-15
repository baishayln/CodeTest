using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mlgb : MonoBehaviour
{
    Vector3 bdmgcllinder = new Vector3(1, 1, 1);
    Collider[] onDamage;
    Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 bedamaged = new Vector3(10, 10, 0);
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("wdnmd");
            rig.velocity = bedamaged;
        }
    }
}