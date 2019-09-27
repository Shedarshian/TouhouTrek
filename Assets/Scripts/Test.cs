using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZMDFQ;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SeatByEventSystem eventSystem = new SeatByEventSystem();
        eventSystem.MaxSeat = 8;
        for (int i = 0; i < 8; i++)
        {
            int k = i;
            eventSystem.Register(EventEnum.ActionEnd, i, (x) =>
            {
                Debug.Log($"p{k}的1");
                return Task.CompletedTask;
            });
            eventSystem.Register(EventEnum.ActionEnd, i, (x) =>
            {
                Debug.Log($"p{k}的2");
                return Task.CompletedTask;
            }, 1);
        }
        eventSystem.Register(EventEnum.ActionEnd, -1, (x) =>
        {
            Debug.Log($"官作测试");
            return Task.CompletedTask;
        });
        eventSystem.Call(EventEnum.ActionEnd, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
