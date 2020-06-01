using UnityEngine;
using UnityEngine.Events;

public class MoveMenuScript : MonoBehaviour
{
    [SerializeField]
    private float changePerSecond;
    [SerializeField]
    private Vector3 size;
    [SerializeField]
    private UnityEvent onDone;
    private Vector3 endPoint;
    private Vector3 startPoint;
    private Vector3 targetPoint;
    private new RectTransform transform;
    private bool running;

    private void Start()
    {
        transform = GetComponent<RectTransform>();
        startPoint = transform.localPosition;
        endPoint = transform.localPosition + size;
        targetPoint = endPoint;
    }

    private void Update()
    {
        if (running)
        {
            float distance = Vector3.Distance(transform.localPosition, targetPoint);
            float movement = changePerSecond * Time.deltaTime;
            float increment = movement / distance;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPoint, increment);
            if (transform.localPosition == targetPoint)
            {
                onDone.Invoke();
                targetPoint = targetPoint == endPoint ? startPoint : endPoint;
                running = false;
            }
        }
    }

    public void OnActivate()
    {
        running = true;
    }
}
