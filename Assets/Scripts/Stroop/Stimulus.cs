using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stimulus
{
    public string word;       
    public Color color;       
    public bool isCongruent;  

    public Stimulus(string word, Color color)
    {
        this.word = word;
        this.color = color;
        this.isCongruent = (word.ToLower() == ColorUtility.ToHtmlStringRGB(color).ToLower());
    }
}
