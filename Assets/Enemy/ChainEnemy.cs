using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainEnemy : MonoBehaviour
{
    private Collider2D[] targets;
    public Collider2D[] GetChainAllTargets()
    {
        targets = SearchSameTagInRange(transform.position , 5 , transform.tag);
        return targets;
    }
    public Collider2D GetCurrentChainTarget()
    {
        targets = SearchSameTagInRange(transform.position , 5 , transform.tag);
        Collider2D target;
        // float distance = 0;
        target = targets[0];
        for(int i = targets.Length ; i > 1 ; i--)
        {
            if (Mathf.Abs((targets[i].transform.position.x - transform.position.x) * (targets[i].transform.position.x - transform.position.x) + (targets[i].transform.position.y - transform.position.y) * (targets[i].transform.position.y - transform.position.y)) <
            Mathf.Abs((target.transform.position.x - transform.position.x) * (target.transform.position.x - transform.position.x) + (target.transform.position.y - transform.position.y) * (target.transform.position.y - transform.position.y)))
            {
                target = targets[i];
            }
        }
        return target;
    }
    private Collider2D[] SearchSameTagInRange(Vector2 point , float radius , string tag)
    {
        return Physics2D.OverlapCircleAll(point , radius , LayerMask.GetMask(tag));
    }
}
