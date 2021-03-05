using System;
using UnityEngine;

public class BallController : MonoBehaviour
{

    private Rigidbody2D _rigidbody2D;
    [SerializeField] private int moveSpeed;
    private Vector3 m_dirReflect;
    private int _collisionDetect;

    public static event Action IsColliding;
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        PlayerController.IsLevelUp += ResetCounter;
        PlayerController.IsResetPlayer += ResetCounter;
    }

    private void ResetCounter()
    {
        int zero = 0;
        _collisionDetect = zero;
    }

    void Update()
    {
        _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * moveSpeed;
    }

    void IfNotCollidingBlocks()
    {
        int zero = 0;
        if (_collisionDetect == 20)
        {
            IsColliding?.Invoke();
            _collisionDetect = zero;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        int zero = 0;
        if (other.gameObject.CompareTag("Wall"))
        {
            _collisionDetect++;
            IfNotCollidingBlocks();
        }
        else if (other.gameObject.CompareTag("Block"))
        {
            _collisionDetect = zero;
        }
    }


}
