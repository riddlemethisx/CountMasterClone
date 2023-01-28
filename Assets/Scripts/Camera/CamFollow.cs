using DG.Tweening;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class CamFollow : MonoBehaviour
{

    public Transform target = null;
    Vector3 offset;
    public float shakeStrength = 2f;
    public bool sheakability;
    public Vector2 xLimit;
    public bool limitation = true;

    [HideInInspector]
    public bool isTriggeredFinish;

    int finishCamOffset = 3;
    Transform mainPlayer;
    bool onetime = false;

    void Start()
    {
        offset = gameObject.transform.position - target.transform.position;
    }

    void LateUpdate()
    {
        if (!isTriggeredFinish)
        {
            if (target != null)
            {
                if (limitation)
                {
                    transform.position = new Vector3(
                            Mathf.Clamp((target.position.x + offset.x), xLimit.x, xLimit.y),
                            target.position.y + offset.y,
                            target.position.z + offset.z
                            );
                }
                else
                {
                    transform.position = new Vector3(
                            target.position.x + offset.x,
                            target.position.y + offset.y,
                            target.position.z + offset.z
                            );
                }
            }
        }
        else
        {
            if (transform.rotation.y >= -0.5f)
                transform.RotateAround(target.position, new Vector3(0, -90, 0), 40 * Time.deltaTime);

            if (!onetime) { 
                mainPlayer = target.GetChild(1);
                onetime = true;
            }
            transform.position = new Vector3(
                            target.position.x + offset.x + finishCamOffset * 1.5f,
                            mainPlayer.position.y + offset.y + finishCamOffset,
                            target.position.z + offset.z - finishCamOffset / 8
                            );
        }

    }

    public void Shake()
    {
        if (sheakability)
        {
            transform.GetChild(0).DOShakePosition(1f, shakeStrength, fadeOut: true);    //childýný sallamazsak efekt gerçekleþmiyor
            return;
        }
    }

}
