using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveSingleSteeringLauncher : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.GetComponent<CurveSingleSteeringProjectile>().setAngle(Vector2.up);
            bullet.GetComponent<CurveSingleSteeringProjectile>().setTarget(target);
        }
    }
}
