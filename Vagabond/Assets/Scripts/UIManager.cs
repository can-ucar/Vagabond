using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI mlevelText;
    public TextMeshProUGUI mtopLevelText;
    public TextMeshProUGUI mmoneyText;
    public TextMeshProUGUI mballText;

    public Button pauseButton;
    public Button playButton;
    public Button howToButton;
    public GameObject soundButton;
    public Sprite soundOff;
    public Sprite soundOn;
    public GameObject pausePanel;
    public GameObject gameoverPanel;

    private SFXControl _sfxControl;

    public enum GameStatus
    {
        Play,
        Pause
    }

    public GameStatus currentGameStatus;

    public enum TextType
    {
        Level,
        TopLevel,
        Money,
        Ball
    }
    
    static Dictionary<TextType,TextMeshProUGUI> _dic = new Dictionary<TextType, TextMeshProUGUI>();

    void Awake()
    {
        _sfxControl = FindObjectOfType<SFXControl>();
        _dic.Clear();
        AddDictionary(TextType.Level,mlevelText);
        AddDictionary(TextType.TopLevel,mtopLevelText);
        AddDictionary(TextType.Money,mmoneyText);
        AddDictionary(TextType.Ball,mballText);
    }

    void AddDictionary(TextType textType, TextMeshProUGUI tmp)
    {
        if (_dic.ContainsKey(textType))
        {
            _dic[textType] = tmp;
        }
        else
            _dic.Add(textType, tmp);
    }
    void Start()
    {
        SetTexts();
        SoundButtonCheck();
        currentGameStatus = GameStatus.Play;
        BallSpawnPoint.IsGameOver += GameOver;
        BlockSpawner.IsLevelsFinished += GameOver;
    }
    
    void OnDisable()
    {
        BallSpawnPoint.IsGameOver -= GameOver;
        BlockSpawner.IsLevelsFinished -= GameOver;
    }

    public static void TextUpdate(TextType textType, string text)
    {
        _dic[textType].text = text;
    }

    private void SetTexts()
    {
        _dic[TextType.Level].text = ""+DataReceiver.GetLevel();
        _dic[TextType.TopLevel].text = "TOP "+DataReceiver.GetTopGameLevel();
        _dic[TextType.Money].text = "x"+DataReceiver.GetMoney();
        _dic[TextType.Ball].text = "x"+DataReceiver.GetBallAmount();
    }

    void SoundButtonCheck()
    {
        soundButton.GetComponent<Image>().sprite = DataReceiver.GetSoundStatus() ? soundOn : soundOff;
    }
    

    public static void BallTextController()
    {
        _dic[TextType.Ball].gameObject.SetActive(!_dic[TextType.Ball].gameObject.activeSelf);
    }

    public void GameController()
    {
        switch (currentGameStatus)
        {
            case GameStatus.Play:
                Time.timeScale = 0f;
                DataReceiver.ChangeGameStatus();
                pausePanel.SetActive(true);
                pauseButton.gameObject.SetActive(false);
                playButton.gameObject.SetActive(true);
                currentGameStatus = GameStatus.Pause;
                break;
            case GameStatus.Pause:
                pausePanel.SetActive(false);
                pauseButton.gameObject.SetActive(true);
                playButton.gameObject.SetActive(false);
                currentGameStatus = GameStatus.Play;
                DataReceiver.ChangeGameStatus();
                Time.timeScale = 1f;
                break;
            default:
                break;
        }
    }

    public void GoMainMenu()
    {
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        currentGameStatus = GameStatus.Play;
        DataReceiver.ChangeGameStatus();
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void SoundController()
    {
        if (!DataReceiver.GetSoundStatus())
        {
            DataReceiver.ChangeSoundStatus();
            soundButton.GetComponent<Image>().sprite = soundOn;
            _sfxControl.PlaySfx(SFXControl.SfxNames.Music);
            _sfxControl.SfxEventON();
        }
        else
        {
            DataReceiver.ChangeSoundStatus();
            soundButton.GetComponent<Image>().sprite = soundOff;
            _sfxControl.StopSfx(SFXControl.SfxNames.Music);
            _sfxControl.SfxEventOFF();
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        pauseButton.gameObject.SetActive(false);
        howToButton.gameObject.SetActive(false);
        gameoverPanel.SetActive(true);
        DataReceiver.ResetData();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(2);
    }

}
