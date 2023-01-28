using System.Collections;
using System.Runtime.CompilerServices;
using DG.Tweening;
using Extentions;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    [HideInInspector]
    public bool isMoveForward;
    private new Camera camera;

    public Joystick variableJoystick;
    Vector3 direction;
    public int moveSpeed;
    float forwardSpeed = 0.8f;
    public Vector2 xLimit;

    TriggerManager triggerManager;

    void Start()
    {
        camera = Camera.main;
        isMoveForward = false;
        triggerManager = GetComponent<TriggerManager>();
    }

    private void Update()
    {
        if (!triggerManager.attack)
        {
            MoveThePlayer();
        }
    }

    void MoveThePlayer()
    {
        if (isMoveForward)
        {
            if (!triggerManager.FinishLine)
            {
                float inputX = variableJoystick.Horizontal;
                direction = camera.transform.TransformVector(inputX, 0, forwardSpeed);
                direction.y = 0;
                direction.x = Mathf.Clamp(direction.x, xLimit.x, xLimit.y);     //x limitasyonu, i�e yaram�yor
            }
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    //void FixedUpdate()
    //{
    //    //if (!triggerManager.attack)
    //    //{
    //    //    MovementFixedUpdate();      //OPT�M�ZASYONU K�T� ETK�LED��� ���N TRANSFORM �LE HAREKET ETT�R�LM��T�R
    //    //}
    //}

    //void MovementFixedUpdate()
    //{
    //    //HER Y�NE HAREKET
    //    //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);
    //    //rb.velocity = transform.forward * moveSpeed;        //bazen triggerlar�n i�inden ge?iyor. rb ile hareket ettirilirse bu sorun ��z�l�r.

    //    //RB �LE HAREKET
    //    rb.velocity = direction * moveSpeed;
    //}
}
