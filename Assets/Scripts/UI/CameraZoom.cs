using UnityEngine;
using UnityEngine.EventSystems;

public class CameraZoom : MonoBehaviour, IScrollHandler
{
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 3f;
    [SerializeField] private float zoomMultiplier = 2f;
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private float pinchSensitivity = 0.005f;

    private float _targetScale;
    private float _velocity;
    private float _prevPinchDistance;

    private RectTransform _rectTransform;
    private RectTransform _parentRectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _parentRectTransform = transform.parent.GetComponent<RectTransform>();

        _targetScale = _rectTransform.localScale.x;
    }

    private void Update()
    {
        HandlePinchZoom();

        float currentScale = _rectTransform.localScale.x;
        float smoothed = Mathf.SmoothDamp(currentScale, _targetScale, ref _velocity, smoothTime);
        _rectTransform.localScale = Vector3.one * smoothed;
        ClampPosition();
    }

    private void HandlePinchZoom()
    {
        if (Input.touchCount != 2)
        {
            _prevPinchDistance = 0f;
            return;
        }

        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        float currentDistance = Vector2.Distance(touch0.position, touch1.position);

        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
        {
            _prevPinchDistance = currentDistance;
            return;
        }

        float delta = currentDistance - _prevPinchDistance;
        _targetScale += delta * pinchSensitivity * zoomMultiplier;
        _targetScale = Mathf.Clamp(_targetScale, minScale, maxScale);
        _prevPinchDistance = currentDistance;
    }

    public void OnScroll(PointerEventData eventData)
    {
        _targetScale += eventData.scrollDelta.y * zoomMultiplier * 0.1f;
        _targetScale = Mathf.Clamp(_targetScale, minScale, maxScale);
    }

    private void ClampPosition()
    {
        float scale = _rectTransform.localScale.x;
        Vector2 scaledSize = _rectTransform.rect.size * scale;
        Vector2 parentSize = _parentRectTransform.rect.size;

        float maxX = Mathf.Max(0f, (scaledSize.x - parentSize.x) * 0.5f);
        float maxY = Mathf.Max(0f, (scaledSize.y - parentSize.y) * 0.5f);

        Vector2 pos = _rectTransform.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
        pos.y = Mathf.Clamp(pos.y, -maxY, maxY);
        _rectTransform.anchoredPosition = pos;
    }
}
