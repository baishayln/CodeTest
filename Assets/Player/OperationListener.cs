using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operation
{
    public string operationName;
    public float timer;
    public Operation()
    {
    }
    public Operation(string name , float time)
    {
        operationName = name;
        timer = time;
    }
    public void setOperation(string name , float time)
    {
        operationName = name;
        timer = time;
    }
}

public class OperationListener : MonoBehaviour
{
    private float chainTime = 1f;
    List<Operation> opeartionHistory = new List<Operation>();
    private Operation[] operations = new Operation[3];
    private Operation empty = new Operation();
    // Start is called before the first frame update
    void Start()
    {
        operations[0] = new Operation("wdnmd" , chainTime);
        operations[1] = new Operation("wdnmd" , chainTime);
        operations[2] = new Operation("wdnmd" , chainTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(operations[0].timer > 0)
        {
            operations[0].timer -= Time.deltaTime;
            if(operations[0].timer <= 0)
            {
                operations[0].timer = 0;
            }
        }
        if(operations[1].timer > 0)
        {
            operations[1].timer -= Time.deltaTime;
            if(operations[1].timer <= 0)
            {
                operations[1].timer = 0;
            }
        }
        if(operations[2].timer > 0)
        {
            operations[2].timer -= Time.deltaTime;
            if(operations[2].timer <= 0)
            {
                operations[2].timer = 0;
            }
        }

        // if(Input.anyKeyDown)
        // {
        //     empty = operations[2];
        //     operations[2] = operations[1];
        //     operations[1] = operations[0];
        //     operations[0] = empty;

        //     if(Input.GetKeyDown(KeyCode.Z))
        //     {
        //         operations[0].setOperation("Z" , chainTime);
        //     }
        //     if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        //     {
        //         operations[0].setOperation("Forward" , chainTime);
        //     }
        //     if(Input.GetKeyDown(KeyCode.DownArrow))
        //     {
        //         operations[0].setOperation("Down" , chainTime);
        //     }
        //     if(Input.GetKeyDown(KeyCode.UpArrow))
        //     {
        //         operations[0].setOperation("Up" , chainTime);
        //     }
        // }
    }

    public void recordOperation()
    {
        // Operation operation = new Operation("wdnmd" , chainTime);
        // opeartionHistory.Add(operation);
        // opeartionHistory.Add(new Operation("wdnmd" , chainTime));

        empty = operations[2];
        operations[2] = operations[1];
        operations[1] = operations[0];
        operations[0] = empty;

        if(Input.GetKeyDown(KeyCode.Z))
        {
            operations[0].setOperation("Z" , chainTime);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            operations[0].setOperation("RightForward" , chainTime);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            operations[0].setOperation("LeftForward" , chainTime);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            operations[0].setOperation("Down" , chainTime);
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            operations[0].setOperation("Up" , chainTime);
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            operations[0].setOperation("Jump" , chainTime);
        }
        else
        {
            operations[0].setOperation("Wdnmd" , chainTime);
        }

        // Debug.Log("有什么不对！一定是玩家按了什么按键！");
        // Debug.Log(operations[0].timer);
        // Debug.Log(operations[0].operationName);
        // Debug.Log(operations[1].timer);
        // Debug.Log(operations[1].operationName);
        // Debug.Log(operations[2].timer);
        // Debug.Log(operations[2].operationName);
    }
    public float getFirstTimer()
    {
        return operations[0].timer;
    }
    public float getSecondTimer()
    {
        return operations[1].timer;
    }
    public float getThirdTimer()
    {
        return operations[2].timer;
    }
    public string getFirstOperation()
    {
        return operations[0].operationName;
    }
    public string getSecondOperation()
    {
        return operations[1].operationName;
    }
    public string getThirdOperation()
    {
        return operations[2].operationName;
    }
    public void clearHistory()
    {
            operations[0].setOperation("wdnmd" , 0);
    }
}
