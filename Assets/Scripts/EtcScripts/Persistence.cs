using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistence : MonoBehaviour
{
    public static Persistence thisUI;

    private void Awake()
    {
        if (thisUI == null)
        {
            thisUI = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
