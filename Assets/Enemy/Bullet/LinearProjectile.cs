using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : ProjectileFather
{
    private float projectileSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * projectileSpeed; 
    }
    public void setProjectileDate(float projectileSpd)
    {
        projectileSpeed = projectileSpd;
    }

}
