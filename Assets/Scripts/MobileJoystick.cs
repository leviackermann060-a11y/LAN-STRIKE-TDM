using UnityEngine;
using UnityEngine.EventSystems;
public class MobileJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    public RectTransform handle; public float radius=75; public Vector2 Value {get;private set;}
    public void OnPointerDown(PointerEventData e)=>OnDrag(e);
    public void OnDrag(PointerEventData e){RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform,e.position,e.pressEventCamera,out Vector2 p);Value=Vector2.ClampMagnitude(p/radius,1);if(handle)handle.anchoredPosition=Value*radius;}
    public void OnPointerUp(PointerEventData e){Value=Vector2.zero;if(handle)handle.anchoredPosition=Vector2.zero;}
}
