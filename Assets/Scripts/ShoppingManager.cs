using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingManager : MonoBehaviour
{
    public static ShoppingManager Instance;
    public GameObject shoppingItem;
    public Transform listHolder;
    public Item[] collectibleItems;
    public int itemCount = 4;

    private Dictionary<Item, GameObject> collectItems;

    public void Awake()
    {
        Instance = this;

        collectItems = new Dictionary<Item, GameObject>(itemCount);

        for(int i = 0; i < itemCount; ++i) {
            int chosen = Random.Range(0, collectibleItems.Length);

            if (collectItems.ContainsKey(collectibleItems[chosen]))
            {
                i--;
                continue;
            }

            GameObject go = Instantiate(shoppingItem, listHolder);
            collectItems.Add(collectibleItems[chosen], go);
            go.GetComponent<Text>().text = collectibleItems[chosen].itemIdentifier;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("OpenList"))
        {
            listHolder.gameObject.SetActive(!listHolder.gameObject.activeSelf);
        }
    }

    public bool IsRequiredItem(Item item)
    {
        return collectItems.ContainsKey(item);
    }

    public void CheckOffItem(Item item)
    {
        if(collectItems.ContainsKey(item))
        {
           // Debug.Log("Crossing off");
            collectItems[item].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public bool HasAllRequiredItems(PlayerInventory inventory)
    {
        foreach (Item item in collectItems.Keys)
        {
            if (!inventory.ContainsItem(item))
            {
                return false;
            }
        }

        return true;
    }
}
