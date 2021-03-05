using System;
using UnityEngine;

public class BlockMovementController : MonoBehaviour
{
    void Start()
    {
        PlayerController.IsLevelUp += MoveBlocks;
    }

    void OnDisable() => PlayerController.IsLevelUp -= MoveBlocks;

    private void MoveBlocks()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - 0.8f);
    }
}
