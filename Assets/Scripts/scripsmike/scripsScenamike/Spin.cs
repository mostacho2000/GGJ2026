using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = 360f;
    void Update() => transform.Rotate(0, 0, degreesPerSecond * Time.deltaTime);
}
