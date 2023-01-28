using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Clone : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("red") && other.transform.parent.childCount > 0)
        {
            Destroy(other.gameObject);
            Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (other.CompareTag("Obstacle"))
        {
            Instantiate(blood, transform.position, Quaternion.identity);
            CloneManager.Instance.ObstacleControl();
            Destroy(gameObject);
        }

        if (other.CompareTag("DownGround"))
        {
            CloneManager.Instance.ObstacleControl();
            Destroy(gameObject);
        }

        if (other.CompareTag("stair"))
        {
            transform.parent.parent = null; 
            transform.parent = null;        //stickman
            GetComponent<Rigidbody>().isKinematic = GetComponent<Collider>().isTrigger = false;
            anim.SetBool("run", false);

            if (CloneManager.Instance.player.transform.childCount == 2)
            {
                other.GetComponent<Renderer>().material.DOColor(new Color(0.4f, 0.98f, 0.65f), 0.5f).SetLoops(1000, LoopType.Yoyo)
                    .SetEase(Ease.Flash);       //deðdiði merdivenin rengini alarm gibi yanýp söndürüyoruz
            }
        }

        if (other.CompareTag("Finish"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition;  //position constraint'ini unfreeze yap
            GetComponent<Collider>().isTrigger = true;
        }
    }
}

