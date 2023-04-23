using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public int bounceLimit;
    private int bounceCount;

    // Start is called before the first frame update
    void Start()
    {
        bounceCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = rigidBody.velocity;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            if (bounceCount >= bounceLimit) {
                Destroy(gameObject);
            }

            bounceCount += 1;
        }
    }
}
