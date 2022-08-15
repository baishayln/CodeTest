using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum testPStateType
{
    IdleAndMove, Run, Jump, Chase, React, Attack, Hit, Death, Dash ,Hited ,Break ,DashPrepare ,DashAttack
}

[Serializable]
public class testCharacterParamete
{
    [Header("移动属性，移动判断属性")]
    public float speedMove = 20f;
    public float jump = 10;
    public float jumpc = 1;
    public Animator animator;
    public Vector2 jingzhi = new Vector2(0, 0);
    public BoxCollider2D jumpcllinder;
    public Rigidbody2D rig;
    public float attack;
    public float h;
    public float runh;
    public float j;
    public float attackForce;
    public float stiffDown;
    public bool isSpeedDown;
    public float atkForce = 10;
    public Character character;
    public OperationListener operationListener;
    public bool canOperation = true;
    public Vector2 direction;


    [Header("补偿速度")]
    public float attackSpeed = 2;

    [Header("打击感")]
    public float shakeTime = 0.1f;
    public int normalPause = 6;
    public float normalStrength = 0.015f;
    public List<string> playerOperation = new List<string>(3);
    
    [SerializeField] 
    public Vector3 check = new Vector3(0 , -0.88f , 0.15f);
    

    [Header("移动属性")]
    [SerializeField] public float MAX_SPPED_RUN = 15;
    [SerializeField] public float MAX_SPPED_WALK = 9;
    [SerializeField] public float MAX_SPPED_DOWN = 15;
    [SerializeField] public float MAX_SPPED_UP = 90;
    [SerializeField] public float MAX_TIME_JUMP = 0.15f;
    [SerializeField] public float runSpeedChangeSpeed = 60;
    [SerializeField] public float walkSpeedChangeSpeed = 30;
    [SerializeField] public float runStopSpeedChangeSpeed = 45;
    [SerializeField] public float derunSpeedChangeSpeed = 45;
    [SerializeField] public float dewalkSpeedChangeSpeed = 60;
    [SerializeField] public float speed = 0;
    [SerializeField] public float downSpeed = 0;
    [SerializeField] public float jumpTime = 0;
    public float speedJump = 0;
    public float runTimer;
    public float runTime = 0.3f;
    public float runTimeSpace;


    [Header("状态判断")]
    public bool isRolling = false;
    public bool isRun = false;
    public bool isJump = false;
    public bool isHited = false;

    [Header("玩家位置")]
    public GameObject playerBoth;

    [Header("攻击属性")]
    public bool isAttack;
    public int comboStep;
    public float atkTimer;
    public bool isRunAttack;
    public float runAtkTimer;
    public float runAtkInterval = 0.5f;
    public float interval = 1f;
    public AnimatorStateInfo info;
    public float anmtfloat;

    [Header("Run状态判定")]
    public float rightRunTimer;
    public float leftRunTimer;

}

public class testController : MonoBehaviour , EAtkAndHit , AirCondition
{
    private PState currentState;
    private Dictionary<PStateType, PState> states = new Dictionary<PStateType, PState>();

    public CharacterParamete characterParamete;
    public Character character;
    public OperationListener operationListener;
    public AudioSource hitedSound;
    public Text trueHealth;
    public GameObject HealthBarIn;
    public GameObject gameOver;
    public Text roomCount;
    private bool start = false;
    private bool cplt = false;
    private float alpha = 0.9f;
    private float timer;
    private GameObject zhuanchang;

    // Start is called before the first frame update
    void Start()
    {

        characterParamete.rig = transform.GetComponent<Rigidbody2D>();
        characterParamete.jumpcllinder = transform.GetComponents<BoxCollider2D>()[2];
        characterParamete.animator = transform.GetComponent<Animator>();
        operationListener = transform.GetComponent<OperationListener>();
        character = transform.GetComponent<Character>();

        // states.Add(PStateType.IdleAndMove, new PlayerIdleMovestateact(this));
        // states.Add(PStateType.Dash, new PlayerIDashstateact(this));
        // states.Add(PStateType.Run, new PlayerIRunstateact(this));
        // states.Add(PStateType.Jump, new PlayerJumpstateact(this));
        // states.Add(PStateType.Attack, new PlayerNormalAttackstateact(this));
        // states.Add(PStateType.DashAttack, new PlayerDashAttackstateact(this));
        // states.Add(PStateType.Hited, new PlayerHitedstateact(this));
        // states.Add(EStateType.Dash, new (this));
        // states.Add(EStateType.Hited, new (this));
        // states.Add(EStateType.Break, new (this));
        // states.Add(EStateType.Death, new (this));
        // states.Add(EStateType.Chase, new (this));
        // states.Add(EStateType.DashPrepare, new (this));

        TransitionState(PStateType.IdleAndMove);

        hitedSound = GetComponents<AudioSource>()[0];
        
        zhuanchang = GameObject.Find ("zhuanchang");



    }

