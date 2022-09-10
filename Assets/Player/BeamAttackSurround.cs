using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttackSurround : TracerLauncher
{
    public float shootWaitTime;
    private float shootTimer = 0;
    public float SeekEnemyDsts;

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
        if (shootTarget.transform.position.x - transform.position.x > SeekEnemyDsts || 
        shootTarget.transform.position.x - transform.position.x < -SeekEnemyDsts || 
        shootTarget.transform.position.y - transform.position.y > SeekEnemyDsts || 
        shootTarget.transform.position.y - transform.position.y < -SeekEnemyDsts)
        {
            shootTarget = null;
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
