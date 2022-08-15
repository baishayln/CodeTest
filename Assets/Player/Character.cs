using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    
    private float health = 25;
    private float attack = 50;
    private float attackForce = 10;
    private float def = 0;
    private float Weight;
    private float atkspeed;
    private float trueDamage;
    private float stiff;
    private float stiffAttack = 50;
    private float speed;
    private Vector2 direction;
    private bool isHited;
    private bool isDead;
    private AnimatorStateInfo info;
    private Animator animator;
    private Animator hitAnimator;
    new private Rigidbody2D rigidbody;


    public float getAttack()
    {
        return attack;
    }
    
    public float getHealth()
    {
        return health;
    }
    public float getAttackForce()
    {
        return attackForce;
    }
    public float getStiff()
    {
        return stiff;
    }
    public float getStiffAtk()
    {
        return stiffAttack;
    }
    public virtual void takeDamage(float dmg)
    {
        if(dmg<=def)
            trueDamage = 1;
        else
            trueDamage = dmg - def;
        health -= trueDamage;
        if (health <= 0)
        {
            Die();
        }
        attacked();
    }

    public virtual void stiffDown(float pd)
    {
        stiff -= pd;
        if (stiff <= 0)
        {
            pofang();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void pofang()
    {

    }

    public virtual void attacked()
    {

    }
    public void GetHit(Vector2 direction)
    {

        transform.localScale = new Vector3(-direction.x, 1, 1);
        isHited = true;
        this.direction = direction;
        animator.SetTrigger("Hit");
        hitAnimator.SetTrigger("Hit");

    }
    public void DeathLine()
    {
        health = 0;
    }
    public virtual float takeSpeed(float force)
    {
        atkspeed = force/Weight;
        return atkspeed;
    }
    public virtual bool getIsDead()
    {
        if(health > 0)
            isDead = false;
        else
            isDead = true;
        return isDead;
    }

    // 此函数没有必要，因为对状态的判断在Controller中完成
    // public virtual void takeHit()
    // {
    //     isHited = true;
    // }

    // Start is called before the first frame update
}
