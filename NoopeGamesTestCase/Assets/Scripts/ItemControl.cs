using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControl : MonoBehaviour
{
    private bool isAlreadyCollected = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isAlreadyCollected)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            MoneyCollector moneyCollector;
            if (other.TryGetComponent(out moneyCollector))
            {
                moneyCollector.AddNewItem(this.transform);
                isAlreadyCollected = true;
            }
        }
    }
}
