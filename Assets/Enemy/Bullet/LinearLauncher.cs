using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearLauncher : MonoBehaviour
{
    protected Transform shootTarget;
    protected Vector3 shootStartPosition;
    protected Vector3 shootEndPosition;
    public GameObject bulletPrefab;
    public float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual protected void fire()
    {
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.transform.up = new Vector2(shootTarget.transform.position.x - transform.position.x,shootTarget.transform.position.y - transform.position.y).normalized;;
        bullet.GetComponent<LinearProjectile>().setProjectileDate(bulletSpeed);
    }
    virtual protected void fire(Vector3 startpst , Vector3 endpst)
    {
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = startpst;
        bullet.transform.up = new Vector2(shootEndPosition.x - shootStartPosition.x,shootEndPosition.y - shootStartPosition.y).normalized;;
        bullet.GetComponent<LinearProjectile>().setProjectileDate(bulletSpeed);
    }

}
