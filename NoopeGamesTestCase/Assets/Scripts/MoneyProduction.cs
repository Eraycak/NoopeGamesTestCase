using System.Collections;
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
            int i = moneyObjectsList.Count % 10;
            int j = moneyObjectsList.Count / 10;
            StartCoroutine(produceMoney(i, j));
        }
    }
    private IEnumerator produceMoney(int i, int j)
    {
        yield return new WaitForSeconds(timeBetweenProduction);
        if (!pauseProduction)
        {
            Vector3 location = new Vector3(firstMoneyPosX + (distanceBetweenMoneyObjsX * i), firstMoneyPosY, firstMoneyPosZ + (distanceBetweenMoneyObjsZ * j));
            Vector3 buildingLocation = transform.parent.position;
            i++;
            if (i == 10)
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
            if (j < 7)
            {
                StartCoroutine(produceMoney(i, j));
            }
        }
    }
}
