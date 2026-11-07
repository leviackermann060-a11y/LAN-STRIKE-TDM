using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FpsPlayerController : MonoBehaviour
{
    public Camera viewCamera;
    public MobileJoystick joystick;
    public TouchLookArea lookArea;
    public HoldInput fireButton, sprintButton, jumpButton;
    public RifleHitscan rifle;
    public float walkSpeed = 4.8f, sprintSpeed = 7f, jumpHeight = 1.15f, gravity = -22f;
    public float mouseSensitivity = 2f, touchSensitivity = 0.11f, gyroSensitivity = 1.4f;
    CharacterController cc; float ySpeed, pitch; bool gyroOn;

    void Awake() {
        cc = GetComponent<CharacterController>();
        if (!viewCamera) viewCamera = GetComponentInChildren<Camera>();
        if (!rifle) rifle = GetComponentInChildren<RifleHitscan>();
        if (Application.isMobilePlatform && SystemInfo.supportsGyroscope) Input.gyro.enabled = true;
    }
    void Update() { Look(); Move(); Shoot(); }
    public void ToggleGyro() {
        gyroOn = !gyroOn && SystemInfo.supportsGyroscope;
        if (SystemInfo.supportsGyroscope) Input.gyro.enabled = gyroOn;
    }
    void Look() {
        float yaw=0, p=0;
        if (Application.isMobilePlatform) {
            if (lookArea) { Vector2 d=lookArea.Consume(); yaw+=d.x*touchSensitivity; p-=d.y*touchSensitivity; }
            if (gyroOn) { Vector3 r=Input.gyro.rotationRateUnbiased; yaw+=r.y*gyroSensitivity*Mathf.Rad2Deg*Time.deltaTime; p+=r.x*gyroSensitivity*Mathf.Rad2Deg*Time.deltaTime; }
        } else {
            yaw=Input.GetAxis("Mouse X")*mouseSensitivity; p=-Input.GetAxis("Mouse Y")*mouseSensitivity;
            if (Input.GetMouseButtonDown(0)) { Cursor.lockState=CursorLockMode.Locked; Cursor.visible=false; }
            if (Input.GetKeyDown(KeyCode.Escape)) { Cursor.lockState=CursorLockMode.None; Cursor.visible=true; }
        }
        transform.Rotate(0,yaw,0); pitch=Mathf.Clamp(pitch+p,-82,82);
        if (viewCamera) viewCamera.transform.localRotation=Quaternion.Euler(pitch,0,0);
    }
    void Move() {
        Vector2 input; bool sprint,jump;
        if (Application.isMobilePlatform) {
            input=joystick?joystick.Value:Vector2.zero; sprint=sprintButton&&sprintButton.Held; jump=jumpButton&&jumpButton.Pressed;
        } else {
            input=new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            sprint=Input.GetKey(KeyCode.LeftShift); jump=Input.GetButtonDown("Jump");
        }
        Vector3 planar=(transform.right*input.x+transform.forward*input.y).normalized;
        cc.Move(planar*(sprint?sprintSpeed:walkSpeed)*Time.deltaTime);
        if(cc.isGrounded&&ySpeed<0)ySpeed=-2;
        if(jump&&cc.isGrounded)ySpeed=Mathf.Sqrt(jumpHeight*-2*gravity);
        ySpeed+=gravity*Time.deltaTime; cc.Move(Vector3.up*ySpeed*Time.deltaTime);
    }
    void Shoot() {
        bool fire=Application.isMobilePlatform?(fireButton&&fireButton.Held):Input.GetMouseButton(0);
        if(fire&&rifle&&viewCamera)rifle.Fire(viewCamera);
    }
}
