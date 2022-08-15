using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patriotstateact : EState
{
    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }

}

public class PatriotIdlestateact : EState
{
    private PatriotParamete patriotParamete;
    private PatriotController manager;
    private float timer = 0;
    private int direct = 0;
    public PatriotIdlestateact(PatriotController manager)
    {
        this.manager = manager;
        this.patriotParamete = manager.patriotParamete;
    }
    public void OnEnter()
    {
        patriotParamete.animator.Play("Idle");
    }

    public void OnUpdate()
    {
        if (patriotParamete.target != null)
        {
            manager.TransitionState(PatriotStateType.Chase);
        }
    }

    public void OnExit()
    {

    }

}


public class PatriotChasestateact : EState
{
    private PatriotController manager;
    private PatriotParamete patriotParamete;

    public PatriotChasestateact(PatriotController manager)
    {
        this.manager = manager;
        this.patriotParamete = manager.patriotParamete;
    }
    public void OnEnter()
    {
        patriotParamete.animator.Play("Walk");
    }

    public void OnUpdate()
    {
        manager.FlipTo(patriotParamete.target);
        if (patriotParamete.target)
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
            patriotParamete.target.position, patriotParamete.chaseSpeed * Time.deltaTime);

        // if(manager.transform.position.x - patriotParamete.target.transform.position.x > 7 || manager.transform.position.x - patriotParamete.target.transform.position.x < -7)
        // {
        //     patriotParamete.target = null;
        //     manager.TransitionState(PatriotStateType.Idle);
        // }

        //  这个部分是原本当目标处于一个用于判定的范围内时，切换至攻击准备状态
        // if (Physics2D.OverlapCircle(patriotParamete.attackPoint.position, patriotParamete.attackArea, patriotParamete.targetLayer))
        // {
        //     manager.TransitionState(PatriotStateType.DashPrepare);
        // }

        if (patriotParamete.patriotMissileAimLockStart) //在移动时如果导弹判定timer到达2秒，会进入导弹准备阶段
        {
            manager.TransitionState(PatriotStateType.MissilePrepare);   //只有移动状态机可以直接调用状态切换函数切换导弹预备状态
        }

    }

    public void OnExit()
    {

    }
}

public class PatriotDashPreparestateact : EState
{
    private PatriotController manager;
    private PatriotParamete patriotParamete;
    private float timer;

    public PatriotDashPreparestateact(PatriotController manager)
    {
        this.manager = manager;
        this.patriotParamete = manager.patriotParamete;
    }
    public void OnEnter()
    {
        patriotParamete.animator.Play("Idle");
    }

    public void OnUpdate()
    {
        manager.FlipTo(patriotParamete.target);
        timer += Time.deltaTime;
        if(timer > 2)
        {
            timer = 0;
            manager.TransitionState(PatriotStateType.Dash);
        }

        if(manager.transform.position.x - patriotParamete.target.transform.position.x > 7 || manager.transform.position.x - patriotParamete.target.transform.position.x < -7)
        {
            patriotParamete.target = null;
            manager.TransitionState(PatriotStateType.Idle);
        }

    }

    public void OnExit()
    {

    }
}

public class PatriotDashAtkstateact : EState , PEAttackRange
{
    private PatriotParamete patriotParamete;
    private PatriotController manager;
    private float timer = 0;
    private float dashTime = 0.5f;
    [SerializeField]public float dashAtkSpeed = 7;
    private GameObject attackTarget;
    private float speedDownspd;
    private int dashDirect = 0;
    public PatriotDashAtkstateact(PatriotController manager)
    {
        this.manager = manager;
        this.patriotParamete = manager.patriotParamete;
    }
    public void OnEnter()
    {
        manager.transform.GetChild(2).gameObject.transform.GetComponent<AttackRange>().getPlayer(this);
        patriotParamete.animator.Play("Attack");
        if(patriotParamete.target.position.x > manager.transform.position.x) 
        {
            dashDirect = 1;
        }
        else 
        {
            dashDirect = -1;
        }
        timer = 0;
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if(timer < dashTime)
        {
            manager.transform.position = new Vector2(manager.transform.position.x + 
                Time.deltaTime * dashAtkSpeed * dashDirect , manager.transform.position.y); 
        }
        if ( dashDirect > 0 )    manager.transform.localScale = new Vector3(1, 1, 1);
        else if ( dashDirect < 0 )    manager.transform.localScale = new Vector3(-1, 1, 1);
        if(timer > dashTime)
        {
            timer = 0;
            // manager.TransitionState(PatriotStateType.Patrol);
        }
    }

