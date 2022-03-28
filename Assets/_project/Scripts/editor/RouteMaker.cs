using UnityEditor;
using UnityEngine;

public class RouteMaker : EditorWindow
{
    [MenuItem("Tools/Route Planner")]
    public static void OpenRoutePlanner() => GetWindow<RouteMaker>();
}