    // Update is called once per frame
    void Update()
    {
        characterParamete.runh = Input.GetAxisRaw("Horizontal");

        characterParamete.h = Input.GetAxisRaw("Horizontal");
        characterParamete.j = Input.GetAxis("Jump");

        characterParamete.animator.SetFloat("Horizontal", characterParamete.rig.velocity.x);
        characterParamete.animator.SetFloat("Vertical", characterParamete.rig.velocity.y);
        characterParamete.animator.SetBool("isGround", characterParamete.jumpcllinder.IsTouchingLayers(LayerMask.GetMask("Ground")));

        if(Input.anyKeyDown && characterParamete.canOperation)
        {
            // Debug.Log("监测玩家操作");
            operationListener.recordOperation();
        }

        // if(characterParamete.canOperation)
        // {
        //     Debug.Log("默认回归Idle");
        //     TransitionState(PStateType.IdleAndMove);
        // }

        if (characterParamete.isHited)
        {
            TransitionState(PStateType.Hited);

            // characterParamete.rigidbody.velocity = characterParamete.direction * characterParamete.speed;
            // characterParamete.isHit = false;

            //if (info.normalizedTime >= .6f)
            //    isHit = false;
        }
        currentState.OnUpdate();
        
        NextRoom();
        
        if (alpha > 0)
        {
            alpha -= 0.04f;
            if (alpha < 0)
            {
                alpha = 0;
            }
            zhuanchang.GetComponent<Image>().color = new Color(0 , 0 , 0 , alpha);
        }

    }

    public void TransitionState(PStateType state)
    {
        // Debug.Log("我正在切换状态");

        if(currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[state];
        currentState.OnEnter();
        
        // Debug.Log("当前状态为" + currentState);
        // if(currentState != states[state])
        // {
            
        // }
    }
    
    // Character中进行所有数值计算操作，此脚本只执行移动等指令
    // public virtual void takeDamage(float dmg)
    // {
    //     if(dmg<=character.def)
    //     {
    //         characterParamete.trueDamage=1;
    //         characterParamete.health -= characterParamete.trueDamage;
    //     }
    //     else
    //         characterParamete.health -=dmg;
    //     if (characterParamete.health <= 0)
    //     {
    //         Die();
    //     }
    //     attacked();
    // }

    // 在Character中有了可返回速度的函数，所以此函数不启用
    // public virtual void takeAtkSpeed(float force)
    // {
    //     characterParamete.atkspeed = force/characterParamete.weight;
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("DeadLine"))
        {
            character.DeathLine();
            if(character.getHealth() == 0)
            {
                gameOver.SetActive(true);
                roomCount.text = Globle.getRoom().ToString();
                Destroy(gameObject);
            }
        }
    }


    public virtual void Die()
    {
        TransitionState(PStateType.Death);
    }

    public virtual void pofang()
    {
        TransitionState(PStateType.Break);
    }

    public virtual void attacked()
    {

    }

    public virtual void Destroythis()
    {
        gameOver.SetActive(true);
        roomCount.text = Globle.getRoom().ToString();
        Destroy(gameObject);
    }
    public void GetHit(Vector2 direction , float force , float dmg , float stfdmg , Vector2 repelDirection)
    {
        transform.localScale = new Vector3(-direction.x, 1, 1);
        characterParamete.isHited = true;
        this.characterParamete.direction = direction;
        character.takeDamage(dmg);
        hitedSound.Play();
        trueHealth.text = character.getHealth().ToString() + "/" + 25;
        HealthBarIn.GetComponent<Image>().fillAmount = (float)character.getHealth() / (float)25;
    }
    void NextRoom()
    {
        if(Globle.getIsGoRoom())
        {
            characterParamete.playerBoth = GameObject.Find ("PlayerBothPoint");
            transform.position = characterParamete.playerBoth.transform.position;
            Globle.inTheNextRoom();
            start = true;
            alpha = 0.95f;
        }
    }
    
    public void AttackOver()    //此次攻击彻底结束，改变当前状态为IdleAndMove
    {
        TransitionState(PStateType.IdleAndMove);
    }
    public void AttackPreInput()    //这个函数在动画中执行，执行后可以进行预输入
    {
        characterParamete.isAttack = false;
    }

    public void RunAtkOver()
    {
        characterParamete.isRunAttack = false;
        characterParamete.speed = 0;
        characterParamete.animator.SetBool("runAttack",false);
        characterParamete.isRunAttack = false;
        characterParamete.canOperation = true;
    }
    public void cantOperator()
    {
        characterParamete.canOperation = false;
    }
    public void canOperator()
    {
        characterParamete.canOperation = true;
    }

    public bool getIsRolling()
    {
        return characterParamete.isRolling;
    }

    public void runStart(float runDirection)
    {
        characterParamete.isRun = true;
        if(runDirection > 0)
        {
            characterParamete.speed = 15;
        }
        else
        {
            characterParamete.speed = -15;
        }
    }

    public void runEnd()
    {
        characterParamete.isRun = false;
    }

}
