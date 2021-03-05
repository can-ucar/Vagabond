using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BlockSpawner _blockSpawner;
    private BallDirectionPreview BallDirectionPreview;
    private Rigidbody2D _rigidbody2D;
    private Camera cam;
    [SerializeField] private GameObject ballPrefab;
    private Vector3 startDragposition;
    private Vector3 endDragPosition;
    private Vector3 playerBallPosition;
    private Vector2 fireDirection;
    [SerializeField]private int moveSpeed;
    [SerializeField]private int BallAmount;
    [SerializeField]private int LandedBalls;
    [SerializeField]private GameObject ballParent;
    //Balls List
    [SerializeField]private List<GameObject> balls = new List<GameObject>();
    
    // Observer Pattern
    public static event Action<Vector3> OnTargettingEnabled;
    public static event Action IsLevelUp;
    public static event Action IsResetPlayer;
    
    //Finite State Machine
    public enum ballState
    {
        aimReady,
        fired,
        wait,
        endShot
    }
    public ballState currentBallState;
    
    
    void Awake()
    {
        GetComponents();
    }

    void Start()
    {
        BallSpawnPoint.LandedBallsCount += CountLandedBalls;
        BallController.IsColliding += ResetPlayerPosition;
        currentBallState = ballState.aimReady;
    } 
    void OnDisabled()
    {
        BallSpawnPoint.LandedBallsCount -= CountLandedBalls;
        BallController.IsColliding -= ResetPlayerPosition;
    }

    void GetComponents()
    {
        cam = FindObjectOfType<Camera>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        BallDirectionPreview = FindObjectOfType<BallDirectionPreview>();
        //
        BallAmount = DataReceiver.GetBallAmount();
    }

    void Update()
    {
        Vector3 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        DetectLastObjectIsLanded();
        switch (currentBallState)
        {
            case ballState.aimReady:
                if (!DataReceiver.GetGameStatus())
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        StartDrag(worldPoint);
                    }

                    if (Input.GetMouseButton(0))
                    {
                        ContinueDrag(worldPoint);
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        EndDrag(worldPoint);
                    }
                }

                break;
            case ballState.fired:
                
                break;
            case ballState.wait:
                
                break;
            case ballState.endShot:
                
                break;
            default:
                break;
        }
    }
    
    private void StartDrag(Vector3 worldPoint)
    {
        startDragposition = worldPoint;
        playerBallPosition = transform.position;
        BallDirectionPreview.SetStartPoint(transform.position);
        OnTargettingEnabled?.Invoke(worldPoint);
    }

    private void ContinueDrag(Vector3 worldPoint)
    {
        endDragPosition = worldPoint;
        Vector3 direction = endDragPosition - startDragposition;
        BallDirectionPreview.SetEndPoint(transform.position -direction);
    }
    
    private void EndDrag(Vector3 worldPoint)
    {
        BallAmount = DataReceiver.GetBallAmount();
        StartCoroutine(SpawnBallsToDirection(worldPoint));
        BallDirectionPreview.ResetLine();
        LandedBalls = 0;
    }

    //Move spawned balls to target direction
    private IEnumerator SpawnBallsToDirection(Vector3 worldPoint)
    {
        Vector3 endPos = worldPoint;
        Vector2 direction = startDragposition - endPos;
        direction.Normalize();
        fireDirection = direction;
        float ballGap = 0.05f;
        
        if (fireDirection != Vector2.zero)
        {   
            ParticleManager.RunParticle(gameObject.transform.position,ParticleManager.ParticleType.Magic);
            UIManager.BallTextController();
            for (int i = 0; i < BallAmount; i++)
            {
                yield return new WaitForSeconds(ballGap);
                GameObject ball = LeanPool.Spawn(ballPrefab, playerBallPosition, Quaternion.identity,ballParent.transform);
                ball.GetComponent<Rigidbody2D>().velocity = fireDirection * moveSpeed;
                balls.Add(ball);
                currentBallState = ballState.fired;
            }
        }
    }
    
    //Set Landed Balls
    private void CountLandedBalls()
    {
        LandedBalls++;
    }
    
    //Detect the last of object of Balls List
    private void DetectLastObjectIsLanded()
    {
        if (currentBallState != ballState.aimReady)
        {
            if (BallAmount == LandedBalls)
            {
                currentBallState = ballState.aimReady;
                UIManager.BallTextController();
                _blockSpawner.NextPattern();
                IsLevelUp?.Invoke();
                RespawnPlayer(balls[BallAmount-1].transform.localPosition);
            }
        }
        
    }
    
    //Respawn player(main ball)
    private void RespawnPlayer(Vector3 respawnPoint)
    {
        transform.localPosition = respawnPoint + new Vector3(0,2f,0);
        balls.Clear();
    }

    //Reset player position (if it stucks)
    private void ResetPlayerPosition()
    {
        currentBallState = ballState.aimReady;
        balls.Clear();
        LeanPool.DespawnAll();
        IsResetPlayer?.Invoke();
    }
}
