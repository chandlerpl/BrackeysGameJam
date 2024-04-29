using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FadeText : MonoBehaviour
{
    [SerializeField]
    private float _duration = 2f;

    private float _startAlpha;

    private TextMeshProUGUI _text;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        _startAlpha = _text.alpha;
    }

    // Start is called before the first frame update
    void Start()
    {
        _text.alpha = _startAlpha;

        StartCoroutine(UpdateText());
    }

    IEnumerator UpdateText()
    {
        float time = 0;

        while (time < 1f)
        {
            yield return new WaitForEndOfFrame();

            time += Time.deltaTime / _duration;

            _text.alpha = 1 - time;
        }
    }

}
