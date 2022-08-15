using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estateact : EState
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

public class Idlestateact : EState
{
    private Paramete paramete;
    private Enemycontroller1 manager;
    private float timer = 0;
    private int direct = 0;
    public Idlestateact(Enemycontroller1 manager)
    {
        this.manager = manager;
        this.paramete = manager.paramete;
    }
    public void OnEnter()
    {
        paramete.animator.Play("Idle");
    }

    public void OnUpdate()
    {
        if (paramete.target != null)
        {
            manager.TransitionState(EStateType.Chase);
        }
        timer += Time.deltaTime;
        if(timer > paramete.idleTime)
        {
            manager.TransitionState(EStateType.Patrol);
            timer = 0;
        }
    }

    public void OnExit()
    {

    }

}

public class Patrolstateact : EState
{
    private Paramete paramete;
    private Enemycontroller1 manager;
    private float timer;
    private int direct = 0;
    private Vector3 d = new Vector3(1 , 1 , 1);
    public Patrolstateact(Enemycontroller1 manager)
    {
        this.manager = manager;
        this.paramete = manager.paramete;
    }
    public void OnEnter()
    {
        paramete.animator.Play("Walk");
    }

    public void OnUpdate()
    {

        if (paramete.target != null)
        {
            manager.TransitionState(EStateType.Chase);
        }
        
        manager.transform.localScale = d;

        manager.transform.position = new Vector2(manager.transform.position.x + 
                Time.deltaTime * paramete.moveSpeed , manager.transform.position.y); 
    }

    public void OnExit()
    {

    }

}


public class Chasestateact : EState
{
    private Enemycontroller1 manager;
    private Paramete paramete;

    public Chasestateact(Enemycontroller1 manager)
    {
        this.manager = manager;
        this.paramete = manager.paramete;
    }
    public void OnEnter()
    {
        paramete.animator.Play("Walk");
    }

    public void OnUpdate()
    {
        manager.FlipTo(paramete.target);
        if (paramete.target)
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
            paramete.target.position, paramete.chaseSpeed * Time.deltaTime);

        if(manager.transform.position.x - paramete.target.transform.position.x > 7 || manager.transform.position.x - paramete.target.transform.position.x < -7)
        {
            paramete.target = null;
            manager.TransitionState(EStateType.Idle);
        }

        if (Physics2D.OverlapCircle(paramete.attackPoint.position, paramete.attackArea, paramete.targetLayer))
        {
            manager.TransitionState(EStateType.DashPrepare);
        }
    }

    public void OnExit()
    {

    }
}

public class DashPreparestateact : EState
{
    private Enemycontroller1 manager;
    private Paramete paramete;
    private float timer;

    public DashPreparestateact(Enemycontroller1 manager)
    {
        this.manager = manager;
        this.paramete = manager.paramete;
    }
    public void OnEnter()
    {
        paramete.animator.Play("Idle");
    }

    public void OnUpdate()
    {
        manager.FlipTo(paramete.target);
        timer += Time.deltaTime;
        if(timer > 2)
        {
            timer = 0;
            manager.TransitionState(EStateType.Dash);
        }

        if(manager.transform.position.x - paramete.target.transform.position.x > 7 || manager.transform.position.x - paramete.target.transform.position.x < -7)
        {
            paramete.target = null;
            manager.TransitionState(EStateType.Idle);
        }

    }

    public void OnExit()
    {

    }
}

