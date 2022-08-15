using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pstateact : EState
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


public class PlayerIdleMovestateact : PState
{
    private CharacterParamete characterParamete;
    private PlayerController1 manager;
    private Character character;
    private OperationListener operationListener;
    private float runInterval = 0.3f;
    private float runIntervalTimer = 0;
    public PlayerIdleMovestateact(PlayerController1 manager)
    {
        this.manager = manager;
        this.characterParamete = manager.characterParamete;
        this.character = manager.character;
        this.operationListener = manager.operationListener;
    }
    public void OnEnter()
    {
        characterParamete.downSpeed = characterParamete.rig.velocity.y;
        characterParamete.speed = characterParamete.rig.velocity.x;
    }

    public void OnUpdate()
    {
        Move();
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(operationListener.getFirstOperation() == operationListener.getSecondOperation() && operationListener.getSecondTimer() > 0)
            {
                if(runIntervalTimer <= 0)
                {
                    manager.TransitionState(PStateType.Dash);
                    runIntervalTimer = runInterval;
                }
                operationListener.clearHistory();
            }
        }
        if(runIntervalTimer > 0)
        {
            runIntervalTimer -= Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            manager.TransitionState(PStateType.Attack);
        }
        // if(operationListener.getFirstOperation() == operationListener.getSecondOperation() && operationListener.getSecondTimer() > 0)
        // {
        //     Debug.Log("润！");
        //     manager.TransitionState(PStateType.Run);
        // }
    }

    public void OnExit()
    {

    }
    void Move()
    {
        characterParamete.downSpeed = characterParamete.rig.velocity.y;
        characterParamete.downSpeed = Mathf.Clamp(characterParamete.downSpeed , -characterParamete.MAX_SPPED_DOWN , characterParamete.MAX_SPPED_UP);
        // if (!characterParamete.jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果脚底的碰撞器没有碰到地板层，则会在重力基础上加一个下降速度  【为了优化手感】
        // {
        //     characterParamete.downSpeed -= 0.15f;
        // }
        if(!characterParamete.isRun && (characterParamete.speed > 6 || characterParamete.speed < -6))    //如果不是跑动状态但当前速度大于6（冲刺结束后的减速、技能、环境交互因素）则会使用较快的刹车速度
        {
            characterParamete.speed = Mathf.MoveTowards(characterParamete.speed , 0 , characterParamete.dewalkSpeedChangeSpeed * Time.deltaTime);    //将向0减速，与无跑动时相同，在速度为-6搭配6时不再执行此IF

            characterParamete.rig.velocity = new Vector2(characterParamete.speed, characterParamete.downSpeed);    //为Player的移动进行赋值
        }
        else if(!characterParamete.isRun)    //如果不是跑动状态，则会用较低的速度移动（包含加速度，减速，以及速度上限）
        {
            if(characterParamete.h != 0)    //不是跑动状态，但玩家按下了左右操作按键，导致h不为0
            {
                characterParamete.speed += characterParamete.h * characterParamete.walkSpeedChangeSpeed * Time.deltaTime;    //会根据h的正负，使用速度变化量修改speed数值，speed用作玩家的移动
                
                characterParamete.speed = Mathf.Clamp(characterParamete.speed , -characterParamete.MAX_SPPED_WALK , characterParamete.MAX_SPPED_WALK);    //将speed控制在正负MAX_SPPED_WALK中
            }
            else    //在玩家没有进行横向移动操作时，执行下行代码
            {
                characterParamete.speed = Mathf.MoveTowards(characterParamete.speed , 0 , characterParamete.dewalkSpeedChangeSpeed * Time.deltaTime);    //每帧参考此帧过去时间，将speed以一定速度向0靠近
            }
            characterParamete.rig.velocity = new Vector2(characterParamete.speed, characterParamete.downSpeed);
        }
        // else    //当玩家处于跑动状态时，以更快的速度进行移动
        // {
        //     if(characterParamete.h != 0)
        //     {
        //         characterParamete.speed += characterParamete.h * characterParamete.runSpeedChangeSpeed * Time.deltaTime;
                
        //         characterParamete.speed = Mathf.Clamp(characterParamete.speed , -characterParamete.MAX_SPPED_RUN , characterParamete.MAX_SPPED_RUN);
        //     }
        //     else
        //     {
        //         characterParamete.speed = Mathf.MoveTowards(characterParamete.speed , 0 , characterParamete.derunSpeedChangeSpeed * Time.deltaTime);
        //     }
        //     characterParamete.rig.velocity = new Vector2(characterParamete.speed, characterParamete.downSpeed);
        // }

        // if (characterParamete.jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))
        // {
        //     characterParamete.jumpc = 1;
        //     characterParamete.downSpeed = 0;
        // }

        if (characterParamete.rig.velocity.x < -0.01)
            manager.transform.localScale = new Vector3(-1, 1, 1);
        else if (characterParamete.rig.velocity.x > 0.01)
            manager.transform.localScale = new Vector3(1, 1, 1);

            
        if (Input.GetButtonDown("Jump") && characterParamete.jumpc > 0)
        {
            // characterParamete.isJump = true;
            // characterParamete.jumpTime = characterParamete.MAX_TIME_JUMP;
            // characterParamete.jumpc --;
            manager.TransitionState(PStateType.Jump);
        }

        if(characterParamete.rig.velocity.y < -0.1)
        {
            manager.TransitionState(PStateType.Jump);
        }

        // if (characterParamete.isJump && characterParamete.jumpTime > 0)
        // {
        //     if(characterParamete.jumpTime >= 0)
        //     {
        //         characterParamete.jumpTime -= Time.deltaTime;
        //         if(characterParamete.jumpTime <= 0)
        //         {
        //             characterParamete.jumpTime = 0;
        //             characterParamete.isJump = false;
        //         }
        //     }
        //     if(Input.GetButtonUp("Jump")) characterParamete.isJump = false;

        //     characterParamete.rig.velocity = new Vector2(characterParamete.rig.velocity.x, 15);
        // }
    }
}

