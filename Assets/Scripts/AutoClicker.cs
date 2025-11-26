using System;
using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Score;
    private float interval;

    private MainStream Stream;

    Click ClickableButton;


    void Awake()
    {
        ClickableButton = GameObject.Find("MainButton").GetComponent<Click>();
        Stream = MainStream.selfMain;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Stream.autoClicker) return;

        interval += Time.deltaTime;

        if (interval > Stream.autoClickerSpeed)
        {
            interval = 0f;
            ClickableButton.Clicked();
        }
    }
}