public class DashAtkstateact : EState , PEAttackRange
{
    private Paramete paramete;
    private Enemycontroller1 manager;
    private float timer = 0;
    private float dashTime = 0.5f;
    [SerializeField]public float dashAtkSpeed = 7;
    private GameObject attackTarget;
    private float speedDownspd;
    private int dashDirect = 0;
    public DashAtkstateact(Enemycontroller1 manager)
    {
        this.manager = manager;
        this.paramete = manager.paramete;
    }
    public void OnEnter()
    {
        manager.transform.GetChild(2).gameObject.transform.GetComponent<AttackRange>().getPlayer(this);
        paramete.animator.Play("Attack");
        if(paramete.target.position.x > manager.transform.position.x) 
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
            manager.TransitionState(EStateType.Patrol);
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


public class Hitedstateact : EState
{
    private Paramete paramete;
    private Enemycontroller1 manager;
    private float timer = 0;
    private int direct = 0;
    private AnimatorStateInfo info;
    private float speed;
    private float Yspeed;
    private float speedChangeSpeed = 15;
    Rigidbody2D rig;

    public Hitedstateact(Enemycontroller1 manager)
    {
        this.manager = manager;
        this.paramete = manager.paramete;
    }
    public void OnEnter()
    {
        rig = manager.transform.GetComponent<Rigidbody2D>();

        paramete.animator.Play("Hit");
        
        // paramete.rigidbody.velocity = paramete.direction * paramete.atkspeed;
        paramete.rigidbody.velocity = new Vector2(paramete.selfNowRepelDirection.x * paramete.direction.x , paramete.selfNowRepelDirection.y);

        speed = paramete.rigidbody.velocity.x;

        paramete.isHit = false;
    }

    public void OnUpdate()
    {
        
        if (paramete.Collider.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果碰撞器碰撞到地板层，则会开始X轴方向的减速
        {
            speed = Mathf.MoveTowards(speed , 0 , speedChangeSpeed * Time.deltaTime);
        }

        if(speed < 0.01 && speed > -0.01)
        {
            speed = 0;
            manager.TransitionState(EStateType.Patrol);
        }

        rig.velocity = new Vector2(speed, paramete.rigidbody.velocity.y);
        
        // if (info.normalizedTime >= .95f)
        // {
        //     manager.TransitionState(EStateType.Chase);
        // }
    }

    public void OnExit()
    {

    }

}

public class Breakstateact : EState
{
    private Paramete paramete;
    private Enemycontroller1 manager;
    private float timer = 0;
    private float breakTime = 5;
    private int direct = 0;
    private AnimatorStateInfo info;
    Rigidbody2D rig;

    public Breakstateact(Enemycontroller1 manager)
    {
        this.manager = manager;
        this.paramete = manager.paramete;
    }
    public void OnEnter()
    {
        timer = breakTime;
        paramete.animator.Play("Break");
    }

    public void OnUpdate()
    {
        timer -= Time.deltaTime;
        
        if(timer < 0)
        {
            manager.TransitionState(EStateType.Idle);
            timer = 0;
            paramete.nowStiff = paramete.stiff;
        }
    }

    public void OnExit()
    {
        
    }

}

public class Deathstateact : EState
{
    private Paramete paramete;
    private Enemycontroller1 manager;
    private float dieTimer;
    private BoxCollider2D body;
    private AnimatorStateInfo info;
    private float x;

    public Deathstateact(Enemycontroller1 manager)
    {
        this.manager = manager;
        this.paramete = manager.paramete;
    }
    public void OnEnter()
    {
        paramete.animator.Play("Dead");
        body = manager.transform.GetComponent<BoxCollider2D>();
        body.enabled = false;
        // x = manager.transform.position.x;
    }

    public void OnUpdate()
    {
        info = paramete.animator.GetCurrentAnimatorStateInfo(0);
        dieTimer += Time.deltaTime;
        if(dieTimer > 3 || info.normalizedTime >= .95f)
        {
            manager.Destroythis();
        }
        // paramete.rigidbody.velocity.x = 0;
        
        paramete.rigidbody.velocity = new Vector2(0 , 0.57f);
    }

    public void OnExit()
    {

    }
}

public class Rollingstateact : EState
{
    private Paramete paramete;
    private Enemycontroller1 manager;
    private float timer = 0;
    private int direct = 0;
    private AnimatorStateInfo info;
    private float speed;
    private float Yspeed;
    private float speedChangeSpeed = 15;
    private float ZRotation;
    Rigidbody2D rig;

    public Rollingstateact(Enemycontroller1 manager)
    {
        this.manager = manager;
        this.paramete = manager.paramete;
    }
    public void OnEnter()
    {
        rig = manager.transform.GetComponent<Rigidbody2D>();

        paramete.animator.Play("Hit");
        
        // paramete.rigidbody.velocity = paramete.direction * paramete.atkspeed;
        paramete.rigidbody.velocity = new Vector2(paramete.selfNowRepelDirection.x * paramete.direction.x , paramete.selfNowRepelDirection.y);

        speed = paramete.rigidbody.velocity.x;

        paramete.isRolling = false;

        ZRotation = 6;
    }

    public void OnUpdate()
    {
        
        if (paramete.Collider.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果碰撞器碰撞到地板层，则会开始X轴方向的减速
        {
            speed = Mathf.MoveTowards(speed , 0 , speedChangeSpeed * Time.deltaTime);
        }

        rig.velocity = new Vector2(speed, paramete.rigidbody.velocity.y);

        ZRotation += 6;

        manager.transform.rotation = Quaternion.Euler(0 , 0 , ZRotation);
        
        if(speed < 0.01 && speed > -0.01)
        {
            speed = 0;
            manager.TransitionState(EStateType.Idle);
        }
        
        // if (info.normalizedTime >= .95f)
        // {
        //     manager.TransitionState(EStateType.Chase);
        // }
    }

    public void OnExit()
    {
        manager.transform.rotation = Quaternion.Euler(0 , 0 , 0);
    }
}
public class HitDownstateact : EState
{
    private Paramete paramete;
    private Enemycontroller1 manager;
    private float timer = 0;
    private int direct = 0;
    private AnimatorStateInfo info;
    private float speed;
    private float Yspeed;
    private float speedChangeSpeed = 15;
    private float ZRotation;
    Rigidbody2D rig;

    public HitDownstateact(Enemycontroller1 manager)
    {
        this.manager = manager;
        this.paramete = manager.paramete;
    }
    public void OnEnter()
    {
        rig = manager.transform.GetComponent<Rigidbody2D>();

        paramete.animator.Play("Dead");
        
        // paramete.rigidbody.velocity = paramete.direction * paramete.atkspeed;
        paramete.rigidbody.velocity = new Vector2(paramete.selfNowRepelDirection.x * paramete.direction.x , paramete.selfNowRepelDirection.y);

        speed = paramete.rigidbody.velocity.x;

        paramete.isRolling = false;

        ZRotation = 6;
    }

    public void OnUpdate()
    {
        
        if (paramete.Collider.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果碰撞器碰撞到地板层，则会开始X轴方向的减速
        {
            speed = Mathf.MoveTowards(speed , 0 , speedChangeSpeed * Time.deltaTime);
        }

        rig.velocity = new Vector2(speed, paramete.rigidbody.velocity.y);
        
        if(speed < 0.01 && speed > -0.01)
        {
            speed = 0;
            manager.TransitionState(EStateType.Idle);
        }
        
        // if (info.normalizedTime >= .95f)
        // {
        //     manager.TransitionState(EStateType.Chase);
        // }
    }

    public void OnExit()
    {
    }
}

// public class Impactstateact : EState
// {
//     private Paramete paramete;
//     private Enemycontroller1 manager;
//     private float dieTimer;

//     public Impactstateact(Enemycontroller1 manager)
//     {
//         this.manager = manager;
//         this.paramete = manager.paramete;
//     }
//     public void OnEnter()
//     {
//         manager.takeAtkSpeed(-0.3f);
//         if (paramete.selfNowRepelDirection.y > 4)
//         {
//             manager.TransitionState(EStateType.Rolling);
//         }
//         else
//         {
//             manager.TransitionState(EStateType.Hited);
//         }
//         // if (paramete.YSpeed)
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