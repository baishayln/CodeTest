using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface EAtkAndHit
{
    void GetHit(Vector2 direction , float force , float dmg , float stfd , Vector2 repelDirection);
}
