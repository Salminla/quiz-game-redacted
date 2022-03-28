using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    private SerializedObject so;
    private SerializedProperty propRoadType;
    private SerializedProperty propSpecialLocation;
    private SerializedProperty propLocationType;
    private SerializedProperty propTravelTime;
    private SerializedProperty propUseCustomLabel;
    private SerializedProperty propCustomLabel;
    private SerializedProperty propArrived;

    private void OnEnable()
    {
        so = serializedObject;
        
        propRoadType = so.FindProperty("roadType");
        propSpecialLocation = so.FindProperty("specialLocation");
        propLocationType = so.FindProperty("locationType");
        propTravelTime = so.FindProperty("travelTime");
        propUseCustomLabel = so.FindProperty("useCustomLabel");
        propCustomLabel = so.FindProperty("customLabel");
        propArrived = so.FindProperty("arrived");

    }

    public override void OnInspectorGUI()
    {
        Waypoint waypoint = target as Waypoint;
        
        so.Update();
        
        EditorGUILayout.PropertyField(propRoadType);
        EditorGUILayout.PropertyField(propSpecialLocation);
        
        if (propSpecialLocation.boolValue)
        {
            EditorGUILayout.LabelField("Travel time to this destination");
            EditorGUILayout.PropertyField(propTravelTime);
            EditorGUILayout.PropertyField(propLocationType);
            EditorGUILayout.PropertyField(propUseCustomLabel);
            if (propUseCustomLabel.boolValue)
                EditorGUILayout.PropertyField(propCustomLabel);
        }
        EditorGUILayout.Separator();
        GUILayout.Label("Add actions that fire at waypoint arrival.");
        GUILayout.Label("(NOTE: In practice this fires at the preceding waypoint)");
        EditorGUILayout.PropertyField(propArrived);
        
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add front"))
        {
            waypoint.CreatePoint(WaypointDirection.Forward);
        }
        EditorGUILayout.EndHorizontal();
        
        so.ApplyModifiedProperties();
    }
}
