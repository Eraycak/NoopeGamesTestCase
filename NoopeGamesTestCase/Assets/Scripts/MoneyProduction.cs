using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyProduction : MonoBehaviour
{

    [SerializeField]
    private GameObject moneyObject;// object for instantianting
    private GameObject createdObject;//instantiated object from moneyObject
    private List<GameObject> moneyObjectsList = new List<GameObject>();//list for keeping createdObjects in order and removing them by their list number
    public List<GameObject> MoneyObjectsList
    {
        get
        {
            return moneyObjectsList;
        }
        set
        {
            moneyObjectsList = value;
        }
    }
    [SerializeField]
    private float firstMoneyPosX = -4f, firstMoneyPosY = -0.15f, firstMoneyPosZ = -4f, distanceBetweenMoneyObjsX = 0.92f, distanceBetweenMoneyObjsY = 0.33f, distanceBetweenMoneyObjsZ = 1.25f;//location information for putting money objects on the plane in order
    [SerializeField]
    private float timeBetweenProduction = 2f; // interval between creation for money objects
    [SerializeField]
    private int productionAreaLengthX = 10, productionAreaLengthZ = 7, productionAreaHeight = 5;// size informations of plane
    private bool pauseProduction = false;// boolean for pausing production while player taking money from there
    public bool PauseProduction
    {
        get
        {
            return pauseProduction;
        }
        set
        {
            pauseProduction = value;
        }
    }
    private bool areaIsPurchased = false;// boolean for preventing production before purchasing area

    public void AreaIsPurchased()
    {
        areaIsPurchased = true;
        StartCoroutine(produceMoney(0, 0, 0));// start's production after purchasing area
    }

    public void IsAreaFull()
    {
        if (moneyObjectsList.Count < (70 * productionAreaHeight) && areaIsPurchased)// checks if area is purchased and has empty space
        {
            int k = moneyObjectsList.Count / (productionAreaLengthX * productionAreaLengthZ);//calculates y axis number 
            int i = (moneyObjectsList.Count - (70 * k)) % 10;//calculates x axis number 
            int j = (moneyObjectsList.Count - (70 * k)) / 10;//calculates z axis number 
            StartCoroutine(produceMoney(i, j, k));//resumes production with those values
        }
    }

    private IEnumerator produceMoney(int i, int j, int k)
    {
        if (!pauseProduction)//checks production paused or not
        {
            Vector3 location = new Vector3(firstMoneyPosX + (distanceBetweenMoneyObjsX * i), firstMoneyPosY + (distanceBetweenMoneyObjsY * k), firstMoneyPosZ + (distanceBetweenMoneyObjsZ * j));// calculates created objects location by using values
            Vector3 buildingLocation = transform.parent.position;//gets parent's location for doing DoLocalJump
            Vector3 rotation = new Vector3(90f, -90f, 0f);//money objects rotation 
            Vector3 scale = new Vector3(1.2f, 0.6f, 0.5f);//money objects scale
            moneyObject.transform.localScale = scale;
            createdObject = Instantiate(moneyObject, buildingLocation, Quaternion.Euler(rotation), this.gameObject.transform);//instantiate object from money object with other values
            createdObject.transform.DOLocalJump(location, 1.5f, 1, 0.5f);// jumps from building location to calculated location
            moneyObjectsList.Add(createdObject);// adds created object to the list 
            i++;
            if (i == productionAreaLengthX)//checks x axis if it is full or not
            {
                j++;//if it is full moves to next z axis and start filling there
                i = 0;
            }
            if (j == productionAreaLengthZ)//checks z axis if it is full or not
            {
                k++;//if it is full moves to next y axis and start filling there
                j = 0;
            }
            if (k<productionAreaHeight)//checks area is full or not
            {
                yield return new WaitForSeconds(timeBetweenProduction);
                StartCoroutine(produceMoney(i, j, k));
            }
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }
    }

}
