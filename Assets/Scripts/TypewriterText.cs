using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterText : MonoBehaviour
{
    //from https://answers.unity.com/questions/676760/scrolling-typewriter-effect.html

    public Text textBox;
    public float timer;
    //Store all your text in this string array
    public string showText;
    private string goatText;

    private void Start()
    {
        goatText = showText;
    }

    public void stopText()
    {
        StopAllCoroutines();
        goatText = showText;
        textBox.text = "";
    }

    public void refreshText()
    {
        StopAllCoroutines();
        goatText = showText;
        textBox.text = "";
        StartCoroutine(AnimateText());
    }

    //Note that the speed you want the typewriter effect to be going at is the yield waitforseconds (in my case it's 1 letter for every      
    //0.03 seconds, replace this with a public float if you want to experiment with speed in from the editor)
    IEnumerator AnimateText()
    {
        for (int i = 0; i < (goatText.Length + 1); i++)
        {
            textBox.text = goatText.Substring(0, i);
            yield return new WaitForSeconds(timer);
        }

        if (textBox.text.Length == goatText.Length)
        {
            refreshText();
        }
    }
}
