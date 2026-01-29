using UnityEngine;
using System.Collections;

public class PopUpSpikes : MonoBehaviour
{
    [SerializeField] private Transform spikesVisual;
    [SerializeField] private Vector3 hiddenLocalPos;
    [SerializeField] private Vector3 shownLocalPos;
    [SerializeField] private float showTime = 0.6f;
    [SerializeField] private float hideTime = 0.8f;

    private void Start() => StartCoroutine(Routine());

    IEnumerator Routine()
    {
        while (true)
        {
            spikesVisual.localPosition = shownLocalPos;
            yield return new WaitForSeconds(showTime);
            spikesVisual.localPosition = hiddenLocalPos;
            yield return new WaitForSeconds(hideTime);
        }
    }
}
