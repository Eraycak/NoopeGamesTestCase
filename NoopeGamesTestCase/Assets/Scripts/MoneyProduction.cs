using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyProduction : MonoBehaviour
{
    private bool areaIsNotFull;
    [SerializeField]
    private float timeBetweenProduction = 2f;
    private List<GameObject> moneyObjectsList = new List<GameObject>();
    [SerializeField]
    private GameObject moneyObject;
    private GameObject createdObject;
    public void AreaIsPurchased()
    {
        int j = 0;
        int i = 0;
        StartCoroutine(produceMoney(i, j));
    }
    public void IsAreaFull()
    {
        if (moneyObjectsList.Count == 70)
        {
            areaIsNotFull = false;
        }
        else
        {
            areaIsNotFull = true;
            int i = moneyObjectsList.Count % 10;
            int j = moneyObjectsList.Count % 10;
            StartCoroutine(produceMoney(i, j));
        }
    }
    private IEnumerator produceMoney(int i, int j)
    {
        yield return new WaitForSeconds(timeBetweenProduction);
        Vector3 location = new Vector3(3.55f + (0.75f * i), -0.15f, 20.75f + (0.5f * j));
        Vector3 buildingLocation = new Vector3(8.86f, 0f, 27.5f);
        i++;
        if (i == 10)
        {
            j++;
            i = 0;
        }
        Vector3 rotation = new Vector3(90f, 0f, 90f);
        Vector3 scale = new Vector3(1.2f, 0.6f, 0.5f);
        moneyObject.transform.localScale = scale;
        createdObject = Instantiate(moneyObject, buildingLocation, Quaternion.Euler(rotation), this.gameObject.transform);
        createdObject.transform.DOJump(location, 1.5f, 1, 0.5f).OnComplete(
            () =>
            {
                createdObject.transform.SetParent(this.gameObject.transform, true);
            }
        );
        moneyObjectsList.Add(createdObject);
        if (j < 7)
        {
            StartCoroutine(produceMoney(i, j));
        }
        else
        {
            areaIsNotFull = false;
        }
    }
}
