using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZMDFQ;

public class Test : MonoBehaviour
{
    Task task;
    TaskCompletionSource<bool> bo;
    Stack<int> stack;
    // Start is called before the first frame update
    void Start()
    {
        bo = new TaskCompletionSource<bool>();
        //task = TimeManager.Instance.WaitAsync(5);
        bo.Task.ContinueWith((x) => { Debug.Log(12345); });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bo.TrySetResult(true);
        }

    }
}
