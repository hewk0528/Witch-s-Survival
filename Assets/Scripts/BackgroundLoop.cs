using UnityEngine;

public class BackgroundLoop : MonoBehaviour {
    private float speed = 7.5f;
    private float width;

    void Awake() {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();
        width = backgroundCollider.size.x * 2.0f;
    }

    void Update() {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (transform.position.x <= -width)
        {
            Reposition();
        }
    }

    void Reposition() {
        Vector2 offset = new Vector2(width * 2.0f, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}
