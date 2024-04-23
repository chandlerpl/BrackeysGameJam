using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public RawImage image;
    public TextMeshProUGUI text;
    public ItemRenderer ren;

    public void Setup(Item item)
    {
        text.text = item.itemIdentifier;
        Render(item);
    }
    public Transform parent;
    public Camera cam;

    private void Render(Item item)
    {
        GameObject go = Instantiate(item.gameObject, parent);
        go.transform.localPosition = Vector3.zero;
        parent.parent.localPosition += new Vector3(0, Random.Range(0, 100f), 0);
        StartCoroutine(Renderr());
    }

    private IEnumerator Renderr()
    {
        yield return new WaitForFixedUpdate();

        RenderTexture text = new RenderTexture(512, 512, 32);
        cam.targetTexture = text;
        cam.Render();

        image.texture = text;
        cam.transform.parent.gameObject.SetActive(false);
    }
}
