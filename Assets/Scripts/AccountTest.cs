using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine.UI;

public class AccountTest : MonoBehaviour
{
    public TextMeshProUGUI userID;
    public string userAccount;

    public void SetUserID()
    {
        userID.text = StoreUserID.instance.userID;
    }
}
