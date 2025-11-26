using UnityEngine;
using TMPro;
using static UnityEngine.Debug;
using System;
using JetBrains.Annotations;

public class Click : MonoBehaviour
{
    private MainStream Stream;
    public TextMeshProUGUI Score;

    System.Random random;

    public void Awake()
    {
        Stream = MainStream.selfMain;
        Score.text = Stream.score.ToString();
    }

    public void Clicked()
    {
        random = new System.Random();
        float chance = random.Next(100)/100f;

        if (chance <= Stream.critChance / 100f)
        {
            int Hit = Mathf.RoundToInt(Stream.clicks * ((1 + Stream.multiplier) * random.Next(200, Stream.critDamage) / 100f));
            Stream.score += Hit;
            Log($"Critical! {Hit}!");
        }
        else
        {
            Stream.score += Mathf.RoundToInt(Stream.clicks * (1 + Stream.multiplier));
        }

        Score.text = Stream.score.ToString();
    }
}
