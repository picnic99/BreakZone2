using Cinemachine;
using UnityEngine;


public enum CameraState
{
    MAIN, FEATURE, ARM
}
public class CameraManager : Manager<CameraManager>
{
    public CameraState state = CameraState.MAIN;

    public CinemachineBrain brain;
    public CinemachineVirtualCameraBase mainCam;
    public CinemachineVirtualCameraBase featureCam;
    public CinemachineVirtualCameraBase armCam;

    public CinemachineVirtualCameraBase curCam;

    public GameObject camRoot;

    public Camera CamRoot { get { return camRoot.GetComponent<Camera>(); } }

    public Character curLookCharacter;

    private int priority = 1;

    private float rotationSpeed = 1f;
    private float _cinemachineTargetPitchY;
    private float _cinemachineTargetPitchX;
    private const float _threshold = 0.01f;


    private CinemachineImpulseSource _impulse;
    private CinemachineImpulseSource Impulse
    {
        get
        {
            if (_impulse == null)
            {
                brain.TryGetComponent<CinemachineImpulseSource>(out _impulse);
                if (_impulse == null)
                {
                    _impulse = brain.gameObject.AddComponent<CinemachineImpulseSource>();
                }
            }
            return _impulse;
        }
    }

    public void SetTarget(object[] args)
    {
        Character crt = args[0] as Character;
        curLookCharacter = crt;
        ShowMainCam();
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventDispatcher.GetInstance().On(EventDispatcher.MAIN_ROLE_CHANGE, SetTarget);
    }

    public override void Init()
    {
        var cam = GameMain.GetInstance().transform.Find("CrtCamera").transform;
        camRoot = cam.gameObject;
        brain = cam.GetComponent<CinemachineBrain>();
        mainCam = cam.Find("CAM_main").GetComponent<CinemachineVirtualCameraBase>();
        featureCam = cam.Find("CAM_feature").GetComponent<CinemachineVirtualCameraBase>();
        armCam = cam.Find("CAM_arm").GetComponent<CinemachineVirtualCameraBase>();

        MonoBridge.GetInstance().AddCall(OnUpdate);
        base.Init();
    }

    public void OpenCam()
    {
        camRoot.SetActive(true);
    }

    public void CloseCam()
    {
        camRoot.SetActive(false);
    }

    public void ShowMainCam()
    {
        mainCam.Follow = curLookCharacter.trans;
        mainCam.LookAt = curLookCharacter.anim.GetBoneTransform(HumanBodyBones.Spine);
        mainCam.Priority = ++priority;
        state = CameraState.MAIN;
        curCam = mainCam;
    }

    public void ShowFeatureCam()
    {
        featureCam.Follow = curLookCharacter.trans;
        featureCam.LookAt = curLookCharacter.anim.GetBoneTransform(HumanBodyBones.Spine);
        featureCam.Priority = ++priority;
        state = CameraState.FEATURE;
        curCam = featureCam;

    }
    public void ShowArmCam()
    {
        armCam.Follow = curLookCharacter.trans.Find("armTarget");
        armCam.LookAt = curLookCharacter.trans.Find("armTarget");
        state = CameraState.ARM;
        armCam.Priority = ++priority;
        curCam = armCam;
        _cinemachineTargetPitchY = 0;
        _cinemachineTargetPitchX = 0;

    }

    public void HorizontalShake(float force = 1)
    {
#if UNITY_2019_4_9

#else
        Impulse.GenerateImpulseWithVelocity(new Vector3(force, 0, 0));
#endif
    }

    public void VerticalShake(float force = 1)
    {
#if UNITY_2019_4_9

#else
        Impulse.GenerateImpulseWithVelocity(new Vector3(0, force, 0));
#endif
    }

    public void EventImpulse(float force = 1)
    {
#if UNITY_2019_4_9

#else
        Impulse.GenerateImpulseWithForce(force);
#endif
    }

    public override void OnUpdate()
    {
        ArmRotate();
    }

    public void ArmRotate()
    {
        if (state != CameraState.ARM) return;

        var mouseY = Input.GetAxis("Mouse Y");
        var mouseX = Input.GetAxis("Mouse X");
        Vector2 look = new Vector2(mouseX * rotationSpeed, -mouseY * rotationSpeed);


        if (look.sqrMagnitude >= _threshold)
        {
            //cmvCamManager.playerController.RotationVelocity = look.x;
            _cinemachineTargetPitchY += look.y;
            _cinemachineTargetPitchX = 0;
            //_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp_3rd, topClamp_3rd);
            curLookCharacter.trans.Find("armTarget").localRotation = Quaternion.Euler(_cinemachineTargetPitchY, _cinemachineTargetPitchX, 0.0f);
            curLookCharacter.trans.Rotate(Vector3.up * look.x);
        }

    }
}