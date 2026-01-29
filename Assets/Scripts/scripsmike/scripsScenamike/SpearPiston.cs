using UnityEngine;
using System.Collections;

public class SpearPiston : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 localDirection = Vector2.right; // (1,0) o (0,1) etc
    public float distance = 2f;
    public float extendTime = 0.12f;
    public float retractTime = 0.12f;

    [Header("Timing")]
    public float delayBeforeExtend = 0.5f;
    public float delayExtended = 0.2f;
    public float delayRetracted = 0.6f;

    [Header("Damage")]
    public bool damageOnlyWhenExtended = true;

    private Vector3 startLocalPos;
    private Collider2D col;

    private void Awake()
    {
        startLocalPos = transform.localPosition;
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        StartCoroutine(Routine());
    }

    IEnumerator Routine()
    {
        while (true)
        {
            // retraída
            SetDamageActive(!damageOnlyWhenExtended);

            yield return new WaitForSeconds(delayBeforeExtend);

            // extender
            SetDamageActive(true);
            yield return MoveTo(startLocalPos + (Vector3)(localDirection.normalized * distance), extendTime);

            yield return new WaitForSeconds(delayExtended);

            // retraer
            SetDamageActive(!damageOnlyWhenExtended);
            yield return MoveTo(startLocalPos, retractTime);

            yield return new WaitForSeconds(delayRetracted);
        }
    }

    IEnumerator MoveTo(Vector3 targetLocal, float time)
    {
        Vector3 from = transform.localPosition;
        float t = 0f;

        if (time <= 0f)
        {
            transform.localPosition = targetLocal;
            yield break;
        }

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            transform.localPosition = Vector3.Lerp(from, targetLocal, t);
            yield return null;
        }
        transform.localPosition = targetLocal;
    }

    void SetDamageActive(bool active)
    {
        if (col != null) col.enabled = active;
    }
}
