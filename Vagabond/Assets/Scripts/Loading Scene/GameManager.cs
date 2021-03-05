using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get { return _instance ?? (_instance = new GameManager()); }
        }

        
        // Start is called before the first frame update
        void Awake()
        {
            _instance = this;
            QualitySettings.vSyncCount = 0;
            QualitySettings.antiAliasing = 0;
            Application.targetFrameRate = 60; 
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            DontDestroyOnLoad(gameObject);
        
            DOTween.Init();
            DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(100, 10);
        }
    }

