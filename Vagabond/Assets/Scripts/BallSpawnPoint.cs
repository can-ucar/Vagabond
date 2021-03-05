using System;
using Lean.Pool;
using UnityEngine;

public class BallSpawnPoint : MonoBehaviour
{
    
    // Observer Pattern
    public static event Action LandedBallsCount;
    public static event Action IsGameOver;
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            LandedBallsCount?.Invoke();
            LeanPool.Despawn(other.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            IsGameOver?.Invoke();
        }
    }
}
