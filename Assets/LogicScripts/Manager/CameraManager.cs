using Cinemachine;
using UnityEngine;


public enum CameraState
{
    MAIN, FEATURE, ARM
}
public class CameraManager : Singleton<CameraManager>, Manager
{
    public CameraState state = CameraState.MAIN;

    public CinemachineBrain brain;
    public CinemachineVirtualCameraBase mainCam;
    public CinemachineVirtualCameraBase featureCam;
    public CinemachineVirtualCameraBase armCam;

    public CinemachineVirtualCameraBase curCam;

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

    public void Init()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
        mainCam = GameObject.Find("CAM_main").GetComponent<CinemachineVirtualCameraBase>();
        featureCam = GameObject.Find("CAM_feature").GetComponent<CinemachineVirtualCameraBase>();
        armCam = GameObject.Find("CAM_arm").GetComponent<CinemachineVirtualCameraBase>();

        MonoBridge.GetInstance().AddCall(OnUpdate);
    }

    public void ShowMainCam(Character character)
    {
        mainCam.Follow = character.trans;
        mainCam.LookAt = character.anim.GetBoneTransform(HumanBodyBones.Spine);
        mainCam.Priority = ++priority;
        state = CameraState.MAIN;
        curCam = mainCam;
    }

    public void ShowFeatureCam(Character character)
    {
        featureCam.Follow = character.trans;
        featureCam.LookAt = character.anim.GetBoneTransform(HumanBodyBones.Spine);
        featureCam.Priority = ++priority;
        state = CameraState.FEATURE;
        curCam = featureCam;

    }
    public void ShowArmCam(Character character)
    {
        armCam.Follow = character.trans.Find("armTarget");
        armCam.LookAt = character.trans.Find("armTarget");
        curLookCharacter = character;
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

    private void OnUpdate()
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