using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerLauncher : LinearLauncher
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // protected override void fire()
    // {
    //     base.fire();
    // }

    protected override void fire(Vector3 startpst, Vector3 endpst)
    {
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        LineRenderer line = bullet.GetComponent<LineRenderer>();
        line.SetPosition(0,startpst);
        line.SetPosition(1,endpst);
    }

}
