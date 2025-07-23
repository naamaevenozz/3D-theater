using UnityEngine;

public class EllipseMover : MonoBehaviour
{
    public float radiusX = 2f;     // רוחב האליפסה (ציר אופקי)
    public float radiusZ = 1f;     // עומק האליפסה (ציר אחורי)
    public float speed = 1f;       // מהירות התנועה
    public bool rotateToDirection = true; // האם להסתובב לכיוון התנועה

    private Vector3 centerPos;
    private float angle = 0f;

    void Start()
    {
        centerPos = transform.localPosition;
    }

    void Update()
    {
        angle += speed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radiusX;
        float z = Mathf.Sin(angle) * radiusZ;

        Vector3 nextPos = centerPos + new Vector3(x, 0f, z);
        
        if (rotateToDirection)
        {
            Vector3 dir = nextPos - transform.position;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);
        }

        transform.localPosition = nextPos;
    }
}
