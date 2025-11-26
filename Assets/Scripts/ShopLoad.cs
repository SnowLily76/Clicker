using System;
using TMPro;
using UnityEngine;
using static UnityEngine.Debug;

public class ShopLoad : MonoBehaviour
{
    [System.Serializable]
    private class PriceList
    {
        public int clicksPrice;
        public int multiplierPrice;
        public int autoClickerPrice;
    }

    [System.Serializable]
    private struct Levels
    {
        public int ClickLevel;
        public int MultiplierLevel;
        public int AutoClickerLevel;
    }

    [System.Serializable]
    public struct Scalings
    {
        public float ClickScaling;
        public float MultiplierScaling;
        public float AutoClickerFirstScaling;
        public float AutoClickerScaling;
    }

    private MainStream Stream;
    public GameObject ClickUpgrade;
    public GameObject MultiplerUpgrade;
    public GameObject AutoClickerUpgrade;
    public TextMeshProUGUI ScoreField;

    public Scalings scalings;
    private Levels levels;

    private PriceList Prices;


    /* 
    Paths:
    0 - Description
    0:0 - Description/UpgradeName
    0:1 - Description/Cost

    2 - Payment
    2:0 - Payment/CurrentOfThatThingForUser
    */ 
    private void UpdateField(GameObject Label, String type, String textValue)
    {
        switch (type)
        {
            case "Current":
                TextMeshProUGUI field = Label.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
                field.text = textValue;
                break;
            case "Cost":
                field = Label.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
                field.text = textValue;
                break;
            default:
                ScoreField.text = textValue;
                break;
        }
    }


    void Awake()
    {
        // Gets Global variables
        Stream = MainStream.selfMain;

        // .GetChild(2).GetChild(0) Accesses the Label to Each Multiplier
        UpdateField(ClickUpgrade, "Current", Stream.clicks.ToString());
        UpdateField(MultiplerUpgrade, "Current", $"x{Stream.multiplier}");
        UpdateField(AutoClickerUpgrade, "Current", Stream.autoClicker ? $"CPS {Stream.autoClickerSpeed}" : "LOCKED");

        // Gets Price JSON Sheet
        TextAsset priceSheet = Resources.Load<TextAsset>("JSON/Pricesheet");
        Prices = JsonUtility.FromJson<PriceList>(priceSheet.text);

        // Sets Base Levels
        levels.ClickLevel = 1;
        levels.MultiplierLevel = 1;
        levels.AutoClickerLevel = 1;
    }

    public void UpgradeClicks()
    {
        if (Stream.score < Prices.clicksPrice) return;

        Stream.score -= Prices.clicksPrice;
        Stream.clicks += 1;
        levels.ClickLevel += 1;

        Prices.clicksPrice = (int)(5 * Mathf.Pow(scalings.ClickScaling, levels.ClickLevel));

        UpdateField(ClickUpgrade, "Current", Stream.clicks.ToString());
        UpdateField(ClickUpgrade, "Cost", $"S{Prices.clicksPrice}");
        UpdateField(null, null, Stream.score.ToString());
    }

    public void UpgradeMultiplier()
    {
        System.Random random = new System.Random();

        if (Stream.score < Prices.multiplierPrice) return;

        Stream.score -= Prices.multiplierPrice;

        levels.MultiplierLevel += 1;
        Stream.multiplier += random.Next(25, 75)/100f;

        Prices.multiplierPrice =(int)(25 * Mathf.Pow(scalings.MultiplierScaling, levels.MultiplierLevel));

        UpdateField(MultiplerUpgrade, "Current", $"x{Stream.multiplier}");
        UpdateField(MultiplerUpgrade, "Cost", $"S{Prices.multiplierPrice}");
        UpdateField(null, null, Stream.score.ToString());
    }

    private void UpgradeAutoClicker()
    {
        if (Stream.autoClickerSpeed < 0.15) return;
        if (Stream.score < Prices.autoClickerPrice) return;

        Stream.score -= Prices.autoClickerPrice;
        Stream.autoClickerSpeed -= 0.15f;
        levels.AutoClickerLevel += 1;

        Prices.autoClickerPrice =(int)(125 * Mathf.Pow(scalings.AutoClickerScaling, levels.AutoClickerLevel));

        UpdateField(AutoClickerUpgrade, "Current", $"CPS {Stream.autoClickerSpeed}");
        UpdateField(AutoClickerUpgrade, "Cost", $"S{Prices.autoClickerPrice}");
        UpdateField(null, null, Stream.score.ToString()); 
    }

    public void BuyAutoClicker()
    {
        if (Stream.autoClicker)
        {
            UpgradeAutoClicker();
            return;
        }

        if (Stream.score < Prices.autoClickerPrice) return;

        Stream.score -= Prices.autoClickerPrice;
        Prices.autoClickerPrice = Mathf.RoundToInt(Prices.autoClickerPrice * scalings.AutoClickerFirstScaling);

        UpdateField(AutoClickerUpgrade, "Current", $"CPS {Stream.autoClickerSpeed}");
        UpdateField(AutoClickerUpgrade, "Cost", $"S{Prices.autoClickerPrice}");
        UpdateField(null, null, Stream.score.ToString()); 

        Stream.autoClicker = true;
    }
}
