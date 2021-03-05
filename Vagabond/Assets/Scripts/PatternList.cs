using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PatternList", menuName = "ScriptableObjects/Level/PatternList", order = 1)]
public class PatternList : ScriptableObject
{
    public enum SpawnPoints
    {
        Left1,
        Left2,
        Left3,
        Mid,
        Right3,
        Right2,
        Right1
    }
    [System.Serializable]
    public class LevelPattern
    {
        [System.Serializable]
        public class Pattern
        {
            public SpawnPoints spawnPoints;
            public BlockController.BlockType blockType;
            public int extraHitPoint;
        }

        public List<Pattern> patterns = new List<Pattern>();
    }

    public List<LevelPattern> levelPatterns;
}
