using UnityEngine;

[CreateAssetMenu(fileName = "MainStream", menuName = "Scriptable Objects/MainStream")]
public class MainStream : ScriptableObject
{
    public static MainStream selfMain;

    public int score = 0;
    public int clicks = 1;
    public float multiplier = 0f;
    public bool autoClicker = false;
    public float autoClickerSpeed = 2.5f;
    public float critChance = 5f;
    public int critDamage = 500;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        selfMain = Instantiate(Resources.Load<MainStream>("GlobalMain"));
    }
}