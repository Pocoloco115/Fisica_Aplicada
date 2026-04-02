using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DragCharge : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayers = ~0;

    private Camera cam;
    private Rigidbody2D rb;
    private Collider2D col;

    public bool isDragging;
    private Vector3 dragOffsetWorld;

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Mouse.current == null) return;

        var mouse = Mouse.current;

        if (mouse.leftButton.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame)
        {
            Vector2 mouseScreen = mouse.position.ReadValue();
            if (IsPointerOverThisObject(mouseScreen))
            {
                isDragging = true;
                rb.linearVelocity = Vector2.zero;

                Vector3 mouseWorld = ScreenToWorld(mouseScreen);
                dragOffsetWorld = transform.position - mouseWorld;
            }
        }

        if (mouse.leftButton.wasReleasedThisFrame || mouse.rightButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (isDragging && mouse.leftButton.isPressed)
        {
            Vector2 mouseScreen = mouse.position.ReadValue();
            Vector3 mouseWorld = ScreenToWorld(mouseScreen);
            Vector3 desiredPos = mouseWorld + dragOffsetWorld;
            desiredPos.z = transform.position.z;

            Vector2 currentPos = transform.position;
            Vector2 targetPos = desiredPos;

            Vector2 dir = targetPos - currentPos;
            float dist = dir.magnitude;

            if (dist > 0.0001f)
            {
                dir /= dist;
                RaycastHit2D hit = Physics2D.Raycast(currentPos, dir, dist, obstacleLayers);

                if (hit.collider != null && hit.collider != col)
                {
                    targetPos = hit.point - dir * 0.01f;
                }
            }

            transform.position = new Vector3(targetPos.x, targetPos.y, desiredPos.z);
        }
    }

    Vector3 ScreenToWorld(Vector2 mouseScreenPos)
    {
        Vector3 p = new Vector3(mouseScreenPos.x, mouseScreenPos.y, -cam.transform.position.z);
        Vector3 world = cam.ScreenToWorldPoint(p);
        world.z = 0f;
        return world;
    }

    bool IsPointerOverThisObject(Vector2 mouseScreenPos)
    {
        Vector3 worldPos = ScreenToWorld(mouseScreenPos);
        return col.OverlapPoint(worldPos);
    }
}
