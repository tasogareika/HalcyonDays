using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingRing : MonoBehaviour
{
    public Image blueRing, redRing;
    public Sprite glitchRing;
    private Sprite normRing;
    private float timer, division;
    public bool loading, reverse;
    public GameObject loadingText;

    private void Start()
    {
        loading = false;
        reverse = false;
        normRing = blueRing.sprite;
        timer = Random.Range(60, 90);
        timer = Mathf.Round(100 / timer);
        timer = timer * 0.01f;
        redRing.fillAmount = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && Application.platform == RuntimePlatform.WindowsEditor)
        {
            toggleLoading();
        }

        if (loading)
        {
            if (!reverse)
            {
                LEDRingLoad(1);
            } else
            {
                LEDRingLoad(2);
            }
        }
    }

    public void toggleLoading()
    {
        blueRing.sprite = normRing;
        if (loading)
        {
            loading = false;
            //loadingText.GetComponent<TypewriterText>().stopText();
        } else if (!loading)
        {
            loading = true;
            //loadingText.GetComponent<TypewriterText>().refreshText();
        }
    }

    private void LEDRingLoad(int ring)
    {
        switch (ring) { 
            case 1:
                redRing.fillAmount += timer;
                if (redRing.fillAmount >= 1)
                {
                    reverse = true;
                    redRing.fillClockwise = false;
                }
                break;

            case 2:
                redRing.fillAmount -= timer;
                if (redRing.fillAmount <= 0)
                {
                    reverse = false;
                    redRing.fillClockwise = true;
                }
                break;
        }
    }
}
