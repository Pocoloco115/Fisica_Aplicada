using UnityEngine;
using System.Collections;

public class ExplosionAnim : MonoBehaviour
{
    public float animationDuration = 2f;

    void Start()
    {
        StartCoroutine(DestroyGameObject());
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(animationDuration);
        Destroy(gameObject);
    }
}