    public void OnExit()
    {

    }
    
    void returnAttackDate()
    {
        if (manager.transform.position.x >= attackTarget.transform.position.x)
            attackTarget.GetComponent<EAtkAndHit>().GetHit(Vector2.left , 1 , 1 , 1 , new Vector2(1 , 1));
        else if (manager.transform.position.x < attackTarget.transform.position.x)
            attackTarget.GetComponent<EAtkAndHit>().GetHit(Vector2.right , 1 , 1 , 1 , new Vector2(1 , 1));
        
    }
    
    public void returnTarget(Collider2D enemyCollider)
    {
        attackTarget = enemyCollider.gameObject;
        returnAttackDate();
    }

}


// public class PatriotHitedstateact : EState
// {
//     private PatriotParamete patriotParamete;
//     private PatriotController manager;
//     private float timer = 0;
//     private int direct = 0;
//     private AnimatorStateInfo info;
//     private float speed;
//     private float Yspeed;
//     private float speedChangeSpeed = 15;
//     Rigidbody2D rig;

//     public Hitedstateact(PatriotController manager)
//     {
//         this.manager = manager;
//         this.patriotParamete = manager.patriotParamete;
//     }
//     public void OnEnter()
//     {
//         rig = manager.transform.GetComponent<Rigidbody2D>();

//         patriotParamete.animator.Play("Hit");
        
//         // patriotParamete.rigidbody.velocity = patriotParamete.direction * patriotParamete.atkspeed;
//         patriotParamete.rigidbody.velocity = new Vector2(patriotParamete.selfNowRepelDirection.x * patriotParamete.direction.x , patriotParamete.selfNowRepelDirection.y);

//         speed = patriotParamete.rigidbody.velocity.x;

//         patriotParamete.isHit = false;
//     }

//     public void OnUpdate()
//     {
        
//         if (patriotParamete.Collider.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果碰撞器碰撞到地板层，则会开始X轴方向的减速
//         {
//             speed = Mathf.MoveTowards(speed , 0 , speedChangeSpeed * Time.deltaTime);
//         }

//         if(speed < 0.01 && speed > -0.01)
//         {
//             speed = 0;
//             manager.TransitionState(PatriotStateType.Patrol);
//         }

//         rig.velocity = new Vector2(speed, patriotParamete.rigidbody.velocity.y);
        
//         // if (info.normalizedTime >= .95f)
//         // {
//         //     manager.TransitionState(PatriotStateType.Chase);
//         // }
//     }

//     public void OnExit()
//     {

//     }

// }

public class PatriotBreakstateact : EState
{
    private PatriotParamete patriotParamete;
    private PatriotController manager;
    private float timer = 0;
    private float breakTime = 5;
    private int direct = 0;
    private AnimatorStateInfo info;
    Rigidbody2D rig;

    public PatriotBreakstateact(PatriotController manager)
    {
        this.manager = manager;
        this.patriotParamete = manager.patriotParamete;
    }
    public void OnEnter()
    {
        timer = breakTime;
        patriotParamete.animator.Play("Break");
    }

    public void OnUpdate()
    {
        timer -= Time.deltaTime;
        
        if(timer < 0)
        {
            manager.TransitionState(PatriotStateType.Idle);
            timer = 0;
            patriotParamete.nowStiff = patriotParamete.stiff;
        }
    }

    public void OnExit()
    {
        
    }

}

public class PatriotDeathstateact : EState
{
    private PatriotParamete patriotParamete;
    private PatriotController manager;
    private float dieTimer;
    private BoxCollider2D body;
    private AnimatorStateInfo info;
    private float x;

    public PatriotDeathstateact(PatriotController manager)
    {
        this.manager = manager;
        this.patriotParamete = manager.patriotParamete;
    }
    public void OnEnter()
    {
        patriotParamete.animator.Play("Dead");
        body = manager.transform.GetComponent<BoxCollider2D>();
        body.enabled = false;
        // x = manager.transform.position.x;
    }

