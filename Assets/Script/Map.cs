using UnityEngine;

[CreateAssetMenu(fileName = "newMap", menuName = "Map")]
public class Map : ScriptableObject
{
    public byte star;
    public bool isMapComplete;
    public new string name;
    public byte[] starPoint;
    public byte highScore;

}
