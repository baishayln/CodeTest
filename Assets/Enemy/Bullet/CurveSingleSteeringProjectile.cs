using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveSingleSteeringProjectile : MonoBehaviour
{
    private Vector3 initialAngle;
    private Vector3 targetAngle;
    public float angleSpeed;
    public float steerTime;
    private float steerTimer;
    public float firstInitialSpeed;
    public float firstSpeedMinxProport;
    public float firstTargetDistance;  //未使用
    public float firstBaseSpeed;  //未使用
    public float secondInitialSpeed;
    public float secondSpeedUpLimit;
    public float secondAcceleratedVelocity;  //二阶段加速度
    public float trackingTime;
    private float trackingTimer;
    private float secondPhaseTimer;
    private float speed;
    private float deadTime = 5;
    private float deadTimer;
    private Rigidbody2D rig;
    private bool arrived;
    private float lerp;
    public GameObject target;
    private GameObject sight;
    void Awake()
    {
        rig = transform.GetComponent<Rigidbody2D>();
        sight = new GameObject("Sight");
        sight.transform.SetParent(transform);
        // sight.transform.SetParent(transform.Find("SurrounderPool"));
    }
    void OnEnable()
    {
        deadTimer = deadTime;
        steerTimer = steerTime;
        secondPhaseTimer = 0;
        arrived = false;
        trackingTimer = trackingTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        steerTimer -= Time.deltaTime;
        if (steerTimer >= 0)
            FirstPhase();
        else
            SecondPhase();
        
        deadTimer -= Time.deltaTime;
        if (deadTimer < 0)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
    private void FirstPhase()
    {
        speed = (steerTimer/steerTime + firstSpeedMinxProport) * firstInitialSpeed;
        lerp = steerTimer/steerTime;
        transform.up = Vector3.Slerp(targetAngle , initialAngle , lerp);
        rig.velocity = transform.up * speed;
    }
    
    private void SecondPhase()
    {
        secondPhaseTimer += Time.deltaTime;
        if (speed < secondSpeedUpLimit)
            speed += Time.deltaTime * secondAcceleratedVelocity * (float)(1 + secondPhaseTimer/0.5);
        else if (speed > secondSpeedUpLimit)
            speed = secondSpeedUpLimit;

        if(trackingTimer > 0)
        {
            sight.transform.up = new Vector3(target.transform.position.x - transform.position.x , target.transform.position.y - transform.position.y , target.transform.position.z - transform.position.z);
            trackingTimer -= Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation , sight.transform.rotation , angleSpeed);
            rig.velocity = transform.up * speed;
        }

    }

    public void setAngle(Vector3 iniangle)
    {
        initialAngle = iniangle;
        transform.up = initialAngle;
        targetAngle = Vector3.Slerp(initialAngle , transform.right , 2/3f);
    }
    
    public void setTarget(GameObject tgt)
    {
        target = tgt;
    }
    
}
