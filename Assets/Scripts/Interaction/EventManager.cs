using System.Collections;
using UnityEngine;


/// <summary>
/// This is an event manager that allows to compatively share static (class) methods
/// as delegate events. You can share Touch actions, Pinch actions so that they're executed 
/// </summary>
public class EventManager : MonoBehaviour {
    public delegate void PinchAction(Touch touchZero, Touch touchOne);
    public static event PinchAction OnPinched;

    public delegate void SelectAction(Touch touch);
    public static event SelectAction OnSelected;

    private void Update() {

        if (OnPinched != null && Input.touchCount == 2){
            
            Debug.Log("Detected two touches!");

            var touchOne = Input.GetTouch(1);
            var touchZero = Input.GetTouch(0);

            var validOne = touchOne.phase == TouchPhase.Moved;
            var validZero = touchZero.phase == TouchPhase.Moved;

            if (validOne && validZero){
                Debug.Log("Detected two valid touches!");
                OnPinched(touchZero, touchOne);
            }

        }

        if (OnSelected != null && Input.touchCount == 1){
            Debug.Log("Detected one touch!");
            
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began){
                Debug.Log("Detected one valid touch!");
                OnSelected(touch);
            }
        }
    }
}