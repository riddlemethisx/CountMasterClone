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
        //towerCountList listesine karakterleri maxPlayerPerRow adedince gruplýp ekleyecek. 5'erli 5'erli
        for (int i = 1; i <= maxPlayerPerRow; i++)
        {
            if (playerAmount < i)       //eðer playerAmount 3 ise, 1'den 3'e kadar çalýþacak. maxPlayerPerRow 5 olsa da, döngü, diðer 2 deðer için çalýþmayacak
            {
                break;
            }
            playerAmount -= i;
            towerCountList.Add(i);      //1-2-3-4-5 diye ekleyecek. (maxPlayerPerRow'a kadar. maxPlayerPerRow'dan sonrasýný aþaðýdaki döngü ekliyor)
        }


        ////BU KISIM ASLINDA EN ALTTA KALAN SON 5 KARAKTERÝN YALNIZ KALMAMASI ÝÇÝN, ONLARIN DA BÝR KATMAN OLMASI ÝÇÝN YAZILMIÞTIR
        for (int i = maxPlayerPerRow; i > 0; i--)   //maxPlayerPerRow=5. yani 5'den 0'a eþit oluncaya kadar 5 kez çalýþacak
        {
            //BURADA ASLINDA KALAN KARAKTER SAYISI(playerAmount) maxPlayerPerRow'DAN FAZLA ÝSE, ONLAR AYRI BÝR KATMAN OLMASI ÝÇÝN BU ÞART YAZILMIÞTIR
            if (playerAmount >= i)      //playerAmount 10 ise, 10'dan aþaðýsý için çalýþacak. 10-9-8 diye maxPlayerPerRow'a eþit oluncaya kadar gidecek
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

        yield return new WaitForSecondsRealtime(0.55f);     //finishe geldikten 0.55 sn sonra çalýþ

        foreach (int towerHumanCount in towerCountList)
        {
            //towerCountList her bir katmanýn rakam sayýsýdýr.
            //her katman oluþturulduðunda önce bir y boþluðu ekleniyor
            foreach (GameObject child in towerList)
            {
                child.transform.DOLocalMove(child.transform.localPosition + new Vector3(0, yGap, 0), 0.2f).SetEase(Ease.OutQuad);      //towerList'in her elemaný y poz.nunda yükseltiliyor
            }

            var tower = new GameObject("Tower" + towerId);  //katman sayýsýna göre tower0, tower1 diye gidiyor

            tower.transform.parent = transform;
            tower.transform.localPosition = new Vector3(0, 0, 0);

            towerList.Add(tower);

            var towerNewPos = Vector3.zero;     //her katman yeniden oluþturulduðunda, bu deðer sýfýrlanýyor
            float tempTowerHumanCount = 0;

            for (int i = 1; i < transform.childCount; i++)      //bu döngü ile tüm childlara eriþilir fakat, sadece towerHumanCount sayýsý kadarý kullanýlýr
            {
                Transform child = transform.GetChild(i);
                child.transform.parent = tower.transform;   //her bir elemanýn parentý tower yapýlýyor
                child.transform.localPosition = new Vector3(tempTowerHumanCount * xGap, 0, 0);      //her birine x boþluðu ekleniyor
                towerNewPos += child.transform.position;    //her karakter eklendiðinde x poz.u artar. towerNewPos aslýnda toplam x poz.unu tutar
                tempTowerHumanCount++;
                i--;

                if (tempTowerHumanCount >= towerHumanCount)     //towerHumanCount kadar karakter diz
                {
                    break;  //döngüden tamamen çýk
                }
            }

            //-towerNewPos.x / towerHumanCount      yazýlarak, dizilen karakterler ortalanýr
            tower.transform.position = new Vector3(-towerNewPos.x / towerHumanCount, tower.transform.position.y - yOffset, tower.transform.position.z);

            towerId++;
            yield return new WaitForSecondsRealtime(0.2f);  //0.2 sn sonra bir sonraki katmana geç
        }
    }
}
