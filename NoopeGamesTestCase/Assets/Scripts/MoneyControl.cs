using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyControl : MonoBehaviour
{
    [SerializeField]
    private int neededMoneyNumber = 100;
    private int keepNeeded;
    private int keepNumOfItemsHolding;
    [SerializeField]
    private float moneyTransferTime = 2f;
    private TextMeshPro moneyText;
    private MoneyCollector moneyCollector;

    private void Start()
    {
        if (neededMoneyNumber == 0)
        {
            this.gameObject.GetComponent<MoneyProduction>().AreaIsPurchased();
            moneyText = this.gameObject.GetComponentInChildren<TextMeshPro>();
            Destroy(moneyText);
            Destroy(this);
        }
        else
        {
            keepNeeded = neededMoneyNumber;
            moneyText = this.gameObject.GetComponentInChildren<TextMeshPro>();
            moneyText.text = keepNeeded - neededMoneyNumber + "/" + keepNeeded;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out moneyCollector);
        if (moneyCollector != null)
        {
            keepNumOfItemsHolding = moneyCollector.NumOfItemsHolding;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        other.TryGetComponent(out moneyCollector);
        if (moneyCollector != null)
        {
            if (keepNumOfItemsHolding == moneyCollector.NumOfItemsHolding)
            {
                keepNumOfItemsHolding--;
                StartCoroutine(waitForMoneyToTransfer(other));
            }
        }
    }

    IEnumerator waitForMoneyToTransfer(Collider other)
    {
        yield return new WaitForSeconds(moneyTransferTime);
        if (other.TryGetComponent(out moneyCollector))
        {
            if (moneyCollector.NumOfItemsHolding > 0)
            {
                moneyCollector.RemoveItem(this.gameObject);
                neededMoneyNumber -= 10;
                moneyText = this.gameObject.GetComponentInChildren<TextMeshPro>();
                moneyText.text = keepNeeded - neededMoneyNumber + "/" + keepNeeded;
                if (neededMoneyNumber == 0)
                {
                    this.gameObject.GetComponent<MoneyProduction>().AreaIsPurchased();
                    Destroy(moneyText);
                    Destroy(this);
                }
            }
        }
    }
}