public class PlayerHitedstateact : PState
{
    private CharacterParamete characterParamete;
    private PlayerController1 manager;
    private Character character;
    private OperationListener operationListener;
    private float timer = 0;
    private int direct = 0;
    private AnimatorStateInfo info;
    private float speed;
    private float Yspeed;
    private float speedChangeSpeed = 8;
    Rigidbody2D rig;

    public PlayerHitedstateact(PlayerController1 manager)
    {
        this.manager = manager;
        this.characterParamete = manager.characterParamete;
        this.character = manager.character;
        this.operationListener = manager.operationListener;
    }
    public void OnEnter()
    {
        rig = manager.transform.GetComponent<Rigidbody2D>();

        characterParamete.animator.SetBool("isHited",true);

        characterParamete.isAttack = false;
        
        // paramete.rigidbody.velocity = paramete.direction * paramete.atkspeed;
        rig.velocity = new Vector2(manager.transform.localScale.x * -8 , rig.velocity.y);
        
        speed = rig.velocity.x;
    }

    public void OnUpdate()
    {
        
        // if (characterParamete.jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))
        // {
        //     speed = Mathf.MoveTowards(speed , 0 , speedChangeSpeed * Time.deltaTime);
        // }
        
        speed = Mathf.MoveTowards(speed , 0 , speedChangeSpeed * Time.deltaTime);

        if (rig.velocity.x < 0.2 && rig.velocity.x > -0.2)
        {
            manager.TransitionState(PStateType.IdleAndMove);
        }
        
        rig.velocity = new Vector2(speed, rig.velocity.y);
        
        // if (info.normalizedTime >= .95f)
        // {
        //     manager.TransitionState(EStateType.Chase);
        // }
    }

