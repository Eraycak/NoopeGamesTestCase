using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyControl : MonoBehaviour
{

    [SerializeField]
    private int neededMoneyNumber = 100;//amount of money to purchase the area
    private int keepNeeded;// keeps neededMoneyNumber start value
    private int keepNumOfItemsHolding;// amount of money on the player is carrying
    [SerializeField]
    private float moneyTransferTime = 2f;//interval between taking money from player
    private TextMeshPro moneyText;//text on the purchaseable area
    private MoneyCollector moneyCollector;

    private void Start()
    {
        if (neededMoneyNumber == 0)// if the area does not need money then turns it to the purchased area for starting production
        {
            this.gameObject.GetComponent<MoneyProduction>().AreaIsPurchased();
            moneyText = this.gameObject.GetComponentInChildren<TextMeshPro>();
            Destroy(moneyText);//destroys money text on the object because it will not be needed
            Destroy(this);//destroys this script on the object because it will not be needed
        }
        else
        {//if area requires money, assigns value to the text 
            keepNeeded = neededMoneyNumber;
            moneyText = this.gameObject.GetComponentInChildren<TextMeshPro>();
            moneyText.text = keepNeeded - neededMoneyNumber + "/" + keepNeeded;
        }
    }

    private void OnTriggerEnter(Collider other)
    {//if player enters the area, tries to reach moneyCollector component
        other.TryGetComponent(out moneyCollector);
        if (moneyCollector != null)
        {
            keepNumOfItemsHolding = moneyCollector.NumOfItemsHolding;//assigns total number of objects on the player' back
        }
    }

    private void OnTriggerStay(Collider other)
    {//if player stays in the collider, starts money transaction between area and player
        
        other.TryGetComponent(out moneyCollector);
        if (moneyCollector != null)
        {
            if (keepNumOfItemsHolding == moneyCollector.NumOfItemsHolding)// removes money from player after finishing previous transfer
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
            if (moneyCollector.NumOfItemsHolding > 0)//if player has money, transfers that
            {
                moneyCollector.RemoveItem(this.gameObject);
                neededMoneyNumber -= 10;//reduces required money value
                moneyText = this.gameObject.GetComponentInChildren<TextMeshPro>();
                moneyText.text = keepNeeded - neededMoneyNumber + "/" + keepNeeded;// updates text with new values
                if (neededMoneyNumber == 0)//if area does not require any more money, starts production and destroys money text and this script on the object
                {
                    this.gameObject.GetComponent<MoneyProduction>().AreaIsPurchased();
                    Destroy(moneyText);
                    Destroy(this);
                }
            }
        }
    }

}
