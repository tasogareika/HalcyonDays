using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacOSEvents : MonoBehaviour
{
    public static MacOSEvents singleton;
    public GameObject demoEndingBox;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        demoEndingBox.SetActive(false);
    }

    public void showEndingBox()
    {
        demoEndingBox.SetActive(true);
    }
}
