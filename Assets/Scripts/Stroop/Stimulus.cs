using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stimulus : MonoBehaviour
{
    public string word;       // Das Wort, das angezeigt wird (z.B. "Rot")
    public Color color;       // Die Farbe, in der das Wort erscheint (z.B. blau)
    public bool isCongruent;  // Gibt an, ob Wort + Farbe kongruent sind

    public Stimulus(string word, Color color)
    {
        this.word = word;
        this.color = color;
        this.isCongruent = (word.ToLower() == ColorUtility.ToHtmlStringRGB(color).ToLower());
    }
}
