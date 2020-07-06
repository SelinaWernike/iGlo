using UnityEngine;

public class SprinningUI : MonoBehaviour
{
    [SerializeField]
    private float timeStep;
    [SerializeField]
    private float angleChange;
    private float startTime;
    private new RectTransform transform;

    private void Start()
    {
        startTime = Time.time;
        transform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Time.time - startTime > timeStep)
        {
            Vector3 currentAngles = transform.localEulerAngles;
            currentAngles.z -= angleChange % 360;
            transform.localEulerAngles = currentAngles;
            startTime = Time.time;
        }
    }
}
