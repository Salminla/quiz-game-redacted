using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class Waypoint : MonoBehaviour
{
    public RoadType roadType;
    public int travelTime = 5;
    public bool specialLocation = false;
    public bool useCustomLabel;
    public string customLabel;
    public SpecialLocation locationType = SpecialLocation.Default;
    public UnityEvent arrived;
    
    private void OnEnable()
    {
        SetName();
    }

    void SetName()
    {
        if (!specialLocation)
        {
            if (roadType == null)
            {
                Debug.Log("No type set for " + transform.name);
                transform.name = "Waypoint (NO TYPE)";
            }
            else
                transform.name = "Waypoint (" + roadType.name + ")";
            return;
        }

        transform.name = locationType.ToString();

    }

    public string GetName()
    {
        if (!useCustomLabel)
            customLabel = locationType.ToString();

        return customLabel;
    }
    // This should probably be elsewhere... (editor scipt?)
    #if UNITY_EDITOR
    public void CreatePoint(WaypointDirection dir)
    {
        Transform waypointContainer = transform.parent;
        foreach (var o in waypointContainer)
        {
            if ((Transform)o != transform) continue;
            GameObject instantiatedObject = (GameObject)PrefabUtility.InstantiatePrefab(PrefabUtility.GetCorrespondingObjectFromSource(gameObject), waypointContainer);
            Undo.RegisterCreatedObjectUndo(instantiatedObject, "Create Waypoint");
            if (dir == WaypointDirection.Forward)
            {
                instantiatedObject.transform.position = gameObject.transform.position + Vector3.right;
                instantiatedObject.transform.SetSiblingIndex(transform.GetSiblingIndex()+1);
            }
            else
            {
                // TODO: This is broken, subtracting from siblingIndex does not work...
                instantiatedObject.transform.position = gameObject.transform.position + Vector3.left;
                instantiatedObject.transform.SetSiblingIndex(transform.GetSiblingIndex()+1);
            }
        }
    }
    #endif
}

public enum WaypointDirection
{
    Forward,
    Backward
}