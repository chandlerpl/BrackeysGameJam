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

    public bool HasCheckedOut { get; private set; }
    public Transform cart;
    private Dictionary<string, ItemUI> collectItems;
    private List<string> collected = new List<string>();
    public void Awake()
    {
        collectItems = new (itemCount);

        StartCoroutine(Test());
    }

    public IEnumerator Test()
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < itemCount; ++i)
        {
            int chosen = Random.Range(0, collectibleItems.Length);

            Item item = collectibleItems[chosen];
            if (collectItems.ContainsKey(item.itemIdentifier))
            {
                i--;
                continue;
            }

            GameObject go = Instantiate(shoppingItem, listHolder);
            ItemUI ui = go.GetComponent<ItemUI>();
            ui.Setup(item);

            collectItems.Add(item.itemIdentifier, ui);
            //go.GetComponent<TextMeshProUGUI>().text = item.itemIdentifier;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("OpenList"))
        {
            listHolder.gameObject.SetActive(!listHolder.gameObject.activeSelf);
        }
    }

    public void Checkout()
    {
        if(HasAllRequiredItems())
        {
            HasCheckedOut = true;
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
                collectItems[item.itemIdentifier].yellowIcon.SetActive(true);
                collectItems[item.itemIdentifier].whiteIcon.SetActive(false);
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
                collectItems[item.itemIdentifier].yellowIcon.SetActive(false);
                collectItems[item.itemIdentifier].whiteIcon.SetActive(true);
                collected.Remove(item.itemIdentifier);
            }
        }
    }

    public bool HasAllRequiredItems()
    {
        bool hasAll = true;

        foreach (string item in collectItems.Keys)
        {
            if (!collected.Contains(item))
            {
                hasAll = false;
            } else
            {
                collectItems[item].greenIcon.SetActive(true);
                collectItems[item].yellowIcon.SetActive(false);
            }
        }

        return hasAll;
    }
}
