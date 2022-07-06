using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyCollector : MonoBehaviour
{

    [SerializeField]
    private Transform ItemHolderTransform;// gameObject which collects moneyObjects on the player's back
    [SerializeField]
    private int numOfItemsHolding = 0, maxNumOfItemsHolding = 5;//current number of objects and max number of objects which player carries
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
    private float distanceBetweenMoneyObjectsOnHolder = -0.015f;//distance for preventing money objects from intersecting
    private bool gettingMoney = false;// boolean for controlling money taking
    private bool removed = false;// boolean for waiting previous money transfer
    [SerializeField]
    private float moneyGettingTime = 2f;// money transfer time
    private List<GameObject> moneyObjectsList = new List<GameObject>();// keeping moneyObjects in the list

    public void AddNewItem(GameObject itemToAdd)
    {
        moneyObjectsList.Add(itemToAdd);// adding new money object to the list
        itemToAdd.transform.DOJump(ItemHolderTransform.position + new Vector3(distanceBetweenMoneyObjectsOnHolder * numOfItemsHolding, 0, 0), 1.5f, 1, 0.5f).OnComplete(//moves money to the player's back from production area
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
    {// removing last money object to the list
        GameObject itemToRemove = moneyObjectsList[moneyObjectsList.Count - 1];
        itemToRemove.transform.DOJump(purchasableArea.transform.position, 1.5f, 1, 0.5f).OnComplete(
            () =>
            {
                itemToRemove.transform.SetParent(purchasableArea.transform, true);
                itemToRemove.transform.localPosition = new Vector3(0f, -10f, 0f);
                itemToRemove.transform.localRotation = Quaternion.identity;
                numOfItemsHolding--;
                moneyObjectsList.Remove(itemToRemove);
                Destroy(itemToRemove, 0.2f);//destroys transferred object
            }
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ProductionArea"))//if player enters production area, production stops and transfer starts
        {
            gettingMoney = true;
            other.GetComponent<MoneyProduction>().PauseProduction = true;
            GameObject otherGameObject = other.gameObject;
            StartCoroutine(getMoney(otherGameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ProductionArea"))//if player exits production area, production starts and transfer stops
        {
            gettingMoney = false;
            other.GetComponent<MoneyProduction>().PauseProduction = false;
            StartCoroutine(Wait(other));
        }
    }

    private IEnumerator Wait(Collider collider)
    {//waits until remove process finishes
        yield return new WaitUntil(() => removed == true);
        collider.GetComponent<MoneyProduction>().IsAreaFull();
    }

    private IEnumerator getMoney(GameObject otherGameObject)
    {
        yield return new WaitForSeconds(moneyGettingTime);
        if (numOfItemsHolding < maxNumOfItemsHolding && gettingMoney)//player gets money if it has space and can get money
        {
            if (otherGameObject.GetComponent<MoneyProduction>().MoneyObjectsList.Count != 0)//player gets money if production area has money
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
            else//if production area has no money, gettingMoney will be false to prevent money transfer
            {
                gettingMoney = false;
            }
        }
    }

}
