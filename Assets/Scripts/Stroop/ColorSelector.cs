using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
    public string colorName; // „rot“, „grün“, „blau“

    public void OnSelected()
    {
        StroopManager manager = FindObjectOfType<StroopManager>();
        if (manager != null && manager.InputAllowed()) // nur wenn erlaubt
        {
            manager.EvaluateAnswer(colorName);
        }
    }

}
