using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyCollector : MonoBehaviour
{
    [Header("Ref")]
    public Transform ItemHolderTransform;
    private int NumOfItemsHolding = 0;
    private float distanceBetweenMoneyObjectsOnHolder = 0.065f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddNewItem(Transform itemToAdd)
    {
        itemToAdd.DOJump(ItemHolderTransform.position + new Vector3(0, distanceBetweenMoneyObjectsOnHolder * NumOfItemsHolding, 0), 1.5f, 1, 0.3f).OnComplete(
            () =>
            {
                itemToAdd.SetParent(ItemHolderTransform, true);
                itemToAdd.localPosition = new Vector3(0, distanceBetweenMoneyObjectsOnHolder * NumOfItemsHolding, 0);
                itemToAdd.localRotation = Quaternion.identity;
                NumOfItemsHolding++;
            }
        );
    }
}
