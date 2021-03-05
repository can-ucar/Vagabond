using UnityEngine;

public class BallDirectionPreview : MonoBehaviour
{

    private LineRenderer _lineRenderer;
    private Vector3 dragStartPoint;
    [SerializeField] private GameObject pressedAreaImage;


    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetStartPoint(Vector3 worldPos)
    {
        dragStartPoint = worldPos;
        _lineRenderer.SetPosition(0 , dragStartPoint);
        PlayerController.OnTargettingEnabled += TargettingEnabled;
    }

    private void TargettingEnabled(Vector3 worldPoint)
    {
        worldPoint = new Vector3(worldPoint.x, worldPoint.y, 0);
        pressedAreaImage.transform.SetPositionAndRotation(worldPoint, Quaternion.identity);
        pressedAreaImage.SetActive(true);
        
    }

    public void SetEndPoint(Vector3 worldPos)
    {
        Vector3 pointOffset = worldPos - dragStartPoint;
        Vector3 endPoint = transform.position + pointOffset;
        _lineRenderer.SetPosition(1, endPoint);
    }

    public void ResetLine()
    {
        _lineRenderer.SetPosition(0,Vector3.zero);
        _lineRenderer.SetPosition(1,Vector3.zero);
        pressedAreaImage.SetActive(false);
        PlayerController.OnTargettingEnabled -= TargettingEnabled;
    }
    

}
