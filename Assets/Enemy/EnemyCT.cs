using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyCT
{

    void takeDamage(float dmg);

    void stiffDown(float pd);

    void Die();

    void pofang();

    void attacked();
    void GetHit(Vector2 direction);
}
