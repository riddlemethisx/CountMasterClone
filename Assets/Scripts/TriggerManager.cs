using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    [HideInInspector]
    public Transform enemy;
    [HideInInspector]
    public bool attack;
    CloneManager cloneManager;
    PlayerController playerManager;
    private new Camera camera;
    [HideInInspector]
    public bool FinishLine;

    void Start()
    {
        camera = Camera.main;
        cloneManager = GetComponent<CloneManager>();
        playerManager = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (attack)
        {
            var enemyDirection = new Vector3(enemy.position.x, transform.position.y, enemy.position.z) - transform.position;

            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation =
                    Quaternion.Slerp(transform.GetChild(i).rotation, Quaternion.LookRotation(enemyDirection, Vector3.up), Time.deltaTime * 3f);
            }

            if (enemy.GetChild(1).childCount > 1)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    var Distance = enemy.GetChild(1).GetChild(0).position - transform.GetChild(i).position;

                    if (Distance.magnitude < 1.5f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            new Vector3(enemy.GetChild(1).GetChild(0).position.x, transform.GetChild(i).position.y,
                                enemy.GetChild(1).GetChild(0).position.z), Time.deltaTime * 1f);
                    }
                }
            }
            else
            {
                attack = false;

                cloneManager.FormatStickMan();

                //bütün karakterlerin rotationu sýfýrlanýyor
                for (int i = 1; i < transform.childCount; i++)
                    transform.GetChild(i).rotation = Quaternion.identity;


                enemy.gameObject.SetActive(false);

            }

            if (transform.childCount == 1)
            {
                enemy.transform.GetChild(1).GetComponent<EnemyManager>().StopAttacking();
                gameObject.SetActive(false);
            }
        }



        if (transform.childCount == 1 && FinishLine)
        {
            playerManager.isMoveForward = false;
        }
    }


    

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("gate"))
        {
            other.transform.parent.GetChild(0).GetComponent<BoxCollider>().enabled = false; // gate 1
            other.transform.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false; // gate 2

            var gateManager = other.GetComponent<GenerateRandomValue>();

            cloneManager.numberOfClones = transform.childCount - 1;

            if (gateManager.multiply)
            {
                cloneManager.MakeStickMan(cloneManager.numberOfClones * gateManager.randomNumber);
            }
            else
            {
                cloneManager.MakeStickMan(cloneManager.numberOfClones + gateManager.randomNumber);
            }
        }

        if (other.CompareTag("enemy"))
        {
            enemy = other.transform;
            attack = true;

            other.transform.GetChild(1).GetComponent<EnemyManager>().AttackThem(transform);

            StartCoroutine(cloneManager.UpdateTheEnemyAndPlayerStickMansNumbers());
        }

        if (other.CompareTag("Finish"))
        {
            //SecondCam.SetActive(true);

            camera.transform.parent.GetComponent<CamFollow>().isTriggeredFinish = true;

            FinishLine = true;
            Tower.Instance.CreateTower(transform.childCount - 1);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
