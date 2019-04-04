using System.Collections;
using UnityEngine;
using System;


/// <summary>
/// This is an event manager that allows to compatively share static (class) methods
/// as delegate events. You can share Touch actions, Pinch actions so that they're executed 
/// </summary>
public class EventManager : MonoBehaviour {

    // The line below is a shorthand for:
    // public delegate void PinchAction(Touch touchZero, Touch touchOne);
    // public static event PinchAction OnPinched;
    public static event Action<Touch, Touch> OnPinched;
    public static event Action<Touch> OnSelected;
    public static event Action<Vector2> OnSelectMouse;

    private void OnMouseDown() {
        Debug.Log(Input.mousePosition);
        
    }

    private void Update() {

        if (OnPinched != null && Input.touchCount == 2){
            // Detect Dragging
            var touchOne = Input.GetTouch(1);
            var touchZero = Input.GetTouch(0);

            var validOne = touchOne.phase == TouchPhase.Moved;
            var validZero = touchZero.phase == TouchPhase.Moved;

            if (validOne && validZero){
                OnPinched(touchZero, touchOne);
            }

        }
        else if (OnSelected != null && Input.touchCount == 1){
            // Detect OnClick
            
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began){
                OnSelected(touch);
            }
        }
        if (OnSelectMouse != null && Input.GetMouseButtonDown(1)){
            Debug.Log("Pressed primary button.");
            OnSelectMouse(Input.mousePosition);
        }
    }
}