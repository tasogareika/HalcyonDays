using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DemoTitle : MonoBehaviour
{
    public GameObject LEDRing, startBtn, blackout;
    public AudioClip glitchSFX, titleBGM;
    private AudioSource SFXPlayer;
    private float ranNo;

    private void Start()
    {
        SFXPlayer = GetComponent<AudioSource>();
        SFXPlayer.clip = titleBGM;
        SFXPlayer.Play();
        System.Random rnd = new System.Random();
        ranNo = rnd.Next(3, 6);
        blackout.SetActive(false);
    }

    public void startGame()
    {
        startBtn.SetActive(false);
        SFXPlayer.Stop();
        SFXPlayer.clip = glitchSFX;
        SFXPlayer.Play();
        StartCoroutine(ringStart(SFXPlayer.clip.length));
        LEDRing.GetComponent<ObjectFloat>().enabled = false;
        LEDRing.GetComponent<LoadingRing>().blueRing.sprite = LEDRing.GetComponent<LoadingRing>().glitchRing;
    }

    IEnumerator ringStart (float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        LEDRing.GetComponent<ObjectFloat>().enabled = true;
        LEDRing.GetComponent<LoadingRing>().toggleLoading();
        blackout.SetActive(true);
        SFXPlayer.clip = titleBGM;
        SFXPlayer.Play();
        StartCoroutine(beginFade(ranNo));
    }

    IEnumerator beginFade(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StartFade();
    }

    private void StartFade()
    {
        StartCoroutine(fadeToBlack(true));
    }

    IEnumerator fadeToBlack(bool fade)
    {
        if (fade)
        {
            for (float i = 0; i < 1.1; i += (0.5f * Time.deltaTime))
            {
                blackout.GetComponent<Image>().color = new Color(1, 1, 1, i);
                if (i >= 1)
                {
                    SceneManager.LoadScene("Demo");
                    fade = false;
                }
                yield return null;
            }
        }
    }
}
