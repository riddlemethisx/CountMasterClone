using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToMoveControl : MonoBehaviour
{
    public Joystick joystick;
    public GameObject dragToMoveUI;


    void Update()
    {
        if(joystick.Horizontal != 0)
        {
            dragToMoveUI.SetActive(false);

            PlayerController.Instance.isMoveForward = true;
            CloneManager.Instance.player.GetChild(1).GetComponent<Animator>().SetBool("run", true);

            gameObject.SetActive(false);
        }
    }
}