    public void OnUpdate()
    {
        info = patriotParamete.animator.GetCurrentAnimatorStateInfo(0);
        dieTimer += Time.deltaTime;
        if(dieTimer > 3 || info.normalizedTime >= .95f)
        {
            manager.Destroythis();
        }
        // patriotParamete.rigidbody.velocity.x = 0;
        
        patriotParamete.rigidbody.velocity = new Vector2(0 , 0.57f);
    }

    public void OnExit()
    {

    }
}

// public class PatriotRollingstateact : EState
// {
//     private PatriotParamete patriotParamete;
//     private PatriotController manager;
//     private float timer = 0;
//     private int direct = 0;
//     private AnimatorStateInfo info;
//     private float speed;
//     private float Yspeed;
//     private float speedChangeSpeed = 15;
//     private float ZRotation;
//     Rigidbody2D rig;

//     public Rollingstateact(PatriotController manager)
//     {
//         this.manager = manager;
//         this.patriotParamete = manager.patriotParamete;
//     }
//     public void OnEnter()
//     {
//         rig = manager.transform.GetComponent<Rigidbody2D>();

//         patriotParamete.animator.Play("Hit");
        
//         // patriotParamete.rigidbody.velocity = patriotParamete.direction * patriotParamete.atkspeed;
//         patriotParamete.rigidbody.velocity = new Vector2(patriotParamete.selfNowRepelDirection.x * patriotParamete.direction.x , patriotParamete.selfNowRepelDirection.y);

//         speed = patriotParamete.rigidbody.velocity.x;

//         patriotParamete.isRolling = false;

//         ZRotation = 6;
//     }

//     public void OnUpdate()
//     {
        
//         if (patriotParamete.Collider.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果碰撞器碰撞到地板层，则会开始X轴方向的减速
//         {
//             speed = Mathf.MoveTowards(speed , 0 , speedChangeSpeed * Time.deltaTime);
//         }

//         rig.velocity = new Vector2(speed, patriotParamete.rigidbody.velocity.y);

//         ZRotation += 6;

//         manager.transform.rotation = Quaternion.Euler(0 , 0 , ZRotation);
        
//         if(speed < 0.01 && speed > -0.01)
//         {
//             speed = 0;
//             manager.TransitionState(PatriotStateType.Idle);
//         }
        
//         // if (info.normalizedTime >= .95f)
//         // {
//         //     manager.TransitionState(PatriotStateType.Chase);
//         // }
//     }

//     public void OnExit()
//     {
//         manager.transform.rotation = Quaternion.Euler(0 , 0 , 0);
//     }
// }
public class PatriotHitDownstateact : EState
{
    private PatriotParamete patriotParamete;
    private PatriotController manager;
    private float timer = 0;
    private int direct = 0;
    private AnimatorStateInfo info;
    private float speed;
    private float Yspeed;
    private float speedChangeSpeed = 15;
    private float ZRotation;
    Rigidbody2D rig;

    public PatriotHitDownstateact(PatriotController manager)
    {
        this.manager = manager;
        this.patriotParamete = manager.patriotParamete;
    }
    public void OnEnter()
    {
        rig = manager.transform.GetComponent<Rigidbody2D>();

        patriotParamete.animator.Play("Dead");
        
        // patriotParamete.rigidbody.velocity = patriotParamete.direction * patriotParamete.atkspeed;
        patriotParamete.rigidbody.velocity = new Vector2(patriotParamete.selfNowRepelDirection.x * patriotParamete.direction.x , patriotParamete.selfNowRepelDirection.y);

        speed = patriotParamete.rigidbody.velocity.x;

        patriotParamete.isRolling = false;

        ZRotation = 6;
    }

