using UnityEngine;
using TMPro;

public class ChargeLabelFollower : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 0.5f, 0f);

    public TextMeshProUGUI vectorText;
    public TextMeshProUGUI magnitudeText;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

        transform.position = target.position + offset;

        CoulombObject co = target.GetComponent<CoulombObject>();
        if (co != null)
        {
            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            float mag = co.chargeValue;

            vectorText.text = $"({target.position.x:F2}, {target.position.y:F2})";
            magnitudeText.text = mag.ToString("F2");
        }

        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                         cam.transform.rotation * Vector3.up);
    }
}
