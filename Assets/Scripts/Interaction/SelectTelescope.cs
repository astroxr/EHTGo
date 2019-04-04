using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class SelectTelescope : MonoBehaviour, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    public int ID;
    public Material m_NotSelected;
    public Material m_Selected;
    private Renderer m_Renderer;

    // public static ArrayList<SelectTelescope> allMySelectables;

    private void Start() {
        m_Renderer = GetComponent<Renderer>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        Debug.Log("Clicking Telescope. ID=" + ID);
        EventSystem.current.SetSelectedGameObject(transform.gameObject, eventData);
    }

    public void OnSelect(BaseEventData eventData)
    {
        m_Renderer.material = m_Selected;
        Debug.Log("Selecting Telescope. ID=" + ID);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        m_Renderer.material = m_NotSelected;
        Debug.Log("Deselecting Telescope. ID=" + ID);
    }
}