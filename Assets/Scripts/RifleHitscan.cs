using UnityEngine;
public class RifleHitscan : MonoBehaviour {
    public float roundsPerSecond=8.5f, damage=25, range=160;
    float nextShot;
    public void Fire(Camera cam) {
        if(Time.time<nextShot)return; nextShot=Time.time+1f/roundsPerSecond;
        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out RaycastHit hit,range)) {
            TargetHealth target=hit.collider.GetComponentInParent<TargetHealth>();
            if(target)target.Damage(damage);
        }
    }
}
