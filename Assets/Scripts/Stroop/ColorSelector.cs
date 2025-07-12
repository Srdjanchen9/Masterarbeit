using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
    public string colorName; // „rot“, „grün“, „blau“

    public void OnSelected()
    {
        FindObjectOfType<StroopManager>().EvaluateAnswer(colorName);
    }
}
