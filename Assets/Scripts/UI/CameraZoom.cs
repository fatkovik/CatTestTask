using UnityEngine;
using UnityEngine.EventSystems;

public class CameraZoom : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler
{
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 3f;
    [SerializeField] private float zoomMultiplier = 2f;
    [SerializeField] private float smoothTime = 0.25f;

    private float _targetScale;
    private float _velocity;

    private RectTransform _rectTransform;
    private RectTransform _parentRectTransform;
    private Canvas _canvas;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _parentRectTransform = transform.parent.GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>().rootCanvas;

        _targetScale = _rectTransform.localScale.x;
    }

    private void Update()
    {
        float currentScale = _rectTransform.localScale.x;
        float smoothed = Mathf.SmoothDamp(currentScale, _targetScale, ref _velocity, smoothTime);
        _rectTransform.localScale = Vector3.one * smoothed;
        ClampPosition();
    }

    // ── Scroll wheel ─────────────────────────────────────────────────────────

    public void OnScroll(PointerEventData eventData)
    {
        _targetScale += eventData.scrollDelta.y * zoomMultiplier * 0.1f;
        _targetScale = Mathf.Clamp(_targetScale, minScale, maxScale);
    }

    // ── Drag to pan ──────────────────────────────────────────────────────────

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        ClampPosition();
    }

    // ── Bounds ───────────────────────────────────────────────────────────────

    // Assumes mapContent pivot = (0.5, 0.5)
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
