using System.Collections.Generic;
using UnityEngine;

public class Item_Grabber : MonoBehaviour
{
    [Header("References")]
    [Tooltip("list of prefabs to use for items")][SerializeField] private GameObject[] itemPrefabs;
    [Tooltip("canvas gameobject to spawn the items under")][SerializeField] private Transform spawnPoint;
    [Tooltip("player gameobject/prefab used as a reference for the buttons")][SerializeField] private GameObject playerObject;

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

    public void increaseMaxHealth(int num) {
        playerObject.GetComponent<StatsScript>().MaxHealth += num;
    }

    public void increaseDefense(float num)
    {
        playerObject.GetComponent<StatsScript>().Defense += num;
    }

    public void increaseMoveSpeed(float num)
    {
        playerObject.GetComponent<StatsScript>().MoveSpeed += num;
    }

    public void increaseAttackSpeed(float num)
    {
        playerObject.GetComponent<StatsScript>().AttackSpeedBonus += num;
    } 
    
    public void increaseDifficulty(float num)
    {
        playerObject.GetComponent<StatsScript>().DifficultyScaler += num;
    }

    public void increaseMeleeScaler(float num)
    {
        playerObject.GetComponent<StatsScript>().MeleeDamageScaler += num;
    }

    public void increaseMeleeDamage(int num)
    {
        playerObject.GetComponent<StatsScript>().MeleeDamageBonus += num;
        Debug.Log("function is working!");
    }

    public void increaseRangeScaler(float num)
    {
        playerObject.GetComponent<StatsScript>().RangeDamageScaler += num;
    }

    public void increaseRangeDamage(int num)
    {
        playerObject.GetComponent<StatsScript>().RangeDamageBonus += num;
    }

    public void increaseAbilityScaler(float num)
    {
        playerObject.GetComponent<StatsScript>().AbilityDamageScaler += num;
    }

    public void increaseAbilityBonus(int num)
    {
        playerObject.GetComponent<StatsScript>().AbilityDamageBonus += num;
    }

    public void increaseUniversalScaler(float num)
    {
        playerObject.GetComponent<StatsScript>().AllDamageScaler += num;
    }

    public void incraseUniversalDamage(int num)
    {
        playerObject.GetComponent<StatsScript>().AllDamageBonus += num;
    }
    
    public void increaseMaxScrap(int num)
    {
        playerObject.GetComponent<StatsScript>().MaxScrap += num;
    }
}
