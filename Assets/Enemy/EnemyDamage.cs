using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    Vector3 bdmgcllinder = new Vector3(1, 1, 1);
    Collider[] onDamage;
    Rigidbody rig;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        onDamage = Physics.OverlapBox(transform.position, bdmgcllinder, Quaternion.identity, LayerMask.GetMask("Attack"));
    }
    void Start()
    {
        rig = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 bedamaged = new Vector3(10, 10, 0);
        if (onDamage.Length != 0)
        {
            rig.velocity = bedamaged;
        }
    }
}