    public void OnExit()
    {
        characterParamete.isHited = false;
        characterParamete.animator.SetBool("isHited",false);
    }

}
public class PlayerIDashstateact : PState
{
    private CharacterParamete characterParamete;
    private PlayerController1 manager;
    private Character character;
    private OperationListener operationListener;
    private float stopTime = 0.1f;
    private float dashTime = 0.25f;
    private float stopTimer;
    private float dashTimer;
    private float dashSpeed = 20;
    private float dashDirection;
    private float runDirection;
    public PlayerIDashstateact(PlayerController1 manager)
    {
        this.manager = manager;
        this.characterParamete = manager.characterParamete;
        this.character = manager.character;
        this.operationListener = manager.operationListener;
    }
    public void OnEnter()
    {
        // characterParamete.animator.SetBool("isDash",true);       //正式测试版本需要将动画置为可用
        stopTimer = stopTime;
        dashTimer = dashTime;
        manager.cantOperator();
        if(Input.GetKey(KeyCode.RightArrow))
        {
            dashDirection = 1;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            dashDirection = -1;
        }
    }

    public void OnUpdate()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            runDirection = 1;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            runDirection = -1;
        }
        else
        {
            runDirection = 0;
        }

        if(stopTimer > 0)
        {
            stopTimer -= Time.deltaTime;
            characterParamete.speed = characterParamete.speed * 0.6f;
            characterParamete.rig.velocity = new Vector2(characterParamete.speed, 0);
        }
        else if(dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            characterParamete.rig.velocity = new Vector2(dashDirection * dashSpeed, 0);
            characterParamete.speed = manager.transform.localScale.x * 20;
            if(Input.GetKeyDown(KeyCode.Z))
            {
                manager.TransitionState(PStateType.DashAttack);
            }
        }
        else
        {
            if(runDirection == dashDirection)
            {
                manager.TransitionState(PStateType.Run);
            }
            else
            {
                manager.TransitionState(PStateType.IdleAndMove);
            }
        }
        

        // if(dashTimer <= 0)
        // {
        //     if(runDirection == dashDirection)
        //     {
        //         manager.TransitionState(PStateType.Run);
        //     }
        //     else
        //     {
        //         manager.TransitionState(PStateType.IdleAndMove);
        //     }
        // }
        



        // if(stopTimer <= 0 && !characterParamete.isRun)
        // {
        //     manager.runStart(runDirection);
        //     manager.canOperator();
        //     if(!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        //     {
        //         manager.TransitionState(PStateType.IdleAndMove);
        //     }
        // }
        
    }
    
    void enterRunAttack()
    {
        characterParamete.isRunAttack = true;
        characterParamete.isRun = false;
        characterParamete.canOperation = false;
        characterParamete.rightRunTimer = 0;
        characterParamete.leftRunTimer = 0;
        characterParamete.animator.SetBool("isRun",false);
        characterParamete.animator.SetBool("runAttack",true);
    }


    public void OnExit()
    {
        // characterParamete.animator.SetBool("isDash",false);      ////正式测试版本需要将动画置为可用
        manager.canOperator();
    }
}


