using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionPrompt : MonoBehaviour
{
    private TextMeshProUGUI text;

    public int counter;
    public string objString;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void activate(string str)
    {
        if (counter == 0)
        {
            counter++;
            objString = str;
            text.text = objString;
        }
    }

    public void deactivate()
    {
        if (counter == 1)
        {
            objString = "";
            text.text = objString;
            counter--;
        }
    }
}
