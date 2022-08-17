using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerProjectile : ProjectileFather
{
    private float startAlpha;
    private float endAlpha;
    private LineRenderer line;
    public float fadeSpeed;
    public float sinkSpeed;
    private Vector3 zeroPosition;
    private Vector3 onePosition;

    private void Awake()
    {
        line = transform.GetComponent<LineRenderer>();
        startAlpha = line.startColor.a;
        endAlpha = line.endColor.a;
    }

    private void OnEnable()
    {
        line.startColor = new Color(line.endColor.r , line.endColor.g , line.endColor.b , startAlpha);
        line.endColor = new Color(line.endColor.r , line.endColor.g , line.endColor.b , endAlpha);
        StartCoroutine(ProjectileFade());
    }

    IEnumerator ProjectileFade()
    {
        yield return new WaitForSeconds(0.2f);
        while(line.startColor.a > 0 || line.endColor.a > 0)
        {
            line.startColor = new Color(line.endColor.r , line.endColor.g , line.endColor.b , line.startColor.a - fadeSpeed);
            line.endColor = new Color(line.endColor.r , line.endColor.g , line.endColor.b , line.endColor.a - fadeSpeed);
            transform.position = new Vector3(transform.position.x , transform.position.y - 0.02f , transform.position.z);
            zeroPosition = new Vector3(line.GetPosition(0).x , line.GetPosition(0).y - sinkSpeed , line.GetPosition(0).z);
            onePosition = new Vector3(line.GetPosition(1).x , line.GetPosition(1).y - sinkSpeed , line.GetPosition(1).z);
            line.SetPosition(0,zeroPosition);
            line.SetPosition(1,onePosition);
            yield return new WaitForFixedUpdate();
        }
        ObjectPool.Instance.PushObject(gameObject);
    }

}
