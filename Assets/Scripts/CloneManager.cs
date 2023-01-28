using DG.Tweening;
using Extentions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class CloneManager : MonoSingleton<CloneManager>
{
    [HideInInspector]
    public int numberOfClones;
    private int numberOfEnemyClones;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private TextMeshPro counterText;
    [HideInInspector]
    public Transform player;
    
    [Range(0f, 1f)][SerializeField] private float DistanceFactor, Radius;
    TriggerManager triggerManager;

    private void Start()
    {
        player = transform;
        numberOfClones = transform.childCount - 1;   //1 tanesi Counter_Label olduðu için onu saymýyoruz
        counterText.text = numberOfClones.ToString();
        triggerManager = GetComponent<TriggerManager>();
    }

    public void MakeStickMan(int number)
    {
        for (int i = numberOfClones; i < number; i++)
        {
            Instantiate(clonePrefab, transform.position, quaternion.identity, transform);
        }

        numberOfClones = transform.childCount - 1;
        counterText.text = numberOfClones.ToString();
        FormatStickMan();
    }
    public void FormatStickMan()
    {
        for (int i = 1; i < player.childCount; i++)
        {
            var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
            var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);

            var NewPos = new Vector3(x, -0.55f, z);

            player.transform.GetChild(i).DOLocalMove(NewPos, 0.5f).SetEase(Ease.OutBack);       //0.5 sn içersinde, karakterler, ilgili poz.lara atanýr
        }
    }






    public void ObstacleControl()
    {
        numberOfClones = transform.childCount - 1;
        counterText.text = numberOfClones.ToString();
    }

    void CalculateStickmans()
    {
        numberOfEnemyClones = triggerManager.enemy.transform.GetChild(1).childCount - 1;
        numberOfClones = transform.childCount - 1;
    }

    public IEnumerator UpdateTheEnemyAndPlayerStickMansNumbers()
    {
        CalculateStickmans();

        while (numberOfEnemyClones > 0 && numberOfClones > 0)
        {
            CalculateStickmans();

            triggerManager.enemy.transform.GetChild(1).GetComponent<EnemyManager>().CounterTxt.text = numberOfEnemyClones.ToString();
            counterText.text = numberOfClones.ToString();

            yield return null;
        }

        if (numberOfEnemyClones == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.identity;
            }
        }
    }


}
