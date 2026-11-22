using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarBasic : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float maxReverseSpeed = 3f;
    [SerializeField] private float acceleration = 6f;
    [SerializeField] private float friction = 4f;
    [SerializeField] private float turnSpeed = 140f;

    private float currentSpeed = 0f;

    void Update()
    {
        var keys = Keyboard.current;
        if (keys == null) return;

        float drive = 0f;
        float turn = 0f;

        if (keys.wKey.isPressed || keys.upArrowKey.isPressed)
        {
            drive = drive + 1f;
        }
        if (keys.sKey.isPressed || keys.downArrowKey.isPressed)
        {
            drive = drive - 1f;
        }
        if (keys.aKey.isPressed || keys.leftArrowKey.isPressed)
        {
            turn = turn + 1f;
        }
        if (keys.dKey.isPressed || keys.rightArrowKey.isPressed)
        {
            turn = turn - 1f;
        }

        
        if (drive != 0f)
        {
            currentSpeed = currentSpeed + drive * acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, friction * Time.deltaTime);
        }
        currentSpeed = Mathf.Clamp(currentSpeed, -maxReverseSpeed, maxSpeed);

        
        float speedFactor = currentSpeed / maxSpeed;
        transform.Rotate(0, 0, turn * turnSpeed * speedFactor * Time.deltaTime);


        transform.Translate(Vector3.up * (currentSpeed * Time.deltaTime));
    }
}