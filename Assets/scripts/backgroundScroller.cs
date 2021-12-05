using UnityEngine;

public class backgroundScroller : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private float width;
    public float speed = -2f;
    private Vector2 startingPoint;
    GameManager game;
    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        width = bc.size.x;
        bc.enabled = false;
        rb.velocity = new Vector2(0, 0);
        game = GameManager.Instance;

    }
    // Update is called once per frame
    void Update()
    {
        if (!game.GameOver)
        {
            rb.velocity = new Vector2(speed, 0);
        }
            else
        {
            rb.velocity = new Vector2(0, 0);
        }
        if(transform.position.x < -width)
        {
            transform.position = new Vector2(width * Mathf.Abs(transform.localScale.x), 0);// startingPoint;// (Vector2)transform.position + new Vector2(width * 2, 0);
        }
    }
}
