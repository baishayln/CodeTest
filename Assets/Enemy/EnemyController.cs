using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, EnemyCT
{
    public float health = 1000;
    public float attack;
    public float def;
    public float stiff;
    public float trueDamage;
    public float speed = 5;
    private Vector2 direction;
    private bool isHit;
    private AnimatorStateInfo info;
    private Animator animator;
    private Animator hitAnimator;
    new private Rigidbody2D rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        //animator = transform.GetComponent<Animator>();
        //hitAnimator = transform.GetChild(0).GetComponent<Animator>();
        rigidbody = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //info = animator.GetCurrentAnimatorStateInfo(0);
        if (isHit)
        {
            rigidbody.velocity = direction * speed;
            isHit = false;

            //if (info.normalizedTime >= .6f)
            //    isHit = false;
        }
    }
    public virtual void takeDamage(float dmg)
    {
        if(dmg<=def)
        {
            trueDamage=1;
            health -= trueDamage;
        }
        else
            health -=dmg;
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
        isHit = true;
        this.direction = direction;
        //animator.SetTrigger("Hit");
        //hitAnimator.SetTrigger("Hit");
    }
}