    public void OnUpdate()
    {
        
        if (patriotParamete.Collider.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果碰撞器碰撞到地板层，则会开始X轴方向的减速
        {
            speed = Mathf.MoveTowards(speed , 0 , speedChangeSpeed * Time.deltaTime);
        }

        rig.velocity = new Vector2(speed, patriotParamete.rigidbody.velocity.y);
        
        if(speed < 0.01 && speed > -0.01)
        {
            speed = 0;
            manager.TransitionState(PatriotStateType.Idle);
        }
        
        // if (info.normalizedTime >= .95f)
        // {
        //     manager.TransitionState(PatriotStateType.Chase);
        // }
    }

    public void OnExit()
    {
    }
}

// public class Impactstateact : EState
// {
//     private PatriotParamete patriotParamete;
//     private PatriotController manager;
//     private float dieTimer;

//     public Impactstateact(PatriotController manager)
//     {
//         this.manager = manager;
//         this.patriotParamete = manager.patriotParamete;
//     }
//     public void OnEnter()
//     {
//         manager.takeAtkSpeed(-0.3f);
//         if (patriotParamete.selfNowRepelDirection.y > 4)
//         {
//             manager.TransitionState(PatriotStateType.Rolling);
//         }
//         else
//         {
//             manager.TransitionState(PatriotStateType.Hited);
//         }
//         // if (patriotParamete.YSpeed)
//         // {
            
//         // }
//     }

//     public void OnUpdate()
//     {
//     }

//     public void OnExit()
//     {

//     }
// }
public class MoveOrNormalstateact : EState
{
    private PatriotParamete patriotParamete;
    private PatriotController manager;
    public MoveOrNormalstateact(PatriotController manager)
    {
        this.manager = manager;
        this.patriotParamete = manager.patriotParamete;
    }
    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }

}

public class Attackstateact1 : EState , PEAttackRange
{
    private PatriotParamete patriotParamete;
    private PatriotController manager;
    private float timer = 0;
    private float dashTime = 0.5f;
    [SerializeField]public float dashAtkSpeed = 7;
    private GameObject attackTarget;
    private int dashDirect = 0;
    public Attackstateact1(PatriotController manager)
    {
        this.manager = manager;
        this.patriotParamete = manager.patriotParamete;
    }
    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {

    }
    
    void returnAttackDate()
    {
        if (manager.transform.position.x >= attackTarget.transform.position.x)
            attackTarget.GetComponent<EAtkAndHit>().GetHit(Vector2.left , 1 , 1 , 1 , new Vector2(1 , 1));
        else if (manager.transform.position.x < attackTarget.transform.position.x)
            attackTarget.GetComponent<EAtkAndHit>().GetHit(Vector2.right , 1 , 1 , 1 , new Vector2(1 , 1));
        
    }
    
    public void returnTarget(Collider2D enemyCollider)
    {
        attackTarget = enemyCollider.gameObject;
        returnAttackDate();
    }

}

public class MissilePrestateact : EState
{
    private PatriotParamete patriotParamete;
    private PatriotController manager;
    public GameObject patriotMissile;
    private float firePointX;
    private float firePointY;
    private Vector2 firePoint;
    private Vector2 fireRotation;
    private float missileSpeed = 60;
    private GameObject mainCam;
    public MissilePrestateact(PatriotController manager)
    {
        this.manager = manager;
        this.patriotParamete = manager.patriotParamete;
        patriotMissile = manager.patriotMissile;
        mainCam = GameObject.Find ("Main Camera");
    }
    public void OnEnter()
    {
        firePointX = patriotParamete.target.transform.position.x - manager.transform.position.x;
        firePointY = patriotParamete.target.transform.position.y - manager.transform.position.y;
        firePoint = new Vector2( manager.transform.position.x + (firePointX/Mathf.Sqrt((firePointX * firePointX) + firePointY * firePointY)) , manager.transform.position.y + (firePointY/Mathf.Sqrt(firePointX * firePointX + firePointY * firePointY)));
        fireRotation = new Vector2(firePointX,firePointY).normalized;
        Debug.Log("爱国者导弹准备发射。");
        GameObject missile = GameObject.Instantiate(manager.patriotMissile , firePoint , Quaternion.identity);
        missile.transform.up = fireRotation;
        missile.GetComponent<LinearProjectile>().setProjectileDate(missileSpeed);
    }

    public void OnUpdate()
    {
        manager.TransitionState(PatriotStateType.Chase);
    }

    public void OnExit()
    {

    }

}