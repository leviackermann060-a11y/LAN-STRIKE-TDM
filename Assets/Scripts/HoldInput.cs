using UnityEngine;
using UnityEngine.EventSystems;
public class HoldInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
    public bool Held {get;private set;} public bool Pressed {get;private set;}
    public void OnPointerDown(PointerEventData e){Held=true;Pressed=true;}
    void LateUpdate(){Pressed=false;}
    public void OnPointerUp(PointerEventData e){Held=false;}
    public void OnPointerExit(PointerEventData e){Held=false;}
}
