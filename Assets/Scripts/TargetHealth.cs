using UnityEngine;
public class TargetHealth : MonoBehaviour {
    public float maxHealth=100; float health; Vector3 startPos; Quaternion startRot;
    void Awake(){health=maxHealth;startPos=transform.position;startRot=transform.rotation;}
    public void Damage(float amount){health=Mathf.Max(0,health-amount);if(health<=0)Invoke(nameof(Respawn),2.5f);}
    void Respawn(){health=maxHealth;transform.SetPositionAndRotation(startPos,startRot);}
}
