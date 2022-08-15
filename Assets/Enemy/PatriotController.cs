using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatriotStateType
{
    Idle, BattleBegin, MissilePrepare, Chase, React, Attack, Hit, Death, Dash ,Hited ,Break ,DashPrepare ,Rolling ,Impact ,HitDown
}

[Serializable]
public class PatriotParamete
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
    public float patriotMissileAimTimer = 0;
    public bool patriotMissileAimLockStart = false;
    public float patriotMissileAimCD = 0;
    
}

public class PatriotController : MonoBehaviour , EAtkAndHit , AirCondition
{
    private EState currentState;
    private Dictionary<PatriotStateType, EState> states = new Dictionary<PatriotStateType, EState>();

    public PatriotParamete patriotParamete;
    public AudioSource hitedSound;
    public GameObject patriotMissile;

    // Start is called before the first frame update
    void Start()
    {
        states.Add(PatriotStateType.Idle, new PatriotIdlestateact(this));
        states.Add(PatriotStateType.Dash, new PatriotDashAtkstateact(this));
        states.Add(PatriotStateType.Break, new PatriotBreakstateact(this));
        states.Add(PatriotStateType.Death, new PatriotDeathstateact(this));
        states.Add(PatriotStateType.Chase, new PatriotChasestateact(this));
        states.Add(PatriotStateType.DashPrepare, new PatriotDashPreparestateact(this));
        states.Add(PatriotStateType.HitDown, new PatriotHitDownstateact(this));
        states.Add(PatriotStateType.MissilePrepare, new MissilePrestateact(this));
        // states.Add(PatriotStateType.Hited, new PatriotHitedstateact(this));
        // states.Add(PatriotStateType.Rolling, new PatriotRollingstateact(this));
        // states.Add(PatriotStateType.Impact, new Impactstateact(this));

        patriotParamete.animator = transform.GetComponent<Animator>();

        TransitionState(PatriotStateType.Idle);

        patriotParamete.rigidbody = transform.GetComponent<Rigidbody2D>();

        patriotParamete.nowStiff = patriotParamete.stiff;

        patriotParamete.Collider = transform.GetComponent<BoxCollider2D>();

        hitedSound = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();
        if (patriotParamete.health <= 0)
        {
            Die();
        }
        else if (patriotParamete.isHit)
        {
            TransitionState(PatriotStateType.Hited);

            // patriotParamete.rigidbody.velocity = patriotParamete.direction * patriotParamete.speed;
            // patriotParamete.isHit = false;

            //if (info.normalizedTime >= .6f)
            //    isHit = false;
        }
        // if (patriotParamete.isRolling)
        // {
        //     TransitionState(PatriotStateType.Rolling);
        // }

        patriotParamete.YSpeed = patriotParamete.rigidbody.velocity.y;

        if(patriotParamete.target != null)
        {
            if (patriotParamete.target.transform.position.y - gameObject.transform.position.y > 3)
            {
                if (patriotParamete.patriotMissileAimTimer < 2.1)
                {
                    patriotParamete.patriotMissileAimTimer += Time.deltaTime;
                }
                if (patriotParamete.patriotMissileAimTimer > 2 && patriotParamete.patriotMissileAimCD <= 0)
                {
                    patriotParamete.patriotMissileAimLockStart = true;
                }
                else
                {
                    patriotParamete.patriotMissileAimLockStart = false;
                }
            }
            else
            {
                if (patriotParamete.patriotMissileAimTimer > 0)
                {
                    patriotParamete.patriotMissileAimTimer -= Time.deltaTime;
                }
                if (patriotParamete.patriotMissileAimTimer < 2 || patriotParamete.patriotMissileAimCD > 0)
                {
                    patriotParamete.patriotMissileAimLockStart = false;
                }
            }
        }
        if (patriotParamete.patriotMissileAimCD > 0)
        {
            patriotParamete.patriotMissileAimCD -= Time.deltaTime;
        }

    }

    public void PatriotStateSwitch(PatriotStateType state)  //除移动外的所有动作结束后调用这个函数而不是直接调用状态切换函数
    {
        if (patriotParamete.patriotMissileAimLockStart)
        {
            TransitionState(PatriotStateType.MissilePrepare);
        }
        else
        {
            TransitionState(state);
        }
    }

    public void TransitionState(PatriotStateType state)
    {
        // Debug.Log("我正在切换状态");

        if(currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[state];
        currentState.OnEnter();

        if (state == PatriotStateType.MissilePrepare)
        {
            patriotParamete.patriotMissileAimCD = 4;
            patriotParamete.patriotMissileAimLockStart = false;
        }
        
        
        // Debug.Log("当前状态为" + currentState);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            patriotParamete.target = other.transform;
        }
        if(other.CompareTag("DeadLine"))
        {
            Destroy(gameObject);
        }
    }
    
    // void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Ground") && patriotParamete.YSpeed < -8)
    //     {
    //         // TransitionState(PatriotStateType.Impact);
            
    //         takeAtkSpeed(patriotParamete.rigidbody.velocity.x , patriotParamete.YSpeed * -0.3f);
    //         if (patriotParamete.selfNowRepelDirection.y > 5)
    //         {
    //             TransitionState(PatriotStateType.Hited);
    //         }
    //         else
    //         {
    //             TransitionState(PatriotStateType.HitDown);  //后续替换为倒地动画而不是受击动画
    //         }
    //     }
    // }
    
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         patriotParamete.target = null;
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
        if(dmg<=patriotParamete.def)
        {
            patriotParamete.trueDamage=1;
            patriotParamete.health -= patriotParamete.trueDamage;
        }
        else
            patriotParamete.health -=dmg;
        // if (patriotParamete.health <= 0)
        // {
        //     Die();
        // }
        // attacked();
    }
    public virtual void takeAtkSpeed(float force , Vector2 repelDirection)
    {
        patriotParamete.atkspeed = force/patriotParamete.weight;
        patriotParamete.selfNowRepelDirection = patriotParamete.atkspeed * repelDirection;
    }
    // public virtual void takeAtkSpeed(float Ymul)
    // {
    //     patriotParamete.selfNowRepelDirection.x = patriotParamete.rigidbody.velocity.x;
    //     patriotParamete.selfNowRepelDirection.y = patriotParamete.rigidbody.velocity.y * Ymul;
    // }
    public virtual void takeAtkSpeed(float Xspd , float Yspd)
    {
        patriotParamete.selfNowRepelDirection.x = Xspd;
        patriotParamete.selfNowRepelDirection.y = Yspd;
    }

    public virtual void stiffDown(float pd)
    {
        patriotParamete.nowStiff -= pd;
        if (patriotParamete.nowStiff <= 0)
        {
            patriotParamete.isHit = false;
            pofang();
        }
        
    }

    public virtual void Die()
    {
        TransitionState(PatriotStateType.Death);
    }

    public virtual void pofang()
    {
        TransitionState(PatriotStateType.Break);
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
        this.patriotParamete.direction = direction;
        takeAtkSpeed(force , repelDirection);
        // if (patriotParamete.selfNowRepelDirection.y > 5)
        // {
        //     patriotParamete.isRolling = true;
        // }
        // else
        // {
        //     patriotParamete.isHit = true;
        // }
        patriotParamete.isHit = true;
        stiffDown(stfdmg);
        takeDamage(dmg);
        hitedSound.Play();
        
        //animator.SetTrigger("Hit");
        //hitAnimator.SetTrigger("Hit");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(patriotParamete.attackPoint.position, patriotParamete.attackArea);
    }

    public bool getIsRolling()
    {
        return patriotParamete.isRolling;
    }

}
