using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenerateRandomValue : MonoBehaviour
{
    public TextMeshPro GateNo;
    [HideInInspector]
    public int randomNumber;
    public bool multiply;
    void Start()
    {
        //MULTIPLY SEÇENEÐÝNÝ RANDOM BELÝRLEME
        //int valueType = Random.Range(0, 2);
        //if (valueType == 0)
        //    multiply = false;
        //else
        //    multiply = true;

        if (multiply)
        {
            randomNumber = Random.Range(2, 3);
            GateNo.text = "X" + randomNumber;
        }
        else
        {
            randomNumber = Random.Range(30, 100);

            if (randomNumber % 2 != 0)      //eðer çift sayý deðil ise
                randomNumber += 1;
            
            GateNo.text = randomNumber.ToString();
        }
    }
    
}
