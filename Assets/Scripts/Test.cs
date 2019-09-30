using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZMDFQ;

public class Test : MonoBehaviour
{
    public TextAsset t;
    // Start is called before the first frame update
    void Start()
    {
        ConfigManager.Instance.Init();
        var card= ConfigManager.Instance.GetCard<ZMDFQ.Cards.AT_N001>();
        Debug.Log(card.Name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
