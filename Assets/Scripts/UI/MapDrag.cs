using UnityEngine;
using UnityEngine.EventSystems;

public class MapDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform mapContent;

    internal bool WasDragging { get; private set; }

    private Canvas _canvas;
    private RectTransform _parentRect;

    private void Start()
    {
        _canvas = GetComponentInParent<Canvas>().rootCanvas;
        _parentRect = mapContent.parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        WasDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset next frame so click handlers this frame still see it as true.
        Invoke(nameof(ResetDrag), 0f);
    }

    private void ResetDrag()
    {
        WasDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        mapContent.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        ClampPosition();
    }

    private void ClampPosition()
    {
        float scale = mapContent.localScale.x;
        Vector2 scaledSize = mapContent.rect.size * scale;
        Vector2 parentSize = _parentRect.rect.size;

        float maxX = Mathf.Max(0f, (scaledSize.x - parentSize.x) * 0.5f);
        float maxY = Mathf.Max(0f, (scaledSize.y - parentSize.y) * 0.5f);

        Vector2 pos = mapContent.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
        pos.y = Mathf.Clamp(pos.y, -maxY, maxY);
        mapContent.anchoredPosition = pos;
    }
}
