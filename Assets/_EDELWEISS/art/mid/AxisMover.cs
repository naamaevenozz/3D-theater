using UnityEngine;

public class AxisMover : MonoBehaviour
{
    public bool moveX = true;
    public bool moveY = false;
    public bool moveZ = false;

    public float distanceX = 1f;
    public float distanceY = 1f;
    public float distanceZ = 1f;

    public float speedX = 1f;
    public float speedY = 1f;
    public float speedZ = 1f;

    public bool usePingPong = true;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float time = Time.time;
        float x = moveX ? GetMovement(time, speedX, distanceX) : 0f;
        float y = moveY ? GetMovement(time, speedY, distanceY) : 0f;
        float z = moveZ ? GetMovement(time, speedZ, distanceZ) : 0f;

        transform.localPosition = startPos + new Vector3(x, y, z);
    }

    float GetMovement(float time, float speed, float distance)
    {
        if (usePingPong)
            return Mathf.PingPong(time * speed, distance) - (distance / 2f);
        else
            return Mathf.Repeat(time * speed, distance) - (distance / 2f);
    }
}
