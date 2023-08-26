using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionPrompt : MonoBehaviour
{
    private TextMeshProUGUI text;
    public string objString;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void activate(string str)
    {
        objString = str;
        text.text = objString;
    }

    public void deactivate()
    {
        objString = "";
        text.text = objString;
    }
}
