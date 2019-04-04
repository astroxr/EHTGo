using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class OnClick : MonoBehaviour, IPointerClickHandler
{
    public static event Action<PointerEventData> onClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
            onClick(eventData);
    }
}
