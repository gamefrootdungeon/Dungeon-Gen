using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTChest : MonoBehaviour
{
    public bool Open = false;
    public Animator anim;
    Vector3 rayStart;
    Vector3 direction;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Open = true;
            anim.SetBool("Open", Open);
        }
    }

    //Shoots raycast to see if the front is facing a wall, will rotate until not facing a wall
    public void CheckIfWallInFront()
    {
        //print("Check");
        direction = Vector3.zero;
        rayStart = Vector3.zero;

        RaycastHit hit;
        rayStart = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        direction = transform.forward * 3;

        if (Physics.Raycast(rayStart, direction, 3))
        {
            transform.Rotate(transform.up * 90);
            CheckIfWallInFront();
        }
        return;
    }
}
