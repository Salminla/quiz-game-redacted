using UnityEngine;

[CreateAssetMenu]
public class RoadType : ScriptableObject
{
    public AudioClip sound;
    public Vehicle vehicle;
}

public enum Vehicle
{
    Boat,
    Kayak,
    Train,
    Bus,
    Bicycle
}
