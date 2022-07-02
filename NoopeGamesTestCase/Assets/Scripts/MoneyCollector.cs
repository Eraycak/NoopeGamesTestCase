using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyCollector : MonoBehaviour
{
    [Header("Ref")]
    public Transform ItemHolderTransform;
    private int NumOfItemsHolding = 0;
    private float distanceBetweenMoneyObjectsOnHolder = -0.015f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddNewItem(Transform itemToAdd)
    {
        itemToAdd.DOJump(ItemHolderTransform.position + new Vector3(distanceBetweenMoneyObjectsOnHolder * NumOfItemsHolding, 0, 0), 1.5f, 1, 0.5f).OnComplete(
            () =>
            {
                itemToAdd.SetParent(ItemHolderTransform, true);
                itemToAdd.localPosition = new Vector3(distanceBetweenMoneyObjectsOnHolder * NumOfItemsHolding, 0, -0.0125f);
                itemToAdd.localRotation = Quaternion.Euler(180f, 90f, 90f);
                NumOfItemsHolding++;
            }
        );
    }
}
