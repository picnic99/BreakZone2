using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteract : MonoBehaviour, 
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerUpHandler,
    IPointerMoveHandler,
    IDragHandler,IBeginDragHandler,IEndDragHandler
{

    public Action<PointerEventData> clickCall;
    public Action<PointerEventData> downCall;
    public Action<PointerEventData> upCall;
    public Action<PointerEventData> enterCall;
    public Action<PointerEventData> moveCall;
    public Action<PointerEventData> exitCall;
    public Action<PointerEventData> beginDragCall;
    public Action<PointerEventData> endDragCall;
    public Action<PointerEventData> DragingCall;

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragCall?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragingCall?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endDragCall?.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCall?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        downCall?.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        enterCall?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        exitCall?.Invoke(eventData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        moveCall?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        upCall?.Invoke(eventData);
    }
}
