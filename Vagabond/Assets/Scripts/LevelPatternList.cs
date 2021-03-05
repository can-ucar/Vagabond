using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelPatternList", menuName = "ScriptableObjects/Level/LevelPatternList")]
public class LevelPatternList : ScriptableObject
{

    public List<PatternList> levelPatterns;
}
