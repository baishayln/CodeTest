using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttackSurround : TracerLauncher
{
    public float shootWaitTime;
    private float shootTimer = 0;

    void Update()
    {
        if (shootTarget)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                shootTimer = shootWaitTime;
                fire(transform.position , shootTarget.transform.position);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AttackTarget"))
        {
            shootTarget = other.transform;
        }
        
    }
}