public class PlayerIRunstateact : PState
{
    private CharacterParamete characterParamete;
    private PlayerController1 manager;
    private Character character;
    private OperationListener operationListener;
    private float stopTime = 0.15f;
    private float timer;
    private float runDirection;
    public PlayerIRunstateact(PlayerController1 manager)
    {
        this.manager = manager;
        this.characterParamete = manager.characterParamete;
        this.character = manager.character;
        this.operationListener = manager.operationListener;
    }
    public void OnEnter()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            runDirection = 1;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            runDirection = -1;
        }
        manager.runStart(runDirection);
    }

    public void OnUpdate()
    {
        // if(timer <= 0 && !characterParamete.isRun)
        // {
        //     manager.runStart(runDirection);
        //     manager.canOperator();
        //     if(!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        //     {
        //         manager.TransitionState(PStateType.IdleAndMove);
        //     }
        // }
        if(characterParamete.isRun)
        {
            Run();
        }

        if( (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) && characterParamete.isRun)
        {
            manager.TransitionState(PStateType.IdleAndMove);
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            manager.TransitionState(PStateType.Attack);
        }
    }

    public void OnExit()
    {
        manager.runEnd();
    }
    void Run()
    {
        characterParamete.downSpeed = characterParamete.rig.velocity.y;
        characterParamete.downSpeed = Mathf.Clamp(characterParamete.downSpeed , -characterParamete.MAX_SPPED_DOWN , characterParamete.MAX_SPPED_UP);
        if (!characterParamete.jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果脚底的碰撞器没有碰到地板层，则会在重力基础上加一个下降速度  【为了优化手感】
        {
            characterParamete.downSpeed -= 0.15f;
        }
        if(!characterParamete.isRun && (characterParamete.speed > 6 || characterParamete.speed < -6))    //如果不是跑动状态但当前速度大于6（冲刺结束后的减速、技能、环境交互因素）则会使用较快的刹车速度
        {
            characterParamete.speed = Mathf.MoveTowards(characterParamete.speed , 0 , characterParamete.dewalkSpeedChangeSpeed * Time.deltaTime);    //将向0减速，与无跑动时相同，在速度为-6搭配6时不再执行此IF

            characterParamete.rig.velocity = new Vector2(characterParamete.speed, characterParamete.downSpeed);    //为Player的移动进行赋值
        }
        else if(!characterParamete.isRun)    //如果不是跑动状态，则会用较低的速度移动（包含加速度，减速，以及速度上限）
        {
            if(characterParamete.h != 0)    //不是跑动状态，但玩家按下了左右操作按键，导致h不为0
            {
                characterParamete.speed += characterParamete.h * characterParamete.walkSpeedChangeSpeed * Time.deltaTime;    //会根据h的正负，使用速度变化量修改speed数值，speed用作玩家的移动
                
                characterParamete.speed = Mathf.Clamp(characterParamete.speed , -characterParamete.MAX_SPPED_WALK , characterParamete.MAX_SPPED_WALK);    //将speed控制在正负MAX_SPPED_WALK中
            }
            else    //在玩家没有进行横向移动操作时，执行下行代码
            {
                characterParamete.speed = Mathf.MoveTowards(characterParamete.speed , 0 , characterParamete.dewalkSpeedChangeSpeed * Time.deltaTime);    //每帧参考此帧过去时间，将speed以一定速度向0靠近
            }
            characterParamete.rig.velocity = new Vector2(characterParamete.speed, characterParamete.downSpeed);
        }
        else    //当玩家处于跑动状态时，以更快的速度进行移动
        {
            if(characterParamete.h != 0)
            {
                characterParamete.speed += characterParamete.h * characterParamete.runSpeedChangeSpeed * Time.deltaTime;
                
                characterParamete.speed = Mathf.Clamp(characterParamete.speed , -characterParamete.MAX_SPPED_RUN , characterParamete.MAX_SPPED_RUN);
            }
            else
            {
                characterParamete.speed = Mathf.MoveTowards(characterParamete.speed , 0 , characterParamete.derunSpeedChangeSpeed * Time.deltaTime);
            }
            characterParamete.rig.velocity = new Vector2(characterParamete.speed, characterParamete.downSpeed);
        }

        if (characterParamete.jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            characterParamete.jumpc = 1;
            characterParamete.downSpeed = 0;
        }

        if (characterParamete.rig.velocity.x < -0.01)
            manager.transform.localScale = new Vector3(-1, 1, 1);
        else if (characterParamete.rig.velocity.x > 0.01)
            manager.transform.localScale = new Vector3(1, 1, 1);

            
        if (Input.GetButtonDown("Jump") && characterParamete.jumpc > 0)
        {
            // characterParamete.isJump = true;
            // characterParamete.jumpTime = characterParamete.MAX_TIME_JUMP;
            // characterParamete.jumpc --;
            manager.TransitionState(PStateType.Jump);
        }

        if (characterParamete.isJump && characterParamete.jumpTime > 0)
        {
            if(characterParamete.jumpTime >= 0)
            {
                characterParamete.jumpTime -= Time.deltaTime;
                if(characterParamete.jumpTime <= 0)
                {
                    characterParamete.jumpTime = 0;
                    characterParamete.isJump = false;
                }
            }
            if(Input.GetButtonUp("Jump")) characterParamete.isJump = false;

            characterParamete.rig.velocity = new Vector2(characterParamete.rig.velocity.x, 15);
        }
    }
}

