using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    public Transform pointContainer;
    public float moveSpeed = 1.0f;

    public List<Transform> points = new List<Transform>();
    public List<Vector3> pointsVec = new List<Vector3>();
    private int destinationPos = 0;
    private int currentPos = 0;
    
    private Waypoint currentPoint;

    private bool firstMovementComplete = false;
    
    private bool dirSwitched;
    private bool dirTweenComplete;

    void Start()
    {
        GetPoints();
    }

    private void Update()
    {
        SetDirection();
    }

    private void SetDirection()
    {
        Vector3 localScale = transform.localScale;
        Vector3 inverseY = new Vector3(localScale.x, -localScale.y, localScale.z);
        float angleCos = Mathf.Cos(transform.rotation.eulerAngles.z * (Mathf.PI / 180));
        if (angleCos < -0.2 && !dirSwitched && !dirTweenComplete)
        {
            transform.DOScale(inverseY, 0.05f).OnComplete(() => dirTweenComplete = true);
            dirSwitched = true;
        }
        if (angleCos > 0.2 && dirSwitched && dirTweenComplete)
        {
            transform.DOScale(inverseY, 0.05f).OnComplete(() => dirTweenComplete = false);
            dirSwitched = false;
        }
        
    }
    
    void GetPoints()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.keyPoints.Clear();

        points.Clear();
        pointsVec.Clear();

        foreach (var pointTransform in pointContainer)
        {
            Transform addedPoint = (Transform)pointTransform;
            points.Add(addedPoint);
            pointsVec.Add(addedPoint.position);

            if (!addedPoint.GetComponent<Waypoint>().specialLocation) continue;
            if (GameManager.Instance != null)
                GameManager.Instance.keyPoints.Add(addedPoint);
        }
    }
    
    public void MoveNextLocation()
    {
        if (!firstMovementComplete)
        {
            currentPoint = points[0].GetComponent<Waypoint>();
            if (!currentPoint.specialLocation) return;
            ReachedDestination();
            firstMovementComplete = true;
            return;
        }
        
        List<Vector3> pathToDest = new List<Vector3>();
        // return if overflow
        if (destinationPos+1 > points.Count)
            return;
        for (var i = destinationPos+1; i < points.Count; i++)
        {
            Transform point = points[i];
            currentPoint = point.GetComponent<Waypoint>();
            pathToDest.Add(point.transform.position);
            destinationPos = i;
            if (!currentPoint.specialLocation) continue;
            GameManager.Instance.MovingToLocation(currentPoint);
            moveSpeed = currentPoint.travelTime;
            break;
        }
        // tween to the next location
        transform.DOPath(pathToDest.ToArray(), moveSpeed, PathType.CatmullRom, PathMode.TopDown2D)
            .OnWaypointChange(ReachedWaypoint)
            .OnComplete(ReachedDestination)
            .SetLookAt(0.01f)               // Could be adjustable by path length...
            .SetEase(Ease.InOutQuad); 
    }

    void ReachedWaypoint(int index)
    {
        SetVehicle(index);
        // Fire the event in the current waypoint...
        Waypoint realCurrentPoint = points[currentPos].GetComponent<Waypoint>();
        realCurrentPoint.arrived.Invoke();
    }
    // This method is going to get called from elsewhere
    void ReachedDestination()
    {
        currentPos = destinationPos;
        GameManager.Instance.ReachedLocation(currentPoint);
    }

    void SetVehicle(int pos)
    {
        currentPos++;
        Waypoint realCurrentPoint = points[currentPos].GetComponent<Waypoint>();
        GameManager.Instance.ChangeVehicle(realCurrentPoint.roadType.vehicle);
    }
    public void PlaceAtStart()
    {
        DOTween.KillAll();
        transform.position = points[0].position;
        destinationPos = 0;
        currentPos = 0;
        firstMovementComplete = false;
    }
    private void OnDrawGizmos()
    {
        GetPoints();

        //Draw the Catmull-Rom spline between the points
        for (int i = 0; i < points.ToArray().Length; i++)
        {
            //Cant draw between the endpoints
            //Neither do we need to draw from the second to the last endpoint
            if ((i == 0 || i == points.ToArray().Length - 2 || i == points.ToArray().Length - 1))
            {
                continue;
            }

            CatmullRomSpline.DisplayCatmullRomSpline(points.ToArray(), i);
        }
    }
}
