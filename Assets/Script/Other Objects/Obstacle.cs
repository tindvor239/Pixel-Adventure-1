using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float period = 2f;
    [SerializeField] Vector2 target;
    Vector2 startPosition;
    const float tau = Mathf.PI * 2;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float cycles = Time.time / period;
        float sine = Mathf.Sin(cycles * tau);
        Vector2 offset = target * sine;
        transform.position = startPosition + offset;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Terrain")
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Terrain")
        {
            collision.transform.parent = null;
        }
    }
}
