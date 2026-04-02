using UnityEngine;

public class ChargesManager : MonoBehaviour
{
    public Camera cam;
    public CoulombObject mainCharge;
    public CoulombObject chargePrefab;

    [Header("Márgenes respecto a los bordes")]
    public float borderMargin = 0.5f;

    [Header("Distancia mínima al main")]
    public float minDistanceToMain = 1.5f;

    [Header("Distancia mínima a cualquier carga")]
    public float minDistanceToAnyCharge = 1.0f;

    public GameObject chargeLabelPrefab;
    public Canvas worldCanvas;

    public int maxTries = 20;

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Start()
    {
        CreateLabelForMain();
    }

    void CreateLabelForMain()
    {
        if (chargeLabelPrefab == null || mainCharge == null || worldCanvas == null)
            return;

        GameObject labelObj = Instantiate(
            chargeLabelPrefab,
            worldCanvas.transform
        );

        var rt = labelObj.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.localScale = Vector3.one;
            rt.position = mainCharge.transform.position;
        }

        ChargeLabelFollower follower = labelObj.GetComponent<ChargeLabelFollower>();
        follower.target = mainCharge.transform;
        mainCharge.labelFollower = follower;
    }

    public void InstantiateChargeInScene(bool isPositive)
    {
        if (cam == null || mainCharge == null || chargePrefab == null)
            return;

        for (int i = 0; i < maxTries; i++)
        {
            Vector3 randomPos = GetRandomPositionInsideCamera();

            if (!IsValidSpawnPosition(randomPos))
                continue;

            CoulombObject newCharge = Instantiate(chargePrefab, randomPos, Quaternion.identity);
            CoulombFieldManager.AddCharge(newCharge);

            float magnitude = Random.Range(1f, 3f);
            newCharge.chargeValue = magnitude;
            newCharge.chargeSign = isPositive ? '+' : '-';

            if (chargeLabelPrefab != null && worldCanvas != null)
            {
                GameObject labelInstance = Instantiate(
                    chargeLabelPrefab,
                    worldCanvas.transform
                );

                var rt = labelInstance.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.localScale = Vector3.one;
                    rt.position = randomPos;
                }

                ChargeLabelFollower follower = labelInstance.GetComponent<ChargeLabelFollower>();
                if (follower != null)
                {
                    follower.target = newCharge.transform;
                    newCharge.labelFollower = follower;
                }
            }

            return;
        }
    }

    bool IsValidSpawnPosition(Vector3 pos)
    {
        var charges = CoulombFieldManager.GetAllCharges();
        foreach (var c in charges)
        {
            if (c == null) continue;
            float d = Vector2.Distance(pos, c.transform.position);
            if (d < minDistanceToAnyCharge)
                return false;
        }

        if (mainCharge != null)
        {
            float dMain = Vector2.Distance(pos, mainCharge.transform.position);
            if (dMain < minDistanceToMain)
                return false;
        }

        return true;
    }

    Vector3 GetRandomPositionInsideCamera()
    {
        float orthoSize = cam.orthographicSize;
        float aspect = cam.aspect;

        float halfHeight = orthoSize;
        float halfWidth = orthoSize * aspect;

        float minX = cam.transform.position.x - halfWidth + borderMargin;
        float maxX = cam.transform.position.x + halfWidth - borderMargin;
        float minY = cam.transform.position.y - halfHeight + borderMargin;
        float maxY = cam.transform.position.y + halfHeight - borderMargin;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        return new Vector3(x, y, mainCharge.transform.position.z);
    }
}