using UnityEngine;
using System.Collections.Generic;

public class FieldParticlesManager : MonoBehaviour
{
    public Camera cam;
    public GameObject fieldParticlePrefab;

    public float spacing = 1f;
    public float particleSpeed = 1f;
    public float influenceRadius = 5f;

    List<FieldParticle> particles = new List<FieldParticle>();

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Start()
    {
        SpawnGridInCamera();
    }

    void SpawnGridInCamera()
    {
        float orthoSize = cam.orthographicSize;
        float aspect = cam.aspect;

        float halfHeight = orthoSize;
        float halfWidth = orthoSize * aspect;

        float minX = cam.transform.position.x - halfWidth;
        float maxX = cam.transform.position.x + halfWidth;
        float minY = cam.transform.position.y - halfHeight;
        float maxY = cam.transform.position.y + halfHeight;

        for (float x = minX; x <= maxX; x += spacing)
        {
            for (float y = minY; y <= maxY; y += spacing)
            {
                Vector3 pos = new Vector3(x, y, 0f);
                GameObject go = Instantiate(fieldParticlePrefab, pos, Quaternion.identity);
                FieldParticle fp = go.GetComponent<FieldParticle>();
                particles.Add(fp);
            }
        }
    }

    void Update()
    {
        UpdateParticles();
    }

    void UpdateParticles()
    {
        if (particles.Count == 0) return;

        foreach (var p in particles)
        {
            if (p == null) continue;

            Vector2 pos = p.transform.position;
            Vector2 forceDir = Vector2.zero;

            foreach (var charge in CoulombFieldManager.GetAllCharges())
            {
                if (charge == null) continue;

                Vector2 cPos = charge.transform.position;
                Vector2 dir = pos - cPos;
                float dist = dir.magnitude;
                if (dist < 0.0001f) continue;
                if (dist > influenceRadius) continue;

                dir /= dist;

                float strength = 1f / (dist * dist);
                forceDir += dir * strength * Mathf.Sign(charge.chargeValue);
            }

            p.velocity = forceDir.normalized * particleSpeed;
            p.transform.position += (Vector3)(p.velocity * Time.deltaTime);
        }
    }
}
