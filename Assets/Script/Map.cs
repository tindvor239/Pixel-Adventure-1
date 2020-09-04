using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "newMap", menuName = "Map")]
public class Map : ScriptableObject
{
    private Image[] starImages;
    public new string name;
    public sbyte[] starPoint;
    public Type type;
    public float time;

    public Image[] StarImages
    {
        get { return starImages; }
    }
    public sbyte HighScore
    {
        get { return (sbyte)PlayerPrefs.GetInt(string.Format("{0} highScore", name)); }
        set { PlayerPrefs.SetInt(string.Format("{0} highScore", name), value); }
    }
    public bool IsMapComplete
    {
        get { return GetBoolInPlayerPrefInt(); }
        set { SetBoolInPlayerPrefInt(value); }
    }

    public Map(Image[] starImages, sbyte[] starPoint, string name, Type type, float time)
    {
        
        this.starImages = starImages;
        this.starPoint = starPoint;
        this.name = name;
        this.type = type;
        this.time = time;
        SetStar(starImages, HighScore);
    }

    public void SetStar(Image[] starImages, sbyte score)
    {
        if (starImages != null)
        {
            for (int index = 0; index < starPoint.Length; index++)
            {
                if (score >= starPoint[index])
                {
                    starImages[index].color = Color.yellow;
                }
                else
                {
                    starImages[index].color = Color.white;
                }
            }
        }
    }
    bool GetBoolInPlayerPrefInt()
    {
        int value = 0;
        string prefName = string.Format("is{0}Complete", name);
        if (PlayerPrefs.HasKey(prefName))
        {
            value = PlayerPrefs.GetInt(prefName);
            if (value == 1)
                return true;
        }
        return false;
    }

    void SetBoolInPlayerPrefInt(bool value)
    {
        PlayerPrefs.SetInt(string.Format("is{0}Complete", name), value == true ? 1 : 0);
    }

    public enum Type { Normal, Boss }
}
