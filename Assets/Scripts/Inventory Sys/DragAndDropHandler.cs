using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DragAndDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector2 _startPosition;
    private Transform _startParent;
    private GameObject _draggedIcon;
    private InventorySlot _slot;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _slot = GetComponent<InventorySlot>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_slot._data == null) return;

        _startPosition = _rectTransform.anchoredPosition;
        _startParent = transform.parent;

        _draggedIcon = InventoryManager.Instance._dragGameObject;
        _draggedIcon.gameObject.SetActive(true);

        Image draggedImage = _draggedIcon.GetComponent<Image>();
        draggedImage.sprite = _slot._data._itemData._invInfo._inventorySprite;
        draggedImage.raycastTarget = false;

        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_slot._data == null || _draggedIcon == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            InventoryManager.Instance._inventoryCanvas.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        _draggedIcon.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_slot._data == null || _draggedIcon == null) return;

        _draggedIcon.gameObject.SetActive(false);
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;

        GameObject dropTarget = eventData.pointerCurrentRaycast.gameObject;
        if (dropTarget == null) return;

        if (dropTarget.GetComponent<DragAndDropHandler>() == null) return;

        InventorySlot targetSlot = dropTarget.GetComponent<InventorySlot>();
        if (targetSlot != null)
        {
            if (targetSlot._data == null)
            {
                if (targetSlot._ChangeData(_slot._data))
                    _slot._ChangeData(null);
            }
            else
            {
                _InvData tempData = targetSlot._data;
                if (targetSlot._ChangeData(_slot._data))
                    _slot._ChangeData(tempData);
            }
        }
    }

}