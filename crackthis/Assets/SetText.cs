using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetText : MonoBehaviour
{
    public string textThatWannaCrypted;
    string text = "";

    int counter;
    float timer;

    public float letterAddTime;

    public char[] textChars;

    void Update()
    {
        if (textThatWannaCrypted.Length != text.Length)
        {
            timer += Time.deltaTime;
            if (timer > letterAddTime)
            {
                text += textChars[counter].ToString();
                //Debug.Log(text);
                GetComponent<TextMeshProUGUI>().text = text;
                timer = 0;
                counter++;
            }
        }
    }
}
