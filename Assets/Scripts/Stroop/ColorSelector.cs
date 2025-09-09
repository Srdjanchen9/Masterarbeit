using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
    public string colorName; // 

    public void OnSelected()
    {
        StroopManager manager = FindObjectOfType<StroopManager>();
        if (manager != null && manager.InputAllowed()) 
        {
            manager.EvaluateAnswer(colorName);
        }
    }

}
