using UnityEngine;

[CreateAssetMenu(fileName = "Input_Data")]
public class InputData : ScriptableObject
{
    public bool isMusicPlaying;
    public int gameLevel;
    public int topGameLevel;
    public int totalMoney;
    public int patternNumber;
    public bool isPatternHas;
    public int ballAmount;
    public bool isGamePaused;

}
