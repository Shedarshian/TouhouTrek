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
        var s = "165*8";

        int muti = s.LastIndexOf('*');

        if (muti >= 0)
        {
            string s1 = s.Substring(muti + 1, s.Length - muti - 1);
            Debug.Log(s1);
            s = s.Substring(0, muti);
        }
        Debug.Log(s);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
