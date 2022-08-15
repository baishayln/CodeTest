using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private PEAttackRange PEAttackRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getPlayer(PEAttackRange attackstateact)
    {
        PEAttackRange = attackstateact;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && gameObject.CompareTag("PlayerAttack"))
        {
            PEAttackRange.returnTarget(other);
        }
        
        if (other.CompareTag("Player") && gameObject.CompareTag("EnemyAttack"))
        {
            PEAttackRange.returnTarget(other);
        }
    }
}
