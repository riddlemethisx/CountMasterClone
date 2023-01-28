using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public TextMeshPro CounterTxt;
    [SerializeField] private GameObject stickMan;   //prefab
    [Range(0f, 1f)][SerializeField] private float DistanceFactor, Radius;

    [HideInInspector]
    public Transform enemy;
    public bool attack;
    void Start()
    {
        for (int i = 0; i < Random.Range(20, 30); i++)
        {
            Instantiate(stickMan, transform.position, new Quaternion(0f, 180f, 0f, 1f), transform);
        }

        CounterTxt.text = (transform.childCount).ToString();

        FormatStickMan();
    }

    private void FormatStickMan()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //bu hesaplamadaki i deðeri arttýkça, dairenin içinden baþlayarak dairenin dýþýna doðru sarmal devam eder
            var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius); 
            var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);

            var NewPos = new Vector3(x, -0.6f, z);      //x ve z pozisyonlarý deðiþtiriliyor.

            transform.transform.GetChild(i).localPosition = NewPos;
        }
    }

    void Update()
    {
        if (attack && transform.childCount > 1)
        {
            var enemyDirection = enemy.position - transform.position;   //yön

            for (int i = 0; i < transform.childCount; i++)      //tüm karakterleri yakýnlaþtýr
            {
                transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(i).rotation, quaternion.LookRotation(enemyDirection, Vector3.up),
                    Time.deltaTime * 3f);

                if (enemy.childCount > 1)
                {
                    var distance = enemy.GetChild(1).position - transform.GetChild(i).position;         //mesafe

                    if (distance.magnitude < 1.5f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            enemy.GetChild(1).position, Time.deltaTime * 2f);   //yavaþ yavaþ yaklaþ
                    }
                }

            }
        }
    }

    public void AttackThem(Transform enemyForce)
    {
        enemy = enemyForce;
        attack = true;      //yaklaþmaya baþla

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("run", true);
        }
    }

    public void StopAttacking()
    {
        PlayerController.Instance.isMoveForward = attack = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("run", false);
        }

    }
}
