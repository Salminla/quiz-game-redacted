using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathFollow))]
public class PathFollowEditor : Editor
{
    private Vector3[] pointPositions;
    
    private SerializedObject so;
    private SerializedProperty propPointContainer;
    private SerializedProperty propMoveSpeed;
    private SerializedProperty propPoints;

    private void OnEnable()
    {
        so = serializedObject;
        propPointContainer = so.FindProperty("pointContainer");
        propMoveSpeed = so.FindProperty("moveSpeed");
        propPoints = so.FindProperty("points");

        pointPositions = new Vector3[propPoints.arraySize];
        SceneView.duringSceneGui += DuringSceneGUI;
    }
    public override void OnInspectorGUI()
    {
        PathFollow pathFollow = target as PathFollow;
        
        so.Update();
        EditorGUILayout.PropertyField(propPoints);
        EditorGUILayout.PropertyField(propPointContainer);
        EditorGUILayout.PropertyField(propMoveSpeed);
        so.ApplyModifiedProperties();
    }
    private void DuringSceneGUI(SceneView obj)
    {
        PathFollow pathFollow = target as PathFollow;
        /*
        for (var i = 0; i < pathFollow.points.Count; i++)
        {
            Transform point1 = pathFollow.points[i];
            var point2 = i + 1 < pathFollow.points.Capacity ? pathFollow.points[i+1] : point1;

            Handles.DrawAAPolyLine(point1.position, point2.position);
        }
        */
    }
}
