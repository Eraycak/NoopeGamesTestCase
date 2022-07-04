﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyProduction : MonoBehaviour
{
    [SerializeField]
    private GameObject moneyObject;
    private GameObject createdObject;
    public List<GameObject> moneyObjectsList = new List<GameObject>();
    [SerializeField]
    private float firstMoneyPosX = -4f, firstMoneyPosY = -0.15f, firstMoneyPosZ = -4f, distanceBetweenMoneyObjsX = 0.92f, distanceBetweenMoneyObjsZ = 1.25f;
    [SerializeField]
    private float timeBetweenProduction = 2f;
    [SerializeField]
    private int productionAreaLengthX = 10, productionAreaLengthZ = 7;
    public bool pauseProduction = false;
    private bool areaIsPurchased = false;

    public void AreaIsPurchased()
    {
        areaIsPurchased = true;
        StartCoroutine(produceMoney(0, 0));
    }
    public void IsAreaFull()
    {
        if (moneyObjectsList.Count < 70 && areaIsPurchased)
        {
            //Debug.LogError("isarefull " + moneyObjectsList.Count);
            int i = moneyObjectsList.Count % 10;
            int j = moneyObjectsList.Count / 10;
            StartCoroutine(produceMoney(i, j));
        }
    }
    private IEnumerator produceMoney(int i, int j)
    {
        if (!pauseProduction)
        {
            Vector3 location = new Vector3(firstMoneyPosX + (distanceBetweenMoneyObjsX * i), firstMoneyPosY, firstMoneyPosZ + (distanceBetweenMoneyObjsZ * j));
            Vector3 buildingLocation = transform.parent.position;
            i++;
            if (i == productionAreaLengthX)
            {
                j++;
                i = 0;
            }
            Vector3 rotation = new Vector3(90f, -90f, 0f);
            Vector3 scale = new Vector3(1.2f, 0.6f, 0.5f);
            moneyObject.transform.localScale = scale;
            createdObject = Instantiate(moneyObject, buildingLocation, Quaternion.Euler(rotation), this.gameObject.transform);
            createdObject.GetComponent<SphereCollider>().isTrigger = false;
            createdObject.transform.DOLocalJump(location, 1.5f, 1, 0.5f);
            moneyObjectsList.Add(createdObject);
            //Debug.LogError("coro " + moneyObjectsList.Count);
            if (j < productionAreaLengthZ)
            {
                yield return new WaitForSeconds(timeBetweenProduction);
                StartCoroutine(produceMoney(i, j));
            }
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }
    }
}
