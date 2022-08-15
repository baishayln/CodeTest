using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAttack: MonoBehaviour , AirCondition
{
    float speedMove = 20f;
    float jump = 10;
    float jumpc = 1;
    Animator animator;
    Vector2 jingzhi = new Vector2(0, 0);
    BoxCollider2D jumpcllinder;
    Rigidbody2D rig;
    private float attack;
    private float h;
    private float runh;
    private float j;
    private float attackForce;
    private float stiffDown;
    private bool isSpeedDown;
    public float atkForce = 10;
    private Character character;
    public bool canOperation = true;

    [Header("补偿速度")]
    public float attackSpeed = 2;

    [Header("打击感")]
    public float shakeTime = 0.1f;
    public int normalPause = 6;
    public float normalStrength = 0.015f;
    List<string> playerOperation = new List<string>(3);
    private bool isRolling = false;
    
    [SerializeField] private Vector3 check = new Vector3(0 , -0.88f , 0.15f);

    private Vector2 repelDeriction = new Vector2(1 , 0);    // 击退方向

    // Start is called before the first frame update
    void Start()
    {
        jumpcllinder = transform.GetComponents<BoxCollider2D>()[2];
        rig = transform.GetComponent<Rigidbody2D>();
        animator = transform.GetComponent<Animator>();
        character = transform.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        runh = Input.GetAxisRaw("Horizontal");
        // j = Input.GetAxisRaw("Jump");
        // if(h >= -0.9f && h <= 0.9f) h = h + Input.GetAxis("Horizontal");
        // else h = Input.GetAxis("Horizontal");
        h = Input.GetAxisRaw("Horizontal");
        j = Input.GetAxis("Jump");

        animator.SetFloat("Horizontal", rig.velocity.x);
        animator.SetFloat("Vertical", rig.velocity.y);
        animator.SetBool("isGround", jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")));

        NextRoom();

        Attack();

        Run();

        Move();

    }

    [SerializeField] private float MAX_SPPED_RUN = 15;
    [SerializeField] private float MAX_SPPED_WALK = 6;
    [SerializeField] private float MAX_SPPED_DOWN = 15;
    [SerializeField] private float MAX_SPPED_UP = 90;
    [SerializeField] private float MAX_SPPED_JUMP = 0.15f;
    [SerializeField] private float runSpeedChangeSpeed = 60;
    [SerializeField] private float walkSpeedChangeSpeed = 30;
    [SerializeField] private float runStopSpeedChangeSpeed = 45;
    [SerializeField] private float derunSpeedChangeSpeed = 30;
    [SerializeField] private float dewalkSpeedChangeSpeed = 15;
    [SerializeField] private float speed = 0;
    [SerializeField] private float downSpeed = 0;
    [SerializeField] private float jumpTime = 0;

    // float speedRun = 0;
    // float speedh = 0;
    float speedJump = 0;
    private float runTimer;
    private float runTime = 0.3f;
    private float runTimeSpace;
    private bool isRun = false;
    private bool isJump = false;

    void Move()
    {
        if(canOperation)
        {
            downSpeed = rig.velocity.y;
            downSpeed = Mathf.Clamp(downSpeed , -MAX_SPPED_DOWN , MAX_SPPED_UP);
            if (!jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))    //如果脚底的碰撞器没有碰到地板层，则会在重力基础上加一个下降速度  【为了优化手感】
            {
                downSpeed -= 0.15f;
            }
            if(!isRun && (speed > 6 || speed < -6))    //如果不是跑动状态但当前速度大于6（冲刺结束后的减速、技能、环境交互因素）则会使用较快的刹车速度
            {
                speed = Mathf.MoveTowards(speed , 0 , dewalkSpeedChangeSpeed * Time.deltaTime);    //将向0减速，与无跑动时相同，在速度为-6搭配6时不再执行此IF

                rig.velocity = new Vector2(speed, downSpeed);    //为Player的移动进行赋值
            }
            else if(!isRun)    //如果不是跑动状态，则会用较低的速度移动（包含加速度，减速，以及速度上限）
            {
                if(h != 0)    //不是跑动状态，但玩家按下了左右操作按键，导致h不为0
                {
                    speed += h * walkSpeedChangeSpeed * Time.deltaTime;    //会根据h的正负，使用速度变化量修改speed数值，speed用作玩家的移动
                    
                    speed = Mathf.Clamp(speed , -MAX_SPPED_WALK , MAX_SPPED_WALK);    //将speed控制在正负MAX_SPPED_WALK中
                }
                else    //在玩家没有进行横向移动操作时，执行下行代码
                {
                    speed = Mathf.MoveTowards(speed , 0 , dewalkSpeedChangeSpeed * Time.deltaTime);    //每帧参考此帧过去时间，将speed以一定速度向0靠近
                }
                rig.velocity = new Vector2(speed, downSpeed);
            }
            else    //当玩家处于跑动状态时，以更快的速度进行移动
            {
                if(h != 0)
                {
                    speed += h * runSpeedChangeSpeed * Time.deltaTime;
                    
                    speed = Mathf.Clamp(speed , -MAX_SPPED_RUN , MAX_SPPED_RUN);
                }
                else
                {
                    speed = Mathf.MoveTowards(speed , 0 , derunSpeedChangeSpeed * Time.deltaTime);
                }
                rig.velocity = new Vector2(speed, downSpeed);
            }

            if (jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                jumpc = 1;
                downSpeed = 0;
            }

            if (rig.velocity.x < -0.01)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (rig.velocity.x > 0.01)
                transform.localScale = new Vector3(1, 1, 1);

            
            if (Input.GetButtonDown("Jump") && jumpc > 0)
            {
                isJump = true;
                jumpTime = MAX_SPPED_JUMP;
                jumpc --;
            }

            if (isJump && jumpTime > 0)
            {
                if(jumpTime >= 0)
                {
                    jumpTime -= Time.deltaTime;
                    if(jumpTime <= 0)
                    {
                        jumpTime = 0;
                        isJump = false;
                    }
                }
                if(Input.GetButtonUp("Jump")) isJump = false;

                rig.velocity = new Vector2(rig.velocity.x, 15);
            }

        }
        


        //滑行

        //if(h!=0)
        //{
        //    rig.velocity = new Vector2(h * speedMove, rig.velocity.y);
        //}


        //【前任横向移动代码】

        // if(!isRun)
        // {
        //     rig.velocity = new Vector2(h * speedMove, rig.velocity.y);
        // }

        // if(isRun)
        // {
        //     rig.velocity = new Vector2(runh * speedRun, rig.velocity.y);
        // }
    }

    
    private bool isAttack;
    private int comboStep;
    private float atkTimer;
    private bool isRunAttack;
    private float runAtkTimer;
    public float runAtkInterval = 0.5f;
    public float interval = 1f;
    private AnimatorStateInfo info;
    private float anmtfloat;
    void Attack()
    {
        
        if((speed > 6.5f ||speed < -6.5f) && Input.GetKeyDown(KeyCode.Z) && !isRunAttack)
        {
            isRunAttack = true;
            isRun = false;
            canOperation = false;
            rightRunTimer = 0;
            leftRunTimer = 0;
            animator.SetBool("isRun",false);
            animator.SetBool("runAttack",true);
        }
        else if(isRunAttack && !isRun)
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
            // if(info.normalizedTime > 1) 
            //     anmtfloat = 0;
            // else
            //     anmtfloat = info.normalizedTime;

            // Debug.Log(anmtfloat);
            // Debug.Log(info.normalizedTime);

            if(info.normalizedTime >= 0.95f)
            {
                RunAtkOver();
            }
            
            rig.velocity = new Vector2(speed * (1 - info.normalizedTime), 0);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Z) && !isAttack && !isRunAttack)
            {
                isAttack = true;
                comboStep++;
                if (comboStep > 3)
                    comboStep = 1;
                atkTimer = interval;
                animator.SetTrigger("normalAttack");
                animator.SetInteger("ComboStep", comboStep);
            }

            if (atkTimer != 0)
            {
                atkTimer -= Time.deltaTime;
                if (atkTimer <= 0)
                {
                    atkTimer = 0;
                    comboStep = 0;
                }
            }
        }

    }

    private float rightRunTimer;
    private float leftRunTimer;
    void Run()
    {
         // if((Input.GetKeyDown(KeyCode.RightArrow) && rig.velocity.x < 0 && isRun) || (Input.GetKeyDown(KeyCode.LeftArrow) && rig.velocity.x > 0 && isRun) || ((!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && isRun)))
        if(canOperation)
        {
            if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                isRun = false;
                runTimeSpace = 2f;
                animator.SetBool("isRun",false);
            }

            if(runTimeSpace >= 0)
            {
                runTimeSpace -= Time.deltaTime;
                if(runTimeSpace <= 0)
                {
                    runTimeSpace = 0;
                }
            }
            if(rightRunTimer >= 0)
            {
                rightRunTimer -= Time.deltaTime;
                if(rightRunTimer <= 0)
                {
                    rightRunTimer = 0;
                }
            }
            if(leftRunTimer >= 0)
            {
                leftRunTimer -= Time.deltaTime;
                if(leftRunTimer <= 0)
                {
                    leftRunTimer = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && rightRunTimer > 0)
            {
                RunAtkOver();
                isRun = true;
                speed = 10;
                animator.SetBool("isRun",true);
                AttackOver();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                rightRunTimer = runTime;
                leftRunTimer = 0;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && leftRunTimer > 0)
            {
                RunAtkOver();
                isRun = true;
                speed = -10;
                animator.SetBool("isRun",true);
                AttackOver();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                leftRunTimer = runTime;
                rightRunTimer = 0;
            }
        }
        
    }


    public void AttackOver()
    {
        isAttack = false;
    }

    public void RunAtkOver()
    {
        isRunAttack = false;
        speed = 0;
        animator.SetBool("runAttack",false);
    }
    public void cantOperator()
    {
        canOperation = true;
    }
    public void canOperator()
    {
        canOperation = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            AttackSense.Instance.HitPause(normalPause);
            AttackSense.Instance.CameraShake(shakeTime, normalStrength);
            
            // if (transform.localScale.x > 0)
            //     other.GetComponent<testEnemyController>().GetHit(Vector2.right);
            // else if (transform.localScale.x < 0)
            //     other.GetComponent<testEnemyController>().GetHit(Vector2.left);

            // attackForce = transform.GetComponent<Character>().getAttackForce();
            // other.GetComponent<testEnemyController>().takeAtkSpeed(attackForce);

            // //获取攻击力并造成伤害
            // attack = transform.GetComponent<Character>().getAttack();
            // other.GetComponent<testEnemyController>().takeDamage(attack);


            //获取推动力度
            attackForce = character.getAttackForce();

            //获取攻击力
            attack = character.getAttack();

            //获取硬直损伤
            stiffDown = character.getStiffAtk();

            
            if (transform.position.x >= other.transform.position.x)
                other.GetComponent<EAtkAndHit>().GetHit(Vector2.left , attackForce , attack , stiffDown , repelDeriction);
            else if (transform.position.x < other.transform.position.x)
                other.GetComponent<EAtkAndHit>().GetHit(Vector2.right , attackForce , attack , stiffDown , repelDeriction);


            //获取推动力度并计算推动距离
            // attackForce = transform.GetComponent<Character>().getAttackForce();
            // other.GetComponent<Enemycontroller1>().takeAtkSpeed(attackForce);

            // //获取攻击力并造成伤害
            // attack = transform.GetComponent<Character>().getAttack();
            // other.GetComponent<Enemycontroller1>().takeDamage(attack);


            //获取架势损伤并减少架势条
            //attack = transform.GetComponent<Character>().getAttack();
            //other.GetComponent<EnemyController>().stiffDown(attack);

            //获取击退力度并调用受击单位的击退函数
            
        }
        
        if(other.CompareTag("DeadLine"))
        {
            character.DeathLine();
            if(character.getHealth() == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(check.x, check.y, 0), check.z);
    }

    
    private GameObject playerBoth;

    void NextRoom()
    {
        if(Globle.getIsGoRoom())
        {
            playerBoth = GameObject.Find ("PlayerBothPoint");
            transform.position = playerBoth.transform.position;
            Globle.inTheNextRoom();
        }
    }

    public bool getIsRolling()
    {
        return isRolling;
    }
    
}
