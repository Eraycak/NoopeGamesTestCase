using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyCollector : MonoBehaviour
{

    [SerializeField]
    private Transform ItemHolderTransform;
    [SerializeField]
    private int numOfItemsHolding = 0, maxNumOfItemsHolding = 5;
    public int NumOfItemsHolding
    {
        get
        {
            return numOfItemsHolding;
        }
        set
        {
            numOfItemsHolding = value;
        }
    }
    private float distanceBetweenMoneyObjectsOnHolder = -0.015f;
    private bool gettingMoney = false;
    private bool removed = false;
    [SerializeField]
    private float moneyGettingTime = 2f;
    private List<GameObject> moneyObjectsList = new List<GameObject>();

    public void AddNewItem(GameObject itemToAdd)
    {
        moneyObjectsList.Add(itemToAdd);
        itemToAdd.transform.DOJump(ItemHolderTransform.position + new Vector3(distanceBetweenMoneyObjectsOnHolder * numOfItemsHolding, 0, 0), 1.5f, 1, 0.5f).OnComplete(
            () =>
            {
                itemToAdd.transform.SetParent(ItemHolderTransform, true);
                itemToAdd.transform.localPosition = new Vector3(distanceBetweenMoneyObjectsOnHolder * numOfItemsHolding, 0, -0.0125f);
                itemToAdd.transform.localRotation = Quaternion.Euler(180f, 90f, 90f);
                numOfItemsHolding++;
            }
        );
    }

    public void RemoveItem(GameObject purchasableArea)
    {
        GameObject itemToRemove = moneyObjectsList[moneyObjectsList.Count - 1];
        itemToRemove.transform.DOJump(purchasableArea.transform.position, 1.5f, 1, 0.5f).OnComplete(
            () =>
            {
                itemToRemove.transform.SetParent(purchasableArea.transform, true);
                itemToRemove.transform.localPosition = new Vector3(0f, -10f, 0f);
                itemToRemove.transform.localRotation = Quaternion.identity;
                numOfItemsHolding--;
                moneyObjectsList.Remove(itemToRemove);
                Destroy(itemToRemove, 0.2f);
            }
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ProductionArea"))
        {
            gettingMoney = true;
            other.GetComponent<MoneyProduction>().PauseProduction = true;
            GameObject otherGameObject = other.gameObject;
            StartCoroutine(getMoney(otherGameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ProductionArea"))
        {
            gettingMoney = false;
            other.GetComponent<MoneyProduction>().PauseProduction = false;
            StartCoroutine(Wait(other));
        }
    }

    private IEnumerator Wait(Collider collider)
    {
        yield return new WaitUntil(() => removed == true);
        collider.GetComponent<MoneyProduction>().IsAreaFull();
    }

    private IEnumerator getMoney(GameObject otherGameObject)
    {
        yield return new WaitForSeconds(moneyGettingTime);
        if (numOfItemsHolding < maxNumOfItemsHolding && gettingMoney)
        {
            if (otherGameObject.GetComponent<MoneyProduction>().MoneyObjectsList.Count != 0)
            {
                removed = false;
                List<GameObject> gameObjectsList = otherGameObject.GetComponent<MoneyProduction>().MoneyObjectsList;
                GameObject gameObject = gameObjectsList[gameObjectsList.Count - 1];
                gameObjectsList.RemoveAt(gameObjectsList.Count - 1);
                otherGameObject.GetComponent<MoneyProduction>().MoneyObjectsList = gameObjectsList;
                removed = true;
                AddNewItem(gameObject);
                StartCoroutine(getMoney(otherGameObject));
            }
            else
            {
                gettingMoney = false;
            }
        }
    }

}
