using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStateType
{
    Idle, Patrol, Chase, React, Attack, Hit, Death, Dash ,Hited ,Break ,DashPrepare ,Rolling ,Impact ,HitDown
}

[Serializable]
public class Paramete
{
    public float moveSpeed = 2;
    public float chaseSpeed = 5;
    public float idleTime = 15f;
    public Transform[] patrolPoints;
    public Transform[] chasePoints;
    public Transform target;
    public LayerMask targetLayer;
    public Transform attackPoint;
    public float attackArea = 0.3f;
    public Animator animator;
    public bool getHit;
    public float health = 1000;
    public float attack = 100;
    public float def;
    public float stiff = 250;
    public float nowStiff;
    public float trueDamage;
    public float speed = 5;
    public float atkspeed;
    public float weight = 2;
    public Vector2 direction;
    public bool isHit;
    public bool isBreak;
    public bool isRolling;
    public bool isImpact;
    public AnimatorStateInfo info;
    public Animator hitAnimator;
    public Rigidbody2D rigidbody;
    public Collider2D Collider;
    public Vector2 selfNowRepelDirection;
    public float YSpeed;   //检测Y轴速度
    public float XSpeed;   //检测X轴速度
}

public class Enemycontroller1 : MonoBehaviour , EAtkAndHit , AirCondition
{
    private EState currentState;
    private Dictionary<EStateType, EState> states = new Dictionary<EStateType, EState>();

    public Paramete paramete;
    public AudioSource hitedSound;

    // Start is called before the first frame update
    void Start()
    {
        states.Add(EStateType.Idle, new Idlestateact(this));
        states.Add(EStateType.Patrol, new Patrolstateact(this));
        states.Add(EStateType.Dash, new DashAtkstateact(this));
        states.Add(EStateType.Hited, new Hitedstateact(this));
        states.Add(EStateType.Break, new Breakstateact(this));
        states.Add(EStateType.Death, new Deathstateact(this));
        states.Add(EStateType.Chase, new Chasestateact(this));
        states.Add(EStateType.DashPrepare, new DashPreparestateact(this));
        states.Add(EStateType.Rolling, new Rollingstateact(this));
        states.Add(EStateType.HitDown, new HitDownstateact(this));
        // states.Add(EStateType.Impact, new Impactstateact(this));

        paramete.animator = transform.GetComponent<Animator>();

        TransitionState(EStateType.Idle);

        paramete.rigidbody = transform.GetComponent<Rigidbody2D>();

        paramete.nowStiff = paramete.stiff;

        paramete.Collider = transform.GetComponent<BoxCollider2D>();

        hitedSound = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();
        if (paramete.health <= 0)
        {
            Die();
        }
        else if (paramete.isHit)
        {
            TransitionState(EStateType.Hited);

            // paramete.rigidbody.velocity = paramete.direction * paramete.speed;
            // paramete.isHit = false;

            //if (info.normalizedTime >= .6f)
            //    isHit = false;
        }
        // if (paramete.isRolling)
        // {
        //     TransitionState(EStateType.Rolling);
        // }

        paramete.YSpeed = paramete.rigidbody.velocity.y;
        

    }

    public void TransitionState(EStateType state)
    {
        // Debug.Log("我正在切换状态");

        if(currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[state];
        currentState.OnEnter();
        
        // Debug.Log("当前状态为" + currentState);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            paramete.target = other.transform;
        }
        if(other.CompareTag("DeadLine"))
        {
            Destroy(gameObject);
        }
    }
    
    // void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Ground") && paramete.YSpeed < -8)
    //     {
    //         // TransitionState(EStateType.Impact);
            
    //         takeAtkSpeed(paramete.rigidbody.velocity.x , paramete.YSpeed * -0.3f);
    //         if (paramete.selfNowRepelDirection.y > 5)
    //         {
    //             TransitionState(EStateType.Hited);
    //         }
    //         else
    //         {
    //             TransitionState(EStateType.HitDown);  //后续替换为倒地动画而不是受击动画
    //         }
    //     }
    // }
    
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         paramete.target = null;
    //     }
    // }

    public void FlipTo(Transform target)
    {
        if (target != null)
        {
            if (transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    
    public virtual void takeDamage(float dmg)
    {
        if(dmg<=paramete.def)
        {
            paramete.trueDamage=1;
            paramete.health -= paramete.trueDamage;
        }
        else
            paramete.health -=dmg;
        // if (paramete.health <= 0)
        // {
        //     Die();
        // }
        // attacked();
    }
    public virtual void takeAtkSpeed(float force , Vector2 repelDirection)
    {
        paramete.atkspeed = force/paramete.weight;
        paramete.selfNowRepelDirection = paramete.atkspeed * repelDirection;
    }
    // public virtual void takeAtkSpeed(float Ymul)
    // {
    //     paramete.selfNowRepelDirection.x = paramete.rigidbody.velocity.x;
    //     paramete.selfNowRepelDirection.y = paramete.rigidbody.velocity.y * Ymul;
    // }
    public virtual void takeAtkSpeed(float Xspd , float Yspd)
    {
        paramete.selfNowRepelDirection.x = Xspd;
        paramete.selfNowRepelDirection.y = Yspd;
    }

    public virtual void stiffDown(float pd)
    {
        paramete.nowStiff -= pd;
        if (paramete.nowStiff <= 0)
        {
            paramete.isHit = false;
            pofang();
        }
        
    }

    public virtual void Die()
    {
        TransitionState(EStateType.Death);
    }

    public virtual void pofang()
    {
        TransitionState(EStateType.Break);
    }

    public virtual void attacked()
    {

    }

    public virtual void Destroythis()
    {
        Destroy(gameObject);
    }
    public void GetHit(Vector2 direction , float force , float dmg , float stfdmg , Vector2 repelDirection)
    {
        transform.localScale = new Vector3(-direction.x, 1, 1);     //根据direction调整朝向，direction.x为1时朝左，为-1时朝右。direction为击退方向，当玩家X大于受击者X时（玩家在受击者右侧），direction为左（x为-1），否则为右（x为1）
        this.paramete.direction = direction;
        takeAtkSpeed(force , repelDirection);
        // if (paramete.selfNowRepelDirection.y > 5)
        // {
        //     paramete.isRolling = true;
        // }
        // else
        // {
        //     paramete.isHit = true;
        // }
        paramete.isHit = true;
        stiffDown(stfdmg);
        takeDamage(dmg);
        hitedSound.Play();
        
        //animator.SetTrigger("Hit");
        //hitAnimator.SetTrigger("Hit");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(paramete.attackPoint.position, paramete.attackArea);
    }

    public bool getIsRolling()
    {
        return paramete.isRolling;
    }

}
