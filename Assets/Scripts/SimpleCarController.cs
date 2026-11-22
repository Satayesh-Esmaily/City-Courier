using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// Simple top-down car movement.
// Keys: W / Up = forward, S / Down = reverse, A / Left = turn left, D / Right = turn right.
// Add this script to any car object (sprites face UP).
public class SimpleCarController : MonoBehaviour
{
    [Header("Speed (units per second)")]
    public float maxSpeed = 8f;
    public float maxReverseSpeed = 3f;
    public float acceleration = 6f;
    public float friction = 4f;      // slows the car when no key is pressed

    [Header("Steering")]
    public float turnSpeed = 140f;   // degrees per second at full speed

    float currentSpeed;

    void Update()
    {
        float v, h;
        ReadInput(out v, out h);

        // forward / backward
        if (v != 0f)
            currentSpeed += v * acceleration * Time.deltaTime;
        else
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, friction * Time.deltaTime);
        currentSpeed = Mathf.Clamp(currentSpeed, -maxReverseSpeed, maxSpeed);

        // steering only works while moving (and flips in reverse)
        float speedFactor = Mathf.Clamp(currentSpeed / maxSpeed, -1f, 1f);
        transform.Rotate(0f, 0f, -h * turnSpeed * speedFactor * Time.deltaTime);

        // move along the car's front (up)
        transform.position += transform.up * currentSpeed * Time.deltaTime;
    }

    static void ReadInput(out float v, out float h)
    {
#if ENABLE_INPUT_SYSTEM
        var k = Keyboard.current;
        if (k == null) { v = 0f; h = 0f; return; }
        v = (k.wKey.isPressed || k.upArrowKey.isPressed ? 1f : 0f) - (k.sKey.isPressed || k.downArrowKey.isPressed ? 1f : 0f);
        h = (k.dKey.isPressed || k.rightArrowKey.isPressed ? 1f : 0f) - (k.aKey.isPressed || k.leftArrowKey.isPressed ? 1f : 0f);
#else
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");
#endif
    }
}
