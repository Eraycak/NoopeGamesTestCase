using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyProduction : MonoBehaviour
{

    [SerializeField]
    private GameObject moneyObject;
    private GameObject createdObject;
    private List<GameObject> moneyObjectsList = new List<GameObject>();
    public List<GameObject> MoneyObjectsList
    {
        get
        {
            return moneyObjectsList;
        }
        set
        {
            moneyObjectsList = value;
        }
    }
    [SerializeField]
    private float firstMoneyPosX = -4f, firstMoneyPosY = -0.15f, firstMoneyPosZ = -4f, distanceBetweenMoneyObjsX = 0.92f, distanceBetweenMoneyObjsY = 0.33f, distanceBetweenMoneyObjsZ = 1.25f;
    [SerializeField]
    private float timeBetweenProduction = 2f;
    [SerializeField]
    private int productionAreaLengthX = 10, productionAreaLengthZ = 7, productionAreaHeight = 5;
    private bool pauseProduction = false;
    public bool PauseProduction
    {
        get
        {
            return pauseProduction;
        }
        set
        {
            pauseProduction = value;
        }
    }
    private bool areaIsPurchased = false;

    public void AreaIsPurchased()
    {
        areaIsPurchased = true;
        StartCoroutine(produceMoney(0, 0, 0));
    }

    public void IsAreaFull()
    {
        if (moneyObjectsList.Count < (70 * productionAreaHeight) && areaIsPurchased)
        {
            int k = moneyObjectsList.Count / (productionAreaLengthX * productionAreaLengthZ);
            int i = (moneyObjectsList.Count - (70 * k)) % 10;
            int j = (moneyObjectsList.Count - (70 * k)) / 10;
            StartCoroutine(produceMoney(i, j, k));
        }
    }

    private IEnumerator produceMoney(int i, int j, int k)
    {
        if (!pauseProduction)
        {
            Vector3 location = new Vector3(firstMoneyPosX + (distanceBetweenMoneyObjsX * i), firstMoneyPosY + (distanceBetweenMoneyObjsY * k), firstMoneyPosZ + (distanceBetweenMoneyObjsZ * j));
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
            if (j == productionAreaLengthZ)
            {
                j = 0;
                k++;
            }
            if (k<productionAreaHeight)
            {
                yield return new WaitForSeconds(timeBetweenProduction);
                StartCoroutine(produceMoney(i, j, k));
            }
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }
    }

}
