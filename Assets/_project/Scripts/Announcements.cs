using System.Collections.Generic;
using UnityEngine;

public class Announcements : MonoBehaviour
{
    public Transform destinationContainer;
    private List<Transform> destinations;
    void Start()
    {
        foreach (Transform trans in destinationContainer)
            destinations.Add(trans);
        
    }
    
    void Update()
    {
        
    }
}
