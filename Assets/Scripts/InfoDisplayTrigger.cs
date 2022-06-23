using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDisplayTrigger : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private UIManager uIManager;
    private infoTag tag;
    private string info1;
    private string info2;
    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        uIManager = FindObjectOfType<UIManager>();
    }


    public void SetUpInfo(string info1, string info2,infoTag Tag)
    {
        this.tag = Tag;
        this.info1 = info1;
        this.info2 = info2;
    }
    public void SetUpInfo(string info1,infoTag Tag)
    {
        this.tag = Tag;
        this.info1 = info1;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (tag)
        {
            case infoTag.LevelDisplay:
                uIManager.ShowLevelInfo(info1, info2);
                break;
            case infoTag.NoteDisplay:
                uIManager.ShowLevelInfo(info1);
                break;
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            uIManager.HideLevelInfo();
        }
    }
}
