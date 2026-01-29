using UnityEngine;
using System.Collections;

public class AxeSwing : MonoBehaviour
{
    [Header("Swing (Media Luna)")]
    public float maxAngle = 70f;        // amplitud del balanceo
    public float swingDuration = 0.8f;  // tiempo para ir de -maxAngle a +maxAngle

    [Header("Timing")]
    public float delay = 0.6f;          // ✅ PUBLICO: pausa entre balanceos
    public float startOffset = 0f;      // para que cada hacha empiece en diferente tiempo

    [Header("Optional")]
    public bool startGoingRight = true; // dirección inicial

    private void Start()
    {
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        if (startOffset > 0f)
            yield return new WaitForSeconds(startOffset);

        float from = startGoingRight ? -maxAngle : maxAngle;
        float to   = startGoingRight ?  maxAngle : -maxAngle;

        while (true)
        {
            yield return RotateOverTime(from, to, swingDuration);
            yield return new WaitForSeconds(delay);

            // regresa (para que sea ida y vuelta real)
            yield return RotateOverTime(to, from, swingDuration);
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator RotateOverTime(float from, float to, float time)
    {
        if (time <= 0f)
        {
            SetAngle(to);
            yield break;
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / time;
            float z = Mathf.Lerp(from, to, t);
            SetAngle(z);
            yield return null;
        }
        SetAngle(to);
    }

    private void SetAngle(float z)
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, z);
    }
}
