using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    public GameObject standartParticle;
    public GameObject bombParticle;
    public GameObject magicParticle;
    public GameObject particleParent;

    public enum ParticleType
    {
        Standart,
        Bomb,
        Parent,
        Magic
    }

    private static Dictionary<ParticleType, GameObject> _dic = new Dictionary<ParticleType, GameObject>();

    void Awake()
    {
        _dic.Clear();
        AddDictionary(ParticleType.Standart,standartParticle);
        AddDictionary(ParticleType.Bomb,bombParticle);
        AddDictionary(ParticleType.Parent,particleParent);
        AddDictionary(ParticleType.Magic,magicParticle);
    }
    
    void AddDictionary(ParticleType textType, GameObject gmb)
    {
        if (_dic.ContainsKey(textType))
        {
            _dic[textType] = gmb;
        }
        else
            _dic.Add(textType, gmb);
    }

    public static void RunParticle(Vector2 pos,ParticleType particleType)
    {
        GameObject partic = LeanPool.Spawn(_dic[particleType], pos, Quaternion.identity, _dic[ParticleType.Parent].transform);
        LeanPool.Despawn(partic, 2.5f);
    }

}
