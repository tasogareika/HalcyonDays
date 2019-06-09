using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class ObjectPulse : MonoBehaviour
{
    private Image thisImage;
    private bool pulse, reverse;
    private float counter;
    public float duration;

    private void Start()
    {
        thisImage = GetComponent<Image>();
        thisImage.color = Color.clear;
        counter = 0;
        pulse = false;
        reverse = true;
    }

    public void startPulse()
    {
        pulse = true;
    }

    public void stopPulse()
    {
        thisImage.color = Color.clear;
        pulse = false;
        reverse = true;
    }

    private void Update()
    {
        if (pulse)
        {
            switch (reverse)
            {
                case true:
                    if (counter < duration)
                    {
                        counter += Time.deltaTime;
                        float alpha = Mathf.Lerp(0, 1, counter / duration);

                        thisImage.color = new Color(1, 1, 1, alpha);
                    }
                    else
                    {
                        counter = 0;
                        reverse = false;
                    }
                    break;

                case false:
                    if (counter < duration)
                    {
                        counter += Time.deltaTime;
                        float alpha = Mathf.Lerp(1, 0, counter / duration);

                        thisImage.color = new Color(1, 1, 1, alpha);
                    } else
                    {
                        counter = 0;
                        reverse = true;
                    }
                    break;
            }
        }
    }
}
