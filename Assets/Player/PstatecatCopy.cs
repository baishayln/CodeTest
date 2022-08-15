using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PstateactCopy : EState
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


public class PlayerIdleMovestateactCopy : PState
{
    private CharacterParamete characterParamete;
    private PlayerController1 manager;
    public PlayerIdleMovestateactCopy(PlayerController1 manager)
    {
        this.manager = manager;
        this.characterParamete = manager.characterParamete;
    }
    public void OnEnter()
    {
    }

    public void OnUpdate()
    {
        Move();
    }

    public void OnExit()
    {

    }
    void Move()
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
            characterParamete.isJump = true;
            characterParamete.jumpTime = characterParamete.MAX_TIME_JUMP;
            characterParamete.jumpc --;
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


public class PlayerJumpstateactCopy : PState
{
    private CharacterParamete characterParamete;
    private PlayerController1 manager;
    public PlayerJumpstateactCopy(PlayerController1 manager)
    {
        this.manager = manager;
        this.characterParamete = manager.characterParamete;
    }
    public void OnEnter()
    {
    }

    public void OnUpdate()
    {
        Jump();
    }

    public void OnExit()
    {

    }
    void Jump()
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
            characterParamete.isJump = true;
            characterParamete.jumpTime = characterParamete.MAX_TIME_JUMP;
            characterParamete.jumpc --;
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

public class PlayerNormalAttackstateactCopy : PState , PEAttackRange
{
    private CharacterParamete characterParamete;
    private PlayerController1 manager;
    private Character character;
    private BoxCollider2D attackCllinder;
    private GameObject attackTarget;
    private Vector2 repelDeriction = new Vector2( 0.5f , 1 );
    public PlayerNormalAttackstateactCopy(PlayerController1 manager)
    {
        this.manager = manager;
        this.characterParamete = manager.characterParamete;
    }
    public void OnEnter()
    {
        // attackCllinder = manager.transform.GetChild(0).gameObject.transform.GetComponents<BoxCollider2D>();
        manager.transform.GetChild(0).gameObject.transform.GetComponent<AttackRange>().getPlayer(this);
    }

    public void OnUpdate()
    {
        Attack();
    }

    public void OnExit()
    {

    }
    void Attack()
    {
        
        if((characterParamete.speed > 6.5f ||characterParamete.speed < -6.5f) && Input.GetKeyDown(KeyCode.Z) && !characterParamete.isRunAttack)
        {
            enterRunAttack();
        }
        else if(characterParamete.isRunAttack && !characterParamete.isRun)
        {
            characterParamete.info = characterParamete.animator.GetCurrentAnimatorStateInfo(0);

            if(characterParamete.info.normalizedTime >= 0.95f)
            {
                manager.RunAtkOver();
            }
            
            characterParamete.rig.velocity = new Vector2(characterParamete.speed * (1 - characterParamete.info.normalizedTime), 0);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Z) && !characterParamete.isAttack && !characterParamete.isRunAttack)
            {
                characterParamete.isAttack = true;
                characterParamete.comboStep++;
                if (characterParamete.comboStep > 3)
                    characterParamete.comboStep = 1;
                characterParamete.atkTimer = characterParamete.interval;
                characterParamete.animator.SetTrigger("normalAttack");
                characterParamete.animator.SetInteger("ComboStep", characterParamete.comboStep);
            }

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
            characterParamete.attackForce = characterParamete.character.getAttackForce();

            //获取攻击力
            characterParamete.attack = character.getAttack();

            //获取硬直损伤
            characterParamete.stiffDown = characterParamete.character.getStiffAtk();

            
            if (manager.transform.position.x >= attackTarget.transform.position.x)
                attackTarget.GetComponent<EAtkAndHit>().GetHit(Vector2.left , characterParamete.attackForce , characterParamete.attack , characterParamete.stiffDown , repelDeriction);
            else if (manager.transform.position.x < attackTarget.transform.position.x)
                attackTarget.GetComponent<EAtkAndHit>().GetHit(Vector2.right , characterParamete.attackForce , characterParamete.attack , characterParamete.stiffDown , repelDeriction);

    }
    public void returnTarget(Collider2D enemyCollider)
    {
        attackTarget = enemyCollider.gameObject;
    }

}
