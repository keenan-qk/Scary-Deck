using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementOnlyTest : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        var kb = Keyboard.current;

        float x = (kb.dKey.isPressed ? 1f : 0f)
                - (kb.aKey.isPressed ? 1f : 0f);

        float y = (kb.wKey.isPressed ? 1f : 0f)
                - (kb.sKey.isPressed ? 1f : 0f);

        movement = new Vector2(x, y).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }

    void OnDisable()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }
}

