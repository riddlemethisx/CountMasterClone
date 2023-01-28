using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidAnim : MonoBehaviour
{
    public float minPos;
    public float maxPos;

    public float animSpeed = 1;

    public enum EnumState
    {
        x,
        z
    }
    public EnumState state;

    void Update()
    {
        switch (state)
        {
            case EnumState.x:
                AnimX();
                break;
            case EnumState.z:
                AnimZ();
                break;
        }
    }


    void AnimX()
    {
        if (transform.position.x <= minPos)
        {
            transform.Rotate(0, -180, 0);
        }
        if (transform.position.x >= maxPos)
        {
            transform.Rotate(0, 180, 0);
        }
        transform.Translate(-animSpeed * Time.deltaTime, 0, 0);
    }

    void AnimZ()
    {
        if (transform.position.z <= minPos)
        {
            transform.Rotate(0, -180, 0);
        }
        if (transform.position.z >= maxPos)
        {
            transform.Rotate(0, 180, 0);
        }
        transform.Translate(0, 0, -animSpeed * Time.deltaTime);
    }

}
