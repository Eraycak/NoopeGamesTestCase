using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyCollector : MonoBehaviour
{
    [Header("Ref")]
    public Transform ItemHolderTransform;
    public int NumOfItemsHolding = 0;
    public int MaxNumOfItemsHolding = 5;
    private float distanceBetweenMoneyObjectsOnHolder = -0.015f;
    public List<GameObject> moneyObjectsList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddNewItem(GameObject itemToAdd)
    {
        moneyObjectsList.Add(itemToAdd);
        itemToAdd.transform.DOJump(ItemHolderTransform.position + new Vector3(distanceBetweenMoneyObjectsOnHolder * NumOfItemsHolding, 0, 0), 1.5f, 1, 0.5f).OnComplete(
            () =>
            {
                itemToAdd.transform.SetParent(ItemHolderTransform, true);
                itemToAdd.transform.localPosition = new Vector3(distanceBetweenMoneyObjectsOnHolder * NumOfItemsHolding, 0, -0.0125f);
                itemToAdd.transform.localRotation = Quaternion.Euler(180f, 90f, 90f);
                NumOfItemsHolding++;
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
                NumOfItemsHolding--;
                moneyObjectsList.Remove(itemToRemove);
                Destroy(itemToRemove, 0.2f);
            }
        );
    }
}
