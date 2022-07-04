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
                if (moneyCollector.NumOfItemsHolding < moneyCollector.MaxNumOfItemsHolding)
                {
                    moneyCollector.AddNewItem(this.gameObject);
                    isAlreadyCollected = true;
                }
                else
                {
                    isAlreadyCollected = false;
                }
            }
        }
    }
}
