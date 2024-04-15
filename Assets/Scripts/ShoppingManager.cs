using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingManager : MonoBehaviour
{
    public static ShoppingManager Instance;
    public GameObject shoppingItem;
    public Transform listHolder;
    public Item[] collectibleItems;
    public int itemCount = 4;

    private Dictionary<string, GameObject> collectItems;
    private List<string> collected = new List<string>();
    public void Awake()
    {
        Instance = this;

        collectItems = new Dictionary<string, GameObject>(itemCount);

        for(int i = 0; i < itemCount; ++i) {
            int chosen = Random.Range(0, collectibleItems.Length);

            if (collectItems.ContainsKey(collectibleItems[chosen].itemIdentifier))
            {
                i--;
                continue;
            }

            GameObject go = Instantiate(shoppingItem, listHolder);
            collectItems.Add(collectibleItems[chosen].itemIdentifier, go);
            go.GetComponent<TextMeshProUGUI>().text = collectibleItems[chosen].itemIdentifier;
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

    public void CheckOffItem(Item item)
    {
        if(collectItems.ContainsKey(item.itemIdentifier))
        {
           // Debug.Log("Crossing off");
            collectItems[item.itemIdentifier].transform.GetChild(0).gameObject.SetActive(true);
            collected.Add(item.itemIdentifier);
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
