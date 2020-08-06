using UnityEngine;

public class Side : MonoBehaviour
{
    private bool isWallSliding;

    public bool IsWallSliding
    {
        get { return isWallSliding; }
        set { isWallSliding = value; }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isWallSliding = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isWallSliding = false;
    }
}
