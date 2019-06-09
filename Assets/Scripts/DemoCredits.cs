using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class DemoCredits : MonoBehaviour
{
    public GameObject restartBtn, creditsVideo;
    private bool hasPlayed;

    private void Start()
    {
        restartBtn.SetActive(false);
        creditsVideo.GetComponent<VideoPlayer>().Play();
    }

    private void Update()
    {
        if (creditsVideo.GetComponent<VideoPlayer>().isPlaying)
        {
            hasPlayed = true;
        }

        if (!creditsVideo.GetComponent<VideoPlayer>().isPlaying && hasPlayed)
        {
            showReplay();
            hasPlayed = false;
        }
    }

    private void showReplay()
    {
        restartBtn.SetActive(true);
    }

    public void replayDemo()
    {
        Application.Quit();
    }
}