public class PlayerJumpstateact : PState
{
    private CharacterParamete characterParamete;
    private PlayerController1 manager;
    private Character character;
    private OperationListener operationListener;
    private float jumpTimer;
    private float MAX_TIME_JUMP = 0.15f;
    private bool isUp;
    public PlayerJumpstateact(PlayerController1 manager)
    {
        this.manager = manager;
        this.characterParamete = manager.characterParamete;
        this.character = manager.character;
        this.operationListener = manager.operationListener;
    }
    public void OnEnter()
    {
        if(Input.GetButtonDown("Jump"))
        {
            jumpTimer = MAX_TIME_JUMP;
            characterParamete.jumpc --;
            isUp = true;
        }
        characterParamete.isJump = true;
    }

    public void OnUpdate()
    {
        
        if (!characterParamete.jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果脚底的碰撞器没有碰到地板层，则会在重力基础上加一个下降速度  【为了优化手感】
        {
            characterParamete.downSpeed -= 0.15f;
        }
        JumpMove();
        if (isUp && jumpTimer > 0)
        {
            if(jumpTimer >= 0)
            {
                jumpTimer -= Time.deltaTime;
                if(jumpTimer <= 0)
                {
                    jumpTimer = 0;
                    isUp = false;
                }
            }
            if(Input.GetButtonUp("Jump")) isUp = false;

            characterParamete.rig.velocity = new Vector2(characterParamete.rig.velocity.x, 15);
        }

        if (Input.GetButtonDown("Jump") && characterParamete.jumpc > 0)
        {
            manager.TransitionState(PStateType.Jump);
        }

        if (characterParamete.jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果脚底的碰撞器碰撞到地板层，则会将下落速度置为0，并且重置跳跃次数；
        {
            if(!isUp)
            {
                manager.TransitionState(PStateType.IdleAndMove);
            }
            characterParamete.jumpc = 1;
        }
    }

    public void OnExit()
    {
        characterParamete.isJump = false;
    }
    void JumpMove()
    {
        characterParamete.downSpeed = characterParamete.rig.velocity.y;
        characterParamete.downSpeed = Mathf.Clamp(characterParamete.downSpeed , -characterParamete.MAX_SPPED_DOWN , characterParamete.MAX_SPPED_UP);
        if (!characterParamete.jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果脚底的碰撞器没有碰到地板层，则会在重力基础上加一个下降速度  【为了优化手感】
        {
            characterParamete.downSpeed -= 0.15f;
        }
        if(!characterParamete.isRun && (characterParamete.speed > 6 || characterParamete.speed < -6))    //如果不是跑动状态但当前速度大于6（冲刺结束后的减速、技能、环境交互因素）则会使用较快的刹车速度
        {
            characterParamete.speed = Mathf.MoveTowards(characterParamete.speed , 0 , characterParamete.dewalkSpeedChangeSpeed * Time.deltaTime);    //将向0减速，与无跑动时相同，在速度为-6搭配6时不再执行此IF

            characterParamete.rig.velocity = new Vector2(characterParamete.speed, characterParamete.downSpeed);    //为Player的移动进行赋值
        }
        else if(!characterParamete.isRun)    //如果不是跑动状态，则会用较低的速度移动（包含加速度，减速，以及速度上限）
        {
            if(characterParamete.h != 0)    //不是跑动状态，但玩家按下了左右操作按键，导致h不为0
            {
                characterParamete.speed += characterParamete.h * characterParamete.walkSpeedChangeSpeed * Time.deltaTime;    //会根据h的正负，使用速度变化量修改speed数值，speed用作玩家的移动
                
                characterParamete.speed = Mathf.Clamp(characterParamete.speed , -characterParamete.MAX_SPPED_WALK , characterParamete.MAX_SPPED_WALK);    //将speed控制在正负MAX_SPPED_WALK中
            }
            else    //在玩家没有进行横向移动操作时，执行下行代码
            {
                characterParamete.speed = Mathf.MoveTowards(characterParamete.speed , 0 , characterParamete.dewalkSpeedChangeSpeed * Time.deltaTime);    //每帧参考此帧过去时间，将speed以一定速度向0靠近
            }
            characterParamete.rig.velocity = new Vector2(characterParamete.speed, characterParamete.downSpeed);
        }
        else    //当玩家处于跑动状态时，以更快的速度进行移动
        {
            if(characterParamete.h != 0)
            {
                characterParamete.speed += characterParamete.h * characterParamete.runSpeedChangeSpeed * Time.deltaTime;
                
                characterParamete.speed = Mathf.Clamp(characterParamete.speed , -characterParamete.MAX_SPPED_RUN , characterParamete.MAX_SPPED_RUN);
            }
            else
            {
                characterParamete.speed = Mathf.MoveTowards(characterParamete.speed , 0 , characterParamete.derunSpeedChangeSpeed * Time.deltaTime);
            }
            characterParamete.rig.velocity = new Vector2(characterParamete.speed, characterParamete.downSpeed);
        }

        if (characterParamete.jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            characterParamete.jumpc = 1;
            // characterParamete.downSpeed = 0;
        }

        if (characterParamete.rig.velocity.x < -0.01)
            manager.transform.localScale = new Vector3(-1, 1, 1);
        else if (characterParamete.rig.velocity.x > 0.01)
            manager.transform.localScale = new Vector3(1, 1, 1);

            
        // if (Input.GetButtonDown("Jump") && characterParamete.jumpc > 0)
        // {
        //     characterParamete.isJump = true;
        //     characterParamete.jumpTime = characterParamete.MAX_TIME_JUMP;
        //     characterParamete.jumpc --;
        // }

        // if (characterParamete.isJump && jumpTime > 0)
        // {
        //     if(jumpTime >= 0)
        //     {
        //         jumpTime -= Time.deltaTime;
        //         if(jumpTime <= 0)
        //         {
        //             jumpTime = 0;
        //             characterParamete.isJump = false;
        //         }
        //     }
        //     if(Input.GetButtonUp("Jump")) characterParamete.isJump = false;

        //     characterParamete.rig.velocity = new Vector2(characterParamete.rig.velocity.x, 15);
        // }
    }

}

public class PlayerNormalAttackstateact : PState , PEAttackRange
{
    private CharacterParamete characterParamete;
    private PlayerController1 manager;
    private Character character;
    private OperationListener operationListener;
    private BoxCollider2D attackCllinder;
    private GameObject attackTarget;
    private float MAX_SPPED_ATTACK = 1f;
    private Vector2 fstrepelDeriction = new Vector2( 1 , 0 );
    private Vector2 scdrepelDeriction = new Vector2( 0.5f , 2 );
    private Vector2 thdrepelDeriction = new Vector2( 2 , 0 );
    private Vector2 repelDeriction;
    private float atkTimer;
    public PlayerNormalAttackstateact(PlayerController1 manager)
    {
        this.manager = manager;
        this.characterParamete = manager.characterParamete;
        this.character = manager.character;
        this.operationListener = manager.operationListener;
        atkTimer = 1;
    }
    public void OnEnter()
    {
        // attackCllinder = manager.transform.GetChild(0).gameObject.transform.GetComponents<BoxCollider2D>();
        manager.transform.GetChild(0).gameObject.transform.GetComponent<AttackRange>().getPlayer(this);
        if((characterParamete.speed > 6.5f ||characterParamete.speed < -6.5f) && Input.GetKeyDown(KeyCode.Z) && !characterParamete.isRunAttack)
        {
            manager.TransitionState(PStateType.DashAttack);
        }
        if (Input.GetKeyDown(KeyCode.Z) && !characterParamete.isAttack && !characterParamete.isRunAttack)
        {
            characterParamete.isAttack = true;
            characterParamete.comboStep++;
            if (characterParamete.comboStep > 3)
                characterParamete.comboStep = 1;

            if (characterParamete.comboStep == 1)
                repelDeriction = fstrepelDeriction;
            if (characterParamete.comboStep == 2)
                repelDeriction = scdrepelDeriction;
            if (characterParamete.comboStep == 3)
                repelDeriction = thdrepelDeriction;
                
            if (atkTimer != 0)
            {
                atkTimer -= Time.deltaTime;
                if (atkTimer <= 0)
                {
                    atkTimer = 0;
                    characterParamete.comboStep = 0;
                }
            }

            characterParamete.atkTimer = characterParamete.interval;
            characterParamete.animator.SetTrigger("normalAttack");
            characterParamete.animator.SetInteger("ComboStep", characterParamete.comboStep);
        }
    }

    public void OnUpdate()
    {
        Attack();
        
        if(Input.GetKeyDown(KeyCode.Z) && !characterParamete.isAttack)
        {
            manager.TransitionState(PStateType.Attack);
        }

        if (atkTimer != 0)
        {
            atkTimer -= Time.deltaTime;
            if (atkTimer <= 0)
            {
                atkTimer = 0;
                characterParamete.comboStep = 0;
            }
        }
    }

    public void OnExit()
    {

    }
    void Attack()
    {
        
        if(characterParamete.h != 0)    //当角色在攻击时会使用更低的移动速度
        {
            characterParamete.speed += characterParamete.h * characterParamete.walkSpeedChangeSpeed * Time.deltaTime;    //会根据h的正负，使用速度变化量修改speed数值，speed用作玩家的移动
            
            characterParamete.speed = Mathf.Clamp(characterParamete.speed , -MAX_SPPED_ATTACK , MAX_SPPED_ATTACK);    //将speed控制在正负MAX_SPPED_WALK中
        }
        else    //在玩家没有进行横向移动操作时，执行下行代码
        {
            characterParamete.speed = Mathf.MoveTowards(characterParamete.speed , 0 , characterParamete.dewalkSpeedChangeSpeed * Time.deltaTime);    //每帧参考此帧过去时间，将speed以一定速度向0靠近
        }
        characterParamete.rig.velocity = new Vector2(characterParamete.speed, characterParamete.downSpeed);
        
        
        // if((characterParamete.speed > 6.5f ||characterParamete.speed < -6.5f) && Input.GetKeyDown(KeyCode.Z) && !characterParamete.isRunAttack)
        // {
        //     enterRunAttack();
        // }
        // else 
        if(characterParamete.isRunAttack && !characterParamete.isRun)
        {
            characterParamete.info = characterParamete.animator.GetCurrentAnimatorStateInfo(0);

            if(characterParamete.info.normalizedTime >= 0.95f)
            {
                manager.RunAtkOver();
                manager.AttackOver();
            }
            
            characterParamete.rig.velocity = new Vector2(characterParamete.speed * (1 - characterParamete.info.normalizedTime), 0);
        }
        else
        {
            // if (Input.GetKeyDown(KeyCode.Z) && !characterParamete.isAttack && !characterParamete.isRunAttack)
            // {
            //     characterParamete.isAttack = true;
            //     characterParamete.comboStep++;
            //     if (characterParamete.comboStep > 3)
            //         characterParamete.comboStep = 1;
            //     characterParamete.atkTimer = characterParamete.interval;
            //     characterParamete.animator.SetTrigger("normalAttack");
            //     characterParamete.animator.SetInteger("ComboStep", characterParamete.comboStep);
            // }

            if (characterParamete.atkTimer != 0)
            {
                characterParamete.atkTimer -= Time.deltaTime;
                if (characterParamete.atkTimer <= 0)
                {
                    characterParamete.atkTimer = 0;
                    characterParamete.comboStep = 0;
                }
            }
        }

    }
    void enterRunAttack()
    {
        characterParamete.isRunAttack = true;
        characterParamete.isRun = false;
        characterParamete.canOperation = false;
        characterParamete.rightRunTimer = 0;
        characterParamete.leftRunTimer = 0;
        characterParamete.animator.SetBool("isRun",false);
        characterParamete.animator.SetBool("runAttack",true);
    }
    void getAttackDate()
    {
        AttackSense.Instance.HitPause(characterParamete.normalPause);
        AttackSense.Instance.CameraShake(characterParamete.shakeTime, characterParamete.normalStrength);

        //获取推动力度
        characterParamete.attackForce = character.getAttackForce();

        //获取攻击力
        characterParamete.attack = character.getAttack();

        //获取硬直损伤
        characterParamete.stiffDown = character.getStiffAtk();

            
        if (manager.transform.position.x >= attackTarget.transform.position.x)
            attackTarget.GetComponent<EAtkAndHit>().GetHit(Vector2.left , characterParamete.attackForce , characterParamete.attack , characterParamete.stiffDown , repelDeriction);
        else if (manager.transform.position.x < attackTarget.transform.position.x)
            attackTarget.GetComponent<EAtkAndHit>().GetHit(Vector2.right , characterParamete.attackForce , characterParamete.attack , characterParamete.stiffDown , repelDeriction);
        
    }
    public void returnTarget(Collider2D enemyCollider)
    {
        attackTarget = enemyCollider.gameObject;
        getAttackDate();
    }

}

public class PlayerDashAttackstateact : PState , PEAttackRange
{
    private CharacterParamete characterParamete;
    private PlayerController1 manager;
    private Character character;
    private OperationListener operationListener;
    private BoxCollider2D attackCllinder;
    private GameObject attackTarget;
    private float MAX_SPPED_ATTACK = 1f;
    private Vector2 repelDeriction = new Vector2( 2 , 2 );
    public PlayerDashAttackstateact(PlayerController1 manager)
    {
        this.manager = manager;
        this.characterParamete = manager.characterParamete;
        this.character = manager.character;
        this.operationListener = manager.operationListener;
    }
    public void OnEnter()
    {
        enterRunAttack();
        Debug.Log(characterParamete.speed);
    }

    public void OnUpdate()
    {
        DashAttack();
    }

    public void OnExit()
    {
    }
    void DashAttack()
    {
        characterParamete.info = characterParamete.animator.GetCurrentAnimatorStateInfo(0);

        if(characterParamete.info.normalizedTime >= 0.95f)
        {
            manager.RunAtkOver();
            manager.TransitionState(PStateType.IdleAndMove);
        }
            
        characterParamete.rig.velocity = new Vector2(characterParamete.speed * (1 - characterParamete.info.normalizedTime), 0);
    }
    void enterRunAttack()
    {
        characterParamete.isRunAttack = true;
        characterParamete.isRun = false;
        characterParamete.canOperation = false;
        characterParamete.rightRunTimer = 0;
        characterParamete.leftRunTimer = 0;
        characterParamete.animator.SetBool("isRun",false);
        characterParamete.animator.SetBool("runAttack",true);
    }
    void getAttackDate()
    {
        AttackSense.Instance.HitPause(characterParamete.normalPause);
        AttackSense.Instance.CameraShake(characterParamete.shakeTime, characterParamete.normalStrength);

        //获取推动力度
        characterParamete.attackForce = character.getAttackForce();

        //获取攻击力
        characterParamete.attack = character.getAttack();

        //获取硬直损伤
        characterParamete.stiffDown = character.getStiffAtk();

            
        if (manager.transform.position.x >= attackTarget.transform.position.x)
            attackTarget.GetComponent<EAtkAndHit>().GetHit(Vector2.left , characterParamete.attackForce , characterParamete.attack , characterParamete.stiffDown , repelDeriction);
        else if (manager.transform.position.x < attackTarget.transform.position.x)
            attackTarget.GetComponent<EAtkAndHit>().GetHit(Vector2.right , characterParamete.attackForce , characterParamete.attack , characterParamete.stiffDown , repelDeriction);
        
    }
    public void returnTarget(Collider2D enemyCollider)
    {
        attackTarget = enemyCollider.gameObject;
        getAttackDate();
    }

}
