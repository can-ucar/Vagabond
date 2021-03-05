using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    void OnEnable() => GetComponent<ParticleSystem>().Play();
    void OnDisable() => GetComponent<ParticleSystem>().Stop();
}
