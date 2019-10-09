using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZMDFQ;

public class Test : MonoBehaviour
{
    Task t;
    TaskCompletionSource<bool> tcs;
    // Start is called before the first frame update
    void Start()
    {
        t = testTask();
        t.ContinueWith((x) =>
        {
            Debug.Log(x.Status);
            Debug.Log("Task end");
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            tcs.TrySetCanceled();
        }
        if (Input.GetKey(KeyCode.A))
        {
            tcs.TrySetResult(true);
        }
    }

    Task testTask()
    {
        tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
        return tcs.Task;
    }
}
