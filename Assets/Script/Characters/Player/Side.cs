using UnityEngine;

public class Side : MonoBehaviour
{
    [SerializeField] float distance;
    bool isWallSliding;

    public bool IsWallSliding
    {
        get { return isWallSliding; }
        set { isWallSliding = value; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Terrain")
            isWallSliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Terrain")
            isWallSliding = false;
    }
}
