using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawners;
    [SerializeField] private GameObject squareBlock;
    [SerializeField] private GameObject octagoneBlock;
    [SerializeField] private GameObject extraBall;
    [SerializeField] private GameObject money;
    [SerializeField] private GameObject bomb;
    public List<GameObject> activeBlocks;

    [Header("Pattern Controllers")]
    [SerializeField] private LevelPatternList[] _levelPatternList;
    private int _levelCounter;
    private int _patternCounter;
    private bool gameEnd;
    private int _patternNumber;

    public static event Action IsLevelsFinished;
    public static event Action<int> RunSfx; 

    void Awake()
    {
        _levelCounter = DataReceiver.GetLevel();
        PatternNumberCheck();
    }    

    void Start()
    {
        NextPattern();
    }
    
    private void PatternNumberCheck()
    {
        if (DataReceiver.GetPatternStatus()==false)
        {
            _patternNumber = Random.Range(0,3);
            DataReceiver.SetPatternNumber(_patternNumber);
            DataReceiver.ChangePatternStatus();
        }
        else
        {
            _patternNumber = DataReceiver.GetPatternNumber();
        }
    }

    private void PatternContol()
    {
        if (!gameEnd)
            if (_patternCounter < _levelPatternList[_patternNumber].levelPatterns[_levelCounter].levelPatterns.Count)
            {
                PatternSpawn();
            }
            else
                LevelControl();
    }

    private void LevelControl()
    {
        _patternCounter = 0;
        _levelCounter++;
        NextPattern();
        RunSfx?.Invoke(4);
        if (_levelCounter >= _levelPatternList[_patternNumber].levelPatterns.Count)
        {
            gameEnd = true;
            IsLevelsFinished?.Invoke();
        }
        else
        {
            DataReceiver.SetLevel(1);
            UIManager.TextUpdate(UIManager.TextType.Level,""+DataReceiver.GetLevel());
            UIManager.TextUpdate(UIManager.TextType.TopLevel,"TOP "+DataReceiver.GetTopGameLevel());
        }
            
    }

    private void PatternSpawn()
    {
        PatternList.LevelPattern levelPattern = _levelPatternList[_patternNumber].levelPatterns[_levelCounter].levelPatterns[_patternCounter];
        foreach (var pattern in levelPattern.patterns)
        {
            switch (pattern.blockType)
            {
                case BlockController.BlockType.SquareBlock:
                    Spawner(squareBlock, (int) pattern.spawnPoints,pattern.extraHitPoint);
                    break;
                case BlockController.BlockType.OctagonBlock:
                    Spawner(octagoneBlock, (int) pattern.spawnPoints,pattern.extraHitPoint);
                    break;
                case BlockController.BlockType.Bomb:
                    Spawner(bomb, (int) pattern.spawnPoints,pattern.extraHitPoint);
                    break;
                case BlockController.BlockType.Money:
                    Spawner(money, (int) pattern.spawnPoints,pattern.extraHitPoint);
                    break;
                case BlockController.BlockType.ExtraBall:
                    Spawner(extraBall, (int) pattern.spawnPoints,pattern.extraHitPoint);
                    break;
            }
        }
        _patternCounter++;
    }

    private void Spawner(GameObject prefab, int spawnPoint,int extraHitPoint)
    {
        GameObject a = Instantiate(prefab, spawners[spawnPoint].localPosition, Quaternion.identity,
            spawners[spawnPoint].transform);
        a.transform.localPosition = Vector3.zero;
        a.GetComponent<BlockController>().AddHealth(_levelCounter + 3 + extraHitPoint);
        activeBlocks.Add(a);
    }

    public void NextPattern()
    {
        PatternContol();
    }
}
