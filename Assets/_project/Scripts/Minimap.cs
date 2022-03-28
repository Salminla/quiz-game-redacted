using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Sprite targetPoint;
    public Sprite visitedPoint;
    public Transform pointContainer;
    public float initialScale = 5f;
    public float visitedScale = 4f;
    
    private List<Transform> keypoints = new List<Transform>();
    private List<GameObject> runningPoints = new List<GameObject>();
    private GameObject previousPoint;
    private GameObject currentPoint;

    private void Start()
    {
        StartCoroutine(GetPoints());
    }
    void PopulatePoints()
    {
        foreach (Transform keypoint in keypoints)
        {
            CreateMapPoint(keypoint, keypoint.position, pointContainer);
        }
    }

    private GameObject CreateMapPoint(Transform keypoint, Vector3 pos, Transform parent)
    {
        
        GameObject newPoint = new GameObject(keypoint.name)
        {
            layer = 6,
            transform =
            {
                localScale = Vector3.one * initialScale,
                position = pos,
                parent = parent
            }
        };
        SpriteRenderer spriteRenderer = newPoint.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = targetPoint;
        spriteRenderer.sortingOrder = 1;
        return newPoint;
    }

    public void HighlightPoint(Waypoint point)
    {
        runningPoints.Add(CreateMapPoint(point.transform, point.transform.position, pointContainer));
        if (runningPoints.Count > 1)
        {
            runningPoints[runningPoints.Count - 2].GetComponent<SpriteRenderer>().sprite = visitedPoint;
            runningPoints[runningPoints.Count - 2].transform.localScale = Vector3.one * visitedScale;
        }
    }
    public void ClearPoints()
    {
        runningPoints.Clear();
        foreach (Transform point in pointContainer)
            Destroy(point.gameObject);
    }
    IEnumerator GetPoints()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameManager.Instance.keyPoints.Count > 0)
            keypoints = GameManager.Instance.keyPoints;
        //PopulatePoints();
    }
}
