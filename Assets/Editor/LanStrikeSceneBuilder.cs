#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class LanStrikeSceneBuilder {
    static Material ground,wall,metal,sand,red,blue;
    [MenuItem("Tools/LAN STRIKE/Build Training Ground Scene")]
    public static void Build() {
        if(!AssetDatabase.IsValidFolder("Assets/Scenes"))AssetDatabase.CreateFolder("Assets","Scenes");
        ground=Mat("Dusty concrete",new Color(.34f,.34f,.31f)); wall=Mat("Weathered cement",new Color(.48f,.47f,.42f));
        metal=Mat("Military metal",new Color(.12f,.16f,.15f)); sand=Mat("Olive sandbag",new Color(.34f,.37f,.25f));
        red=Mat("Red team",new Color(.65f,.12f,.1f)); blue=Mat("Blue team",new Color(.1f,.28f,.68f));
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        RenderSettings.ambientLight=new Color(.48f,.5f,.53f); RenderSettings.fog=true; RenderSettings.fogColor=new Color(.68f,.7f,.68f); RenderSettings.fogDensity=.004f;
        var sun=new GameObject("Sun - afternoon"); var l=sun.AddComponent<Light>();l.type=LightType.Directional;l.intensity=1.25f;sun.transform.rotation=Quaternion.Euler(48,-32,0);
        Cube("Ground",new Vector3(0,-.5f,0),new Vector3(100,1,100),ground);
        Cube("North wall",new Vector3(0,2,48),new Vector3(100,4,1),wall);Cube("South wall",new Vector3(0,2,-48),new Vector3(100,4,1),wall);
        Cube("East wall",new Vector3(48,2,0),new Vector3(1,4,100),wall);Cube("West wall",new Vector3(-48,2,0),new Vector3(1,4,100),wall);
        Building("Barracks west",new Vector3(-25,3,18),new Vector3(18,6,13));Building("Barracks east",new Vector3(25,3,-18),new Vector3(18,6,13));
        Building("Central warehouse",new Vector3(0,3,0),new Vector3(14,6,18));
        for(int i=0;i<8;i++){float x=-30+i*8.5f;Cube("Sandbag cover "+i,new Vector3(x,.65f,9),new Vector3(4.5f,1.3f,1.4f),sand);Cube("Metal crate "+i,new Vector3(x,.9f,-10),new Vector3(2,1.8f,2),metal);}
        Cube("Red container",new Vector3(-17,1.4f,-25),new Vector3(13,2.8f,3),red);Cube("Blue container",new Vector3(18,1.4f,25),new Vector3(13,2.8f,3),blue);
        Cube("Red spawn pad",new Vector3(-38,.03f,0),new Vector3(7,.06f,7),red);Cube("Blue spawn pad",new Vector3(38,.03f,0),new Vector3(7,.06f,7),blue);
        MakePlayer();MakeHud();
        EditorSceneManager.SaveScene(scene,"Assets/Scenes/LanStrikeTrainingGround.unity");AssetDatabase.SaveAssets();
        Debug.Log("Scene created at Assets/Scenes/LanStrikeTrainingGround.unity");
    }
    static Material Mat(string n,Color c){var s=Shader.Find("Universal Render Pipeline/Lit");if(s==null)s=Shader.Find("Standard");return new Material(s){name=n,color=c};}
    static GameObject Cube(string n,Vector3 p,Vector3 s,Material m){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.position=p;g.transform.localScale=s;g.GetComponent<Renderer>().sharedMaterial=m;return g;}
    static void Building(string n,Vector3 p,Vector3 s){Cube(n+" shell",p,s,wall);Cube(n+" roof",p+Vector3.up*(s.y*.52f),new Vector3(s.x+.5f,.4f,s.z+.5f),metal);Cube(n+" doorway",p+new Vector3(0,-1.2f,-s.z*.51f),new Vector3(3,3.6f,.25f),metal);}
    static void MakePlayer(){
        var p=new GameObject("Player - local FPS");p.transform.position=new Vector3(-38,1.2f,0);
        var cc=p.AddComponent<CharacterController>();cc.height=1.8f;cc.radius=.35f;
        var c=new GameObject("Camera");c.transform.SetParent(p.transform);c.transform.localPosition=new Vector3(0,.55f,0);
        var cam=c.AddComponent<Camera>();cam.fieldOfView=82;c.AddComponent<AudioListener>();
        var gun=GameObject.CreatePrimitive(PrimitiveType.Cube);gun.name="Placeholder rifle";gun.transform.SetParent(c.transform);gun.transform.localPosition=new Vector3(.28f,-.24f,.55f);gun.transform.localScale=new Vector3(.12f,.13f,.65f);gun.GetComponent<Renderer>().sharedMaterial=metal;Object.DestroyImmediate(gun.GetComponent<Collider>());
        var rifle=gun.AddComponent<RifleHitscan>();var controller=p.AddComponent<FpsPlayerController>();controller.viewCamera=cam;controller.rifle=rifle;p.AddComponent<TargetHealth>();
    }
    static void MakeHud(){
        var go=new GameObject("Mobile HUD");var canvas=go.AddComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;var scaler=go.AddComponent<CanvasScaler>();scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1920,1080);go.AddComponent<GraphicRaycaster>();
        var es=new GameObject("EventSystem");es.AddComponent<EventSystem>();es.AddComponent<StandaloneInputModule>();
        Panel(go.transform,"Joystick area",new Vector2(.04f,.08f),new Vector2(.27f,.48f),new Color(1,1,1,.12f),true);
        Panel(go.transform,"Look area",new Vector2(.35f,.08f),new Vector2(.98f,.9f),new Color(1,1,1,.015f),false);
        HudButton(go.transform,"FIRE",new Vector2(.88f,.25f),new Vector2(150,150));HudButton(go.transform,"JUMP",new Vector2(.76f,.15f),new Vector2(110,90));HudButton(go.transform,"SPRINT",new Vector2(.18f,.08f),new Vector2(130,70));
    }
    static void Panel(Transform par,string n,Vector2 a,Vector2 b,Color color,bool joy){var g=new GameObject(n);g.transform.SetParent(par,false);var im=g.AddComponent<Image>();im.color=color;var r=g.GetComponent<RectTransform>();r.anchorMin=a;r.anchorMax=b;r.offsetMin=r.offsetMax=Vector2.zero;if(joy)g.AddComponent<MobileJoystick>();else g.AddComponent<TouchLookArea>();}
    static void HudButton(Transform par,string n,Vector2 a,Vector2 size){var g=new GameObject(n);g.transform.SetParent(par,false);var im=g.AddComponent<Image>();im.color=new Color(.08f,.1f,.12f,.55f);g.AddComponent<Button>();g.AddComponent<HoldInput>();var r=g.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=a;r.sizeDelta=size;var t=new GameObject("Label");t.transform.SetParent(g.transform,false);var txt=t.AddComponent<Text>();txt.text=n;txt.font=Resources.GetBuiltinResource<Font>("Arial.ttf");txt.fontSize=22;txt.color=Color.white;txt.alignment=TextAnchor.MiddleCenter;var tr=t.GetComponent<RectTransform>();tr.anchorMin=Vector2.zero;tr.anchorMax=Vector2.one;tr.offsetMin=tr.offsetMax=Vector2.zero;}
}
#endif
