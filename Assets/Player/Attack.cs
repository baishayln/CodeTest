using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject putongAttack;
    public Transform attackHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject Atk = GameObject.Instantiate(putongAttack, attackHand.position, attackHand.transform.rotation);
        }
    }
}
