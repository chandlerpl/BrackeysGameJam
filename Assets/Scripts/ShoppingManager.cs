using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingManager : MonoBehaviour
{
    public GameObject shoppingItem;
    public Transform listHolder;
    public Item[] collectibleItems;
    public int itemCount = 4;

    public Transform cart;
    private Dictionary<string, GameObject> collectItems;
    private List<string> collected = new List<string>();
    public void Awake()
    {
        collectItems = new Dictionary<string, GameObject>(itemCount);

        for(int i = 0; i < itemCount; ++i) {
            int chosen = Random.Range(0, collectibleItems.Length);

            Item item = collectibleItems[chosen];
            if (collectItems.ContainsKey(item.itemIdentifier))
            {
                i--;
                continue;
            }

            GameObject go = Instantiate(item.itemIconPrefab, listHolder);
            collectItems.Add(item.itemIdentifier, go);
            //go.GetComponent<TextMeshProUGUI>().text = item.itemIdentifier;
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
        return collectItems.ContainsKey(item.itemIdentifier);
    }

    public void CheckItem(Item item)
    {
        if (collectItems.ContainsKey(item.itemIdentifier))
        {
            if (!collected.Contains(item.itemIdentifier))
            {
                collectItems[item.itemIdentifier].transform.GetChild(1).gameObject.SetActive(true);
                collected.Add(item.itemIdentifier);
            }
        }
    }
    public void UncheckItem(Item item)
    {
        if (collectItems.ContainsKey(item.itemIdentifier))
        {
            if (collected.Contains(item.itemIdentifier))
            {
                collectItems[item.itemIdentifier].transform.GetChild(1).gameObject.SetActive(false);
                collected.Remove(item.itemIdentifier);
            }
        }
    }

    public bool HasAllRequiredItems(Inventory inventory)
    {
        foreach (string item in collectItems.Keys)
        {
            if(!collected.Contains(item))
            {
                return false;
            }
        }

        return true;
    }
}
