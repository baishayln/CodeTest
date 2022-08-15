using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    public GameObject target;
    private float camSpeed = 0;
    [SerializeField]
    private float spdDownSpd = 0.06f;
    private float MINSpd = 0;
    [SerializeField]
    private float XMAX = 3;
    [SerializeField]
    private float YMAX = 1.8f;
    private float Xr = 3;
    private float Yr = 1.8f;
    private float juli;
    private float SELFX;
    private float SELFY;
    private bool isShake;
    private bool isPause;
    private bool wdnmd;
    private Vector3 shouleBePosition;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!target)
        {
            target = GameObject.Find("Camera Rack");
        }
        shouleBePosition = transform.position;
    }

    void FixedUpdate()
    {
        SELFX = shouleBePosition.x;
        SELFY = shouleBePosition.y;
        if (Mathf.Abs(target.transform.position.x - SELFX) > Mathf.Abs((target.transform.position.y - SELFY)/0.6f))
        {
            juli = Mathf.Abs(target.transform.position.x - SELFX);
        }
        else
        {
            juli = Mathf.Abs((target.transform.position.y - SELFY)/0.6f);
        }

        camSpeed = juli * 5;

        shouleBePosition = Vector3.MoveTowards(shouleBePosition,
            target.transform.position, camSpeed * Time.deltaTime);

        if (!isPause || !isShake)
        {
            transform.position = shouleBePosition;
        }

    }

    public void HitPause(int duration)
    {
        StartCoroutine(Pause(duration));
    }

    IEnumerator Pause(int duration)
    {
        isPause = true;
        float pauseTime = duration / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
        isPause = false;
        // yield return wdnmd = new AsyncOperation().isDone;
    }

    public void CameraShake(float duration, float strength)
    {
        if (!isShake)
            StartCoroutine(Shake(duration, strength));
    }

    IEnumerator Shake(float duration, float strength)
    {
        isShake = true;
        Transform camera = Camera.main.transform;

        while (duration > 0)
        {
            camera.position = Random.insideUnitSphere * Random.Range(strength , strength/2) + shouleBePosition;
            duration -= Time.deltaTime;
            yield return null;
        }
        camera.position = shouleBePosition;
        isShake = false;
    }
}

