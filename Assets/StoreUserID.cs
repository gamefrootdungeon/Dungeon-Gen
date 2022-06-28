using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUserID : MonoBehaviour
{
    public string userID ="";
    public bool isLoggedin = false;
    public static StoreUserID instance;
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }
}
