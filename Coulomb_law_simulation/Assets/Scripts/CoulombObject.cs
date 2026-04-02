using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CoulombObject : MonoBehaviour
{
    public float chargeValue = 1f;
    public char chargeSign = '+';
    public bool isMainCharge = false;
    public float forceScale = 1e-9f;
    public ChargeLabelFollower labelFollower;
    public Sprite positiveCharge;
    public Sprite negativeCharge;
    public GameObject explosionPrefab;  
    private SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = chargeSign == '+' ? positiveCharge : negativeCharge;
        }
    }
    public void ApplyCoulombForce(Vector2 forceWorld)
    {
        rb.AddForce(forceWorld * forceScale);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMainCharge)
            return;
        if (collision.gameObject.CompareTag("charge")) return;
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        CoulombFieldManager.RemoveCharge(this);
        if(labelFollower != null)
        {
            Destroy(labelFollower.gameObject);
        }
        Destroy(gameObject);
    }
}
