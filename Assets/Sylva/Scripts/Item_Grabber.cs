using System.Collections.Generic;
using UnityEngine;

public class Item_Grabber : MonoBehaviour
{
    [Header("References")]
    [Tooltip("list of prefabs to use for items")][SerializeField] private GameObject[] itemPrefabs;
    [Tooltip("canvas gameobject to spawn the items under")][SerializeField] private Transform spawnPoint;

    private int randomNumber;
    private int itemIndex;
    private List<GameObject> itemBoxes = new();

    public void Trigger()
    {
        itemBoxes.Clear();

        //------------------------------------------------------------------------------------------------------
        //         first item box
        //------------------------------------------------------------------------------------------------------

        randomNumber = Random.Range(1, 1001);
        itemIndex = 0;
        for (int i = 1; i < itemPrefabs.Length; i++)
        {
            if (itemPrefabs[itemIndex].GetComponent<Item>().getChance() > itemPrefabs[i].GetComponent<Item>().getChance() && randomNumber <= itemPrefabs[i].GetComponent<Item>().getChance() && !itemPrefabs[itemIndex].GetComponent<Item>().isRemoved())
            {
                itemIndex = i;
            }
        }
        if (itemPrefabs[itemIndex].gameObject.GetComponent<Item>().getRemoveCheck())
        {
            itemPrefabs[itemIndex].gameObject.GetComponent<Item>().setRemoved();
        }

        GameObject firstItemBox = Instantiate(itemPrefabs[itemIndex], spawnPoint.position, Quaternion.identity);
        firstItemBox.transform.SetParent(GameObject.FindGameObjectWithTag("ItemPoint").transform, false);

        itemBoxes.Add(firstItemBox);

        //------------------------------------------------------------------------------------------------------
        //          second item box
        //------------------------------------------------------------------------------------------------------

        randomNumber = Random.Range(1, 1001);
        itemIndex = 0;
        for (int i = 1; i < itemPrefabs.Length; i++)
        {
            if (itemPrefabs[itemIndex].GetComponent<Item>().getChance() > itemPrefabs[i].GetComponent<Item>().getChance() && randomNumber <= itemPrefabs[i].GetComponent<Item>().getChance() && !itemPrefabs[itemIndex].GetComponent<Item>().isRemoved())
            {
                itemIndex = i;
            }
        }
        if (itemPrefabs[itemIndex].gameObject.GetComponent<Item>().getRemoveCheck())
        {
            itemPrefabs[itemIndex].gameObject.GetComponent<Item>().setRemoved();
        }

        GameObject secondItemBox = Instantiate(itemPrefabs[itemIndex], spawnPoint.position, Quaternion.identity);
        secondItemBox.transform.SetParent(GameObject.FindGameObjectWithTag("ItemPoint").transform, false);

        itemBoxes.Add(secondItemBox);

        //------------------------------------------------------------------------------------------------------
        //          third item box
        //------------------------------------------------------------------------------------------------------

        randomNumber = Random.Range(1, 1001);
        itemIndex = 0;
        for (int i = 1; i < itemPrefabs.Length; i++)
        {
            if (itemPrefabs[itemIndex].GetComponent<Item>().getChance() > itemPrefabs[i].GetComponent<Item>().getChance() && randomNumber <= itemPrefabs[i].GetComponent<Item>().getChance() && !itemPrefabs[itemIndex].GetComponent<Item>().isRemoved())
            {
                itemIndex = i;
            }
        }
        if (itemPrefabs[itemIndex].gameObject.GetComponent<Item>().getRemoveCheck())
        {
            itemPrefabs[itemIndex].gameObject.GetComponent<Item>().setRemoved();
        }

        GameObject thirdItemBox = Instantiate(itemPrefabs[itemIndex], spawnPoint.position, Quaternion.identity);
        thirdItemBox.transform.SetParent(GameObject.FindGameObjectWithTag("ItemPoint").transform, false);

        itemBoxes.Add(thirdItemBox);
    }

    public void itemGrabbed()
    {
        foreach(GameObject item in itemBoxes)
        {
            Destroy(item);
        }
    }
}
