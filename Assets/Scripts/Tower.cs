using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Extentions;
using UnityEngine;

public class Tower : MonoSingleton<Tower>
{
    private int playerAmount;
    [Range(5f, 10f)][SerializeField] private int maxPlayerPerRow;
    [Range(0f, 2f)][SerializeField] private float xGap;
    [Range(0f, 2f)][SerializeField] private float yGap;
    [Range(0f, 10f)][SerializeField] private float yOffset;

    [SerializeField] private List<int> towerCountList = new List<int>();
    [SerializeField] private List<GameObject> towerList = new List<GameObject>();

    public void CreateTower(int stickManNo)
    {
        playerAmount = stickManNo;
        FillTowerList();
        StartCoroutine(BuildTowerCoroutine());

    }
    void FillTowerList()
    {
        //towerCountList listesine karakterleri maxPlayerPerRow adedince grupl�p ekleyecek. 5'erli 5'erli
        for (int i = 1; i <= maxPlayerPerRow; i++)
        {
            if (playerAmount < i)       //e�er playerAmount 3 ise, 1'den 3'e kadar �al��acak. maxPlayerPerRow 5 olsa da, d�ng�, di�er 2 de�er i�in �al��mayacak
            {
                break;
            }
            playerAmount -= i;
            towerCountList.Add(i);      //1-2-3-4-5 diye ekleyecek. (maxPlayerPerRow'a kadar. maxPlayerPerRow'dan sonras�n� a�a��daki d�ng� ekliyor)
        }


        ////BU KISIM ASLINDA EN ALTTA KALAN SON 5 KARAKTER�N YALNIZ KALMAMASI ���N, ONLARIN DA B�R KATMAN OLMASI ���N YAZILMI�TIR
        for (int i = maxPlayerPerRow; i > 0; i--)   //maxPlayerPerRow=5. yani 5'den 0'a e�it oluncaya kadar 5 kez �al��acak
        {
            //BURADA ASLINDA KALAN KARAKTER SAYISI(playerAmount) maxPlayerPerRow'DAN FAZLA �SE, ONLAR AYRI B�R KATMAN OLMASI ���N BU �ART YAZILMI�TIR
            if (playerAmount >= i)      //playerAmount 10 ise, 10'dan a�a��s� i�in �al��acak. 10-9-8 diye maxPlayerPerRow'a e�it oluncaya kadar gidecek
            {
                playerAmount -= i;
                towerCountList.Add(i);  // 5-4-3-2-1... diye ekler
                i++;
            }
        }
    }

    IEnumerator BuildTowerCoroutine()
    {
        var towerId = 0;
        transform.DOMoveX(0f, 0.5f).SetEase(Ease.Flash);

        yield return new WaitForSecondsRealtime(0.55f);     //finishe geldikten 0.55 sn sonra �al��

        foreach (int towerHumanCount in towerCountList)
        {
            //towerCountList her bir katman�n rakam say�s�d�r.
            //her katman olu�turuldu�unda �nce bir y bo�lu�u ekleniyor
            foreach (GameObject child in towerList)
            {
                child.transform.DOLocalMove(child.transform.localPosition + new Vector3(0, yGap, 0), 0.2f).SetEase(Ease.OutQuad);      //towerList'in her eleman� y poz.nunda y�kseltiliyor
            }

            var tower = new GameObject("Tower" + towerId);  //katman say�s�na g�re tower0, tower1 diye gidiyor

            tower.transform.parent = transform;
            tower.transform.localPosition = new Vector3(0, 0, 0);

            towerList.Add(tower);

            var towerNewPos = Vector3.zero;     //her katman yeniden olu�turuldu�unda, bu de�er s�f�rlan�yor
            float tempTowerHumanCount = 0;

            for (int i = 1; i < transform.childCount; i++)      //bu d�ng� ile t�m childlara eri�ilir fakat, sadece towerHumanCount say�s� kadar� kullan�l�r
            {
                Transform child = transform.GetChild(i);
                child.transform.parent = tower.transform;   //her bir eleman�n parent� tower yap�l�yor
                child.transform.localPosition = new Vector3(tempTowerHumanCount * xGap, 0, 0);      //her birine x bo�lu�u ekleniyor
                towerNewPos += child.transform.position;    //her karakter eklendi�inde x poz.u artar. towerNewPos asl�nda toplam x poz.unu tutar
                tempTowerHumanCount++;
                i--;

                if (tempTowerHumanCount >= towerHumanCount)     //towerHumanCount kadar karakter diz
                {
                    break;  //d�ng�den tamamen ��k
                }
            }

            //-towerNewPos.x / towerHumanCount      yaz�larak, dizilen karakterler ortalan�r
            tower.transform.position = new Vector3(-towerNewPos.x / towerHumanCount, tower.transform.position.y - yOffset, tower.transform.position.z);

            towerId++;
            yield return new WaitForSecondsRealtime(0.2f);  //0.2 sn sonra bir sonraki katmana ge�
        }
    }
}
