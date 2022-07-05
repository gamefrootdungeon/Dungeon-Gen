using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DoorType
{
    EntranceDoor,
    ExitDoor
}
public class SpecialDoorTrigger : MonoBehaviour
{
    public DoorType doorType;
    public Vector2Int postion;
    [SerializeField] private UIManager uiManager;
    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (doorType)
            {
                case DoorType.EntranceDoor:
                    print("Entrance Door");
                    break;
                case DoorType.ExitDoor:
                    uiManager.ToMainMenu();
                    print("Exit Door");
                    break;
            }
        }
    }
}
