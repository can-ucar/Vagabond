using UnityEngine;

public class DataReceiver : MonoBehaviour
{
    [SerializeField]private InputData loadedInputData;
    public static InputData inputData;

    void Awake()
    {
        inputData = loadedInputData;
    }
    
    public static int GetLevel()
    {
        return inputData.gameLevel;
    }

    public static void SetLevel(int val)
    {
        inputData.gameLevel += val;
        SetTopGameLevel(GetTopGameLevel());
    }
    
    public static bool GetSoundStatus()
    {
        return inputData.isMusicPlaying;
    }

    public static void ChangeSoundStatus()
    {
        if (!inputData.isMusicPlaying)
        {
            inputData.isMusicPlaying = true;
        }
        else
        {
            inputData.isMusicPlaying = false;
        }
    }

    public static int GetBallAmount()
    {
        return inputData.ballAmount;
    }

    public static void SetBallAmount(int value)
    {
        inputData.ballAmount += value;
    }

    public static int GetPatternNumber()
    {
        return inputData.patternNumber;
    }

    public static void SetPatternNumber(int value)
    {
        inputData.patternNumber = value;
    }

    public static bool GetPatternStatus()
    {
        return inputData.isPatternHas;
    }

    public static void ChangePatternStatus()
    {
        if (!inputData.isPatternHas)
        {
            inputData.isPatternHas = true;
        }
        else
        {
            inputData.isPatternHas = false;
        }
    }

    public static int GetMoney()
    {
        return inputData.totalMoney;
    }

    public static void SetMoney(int value)
    {
        inputData.totalMoney += value;
    }

    public static int GetTopGameLevel()
    {
        return inputData.topGameLevel;
    }

    public static void SetTopGameLevel(int val)
    {
        if (val < GetLevel())
        {
            inputData.topGameLevel = GetLevel();
        }
    }

    public static bool GetGameStatus()
    {
        return inputData.isGamePaused;
    }

    public static void ChangeGameStatus()
    {
        if (inputData.isGamePaused)
        {
            inputData.isGamePaused = false;
        }
        else
        {
            inputData.isGamePaused = true;
        }
    }

    public static void ResetData()
    {
        inputData.ballAmount = 1;
        inputData.gameLevel = 0;
        inputData.isPatternHas = false;
    }

}
