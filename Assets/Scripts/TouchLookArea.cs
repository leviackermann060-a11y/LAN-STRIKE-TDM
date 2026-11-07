using UnityEngine;
using UnityEngine.EventSystems;
public class TouchLookArea : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    Vector2 last,delta; bool active;
    public void OnPointerDown(PointerEventData e){last=e.position;delta=Vector2.zero;active=true;}
    public void OnDrag(PointerEventData e){if(!active)return;delta+=e.position-last;last=e.position;}
    public void OnPointerUp(PointerEventData e){active=false;}
    public Vector2 Consume(){Vector2 d=delta;delta=Vector2.zero;return d;}
}
