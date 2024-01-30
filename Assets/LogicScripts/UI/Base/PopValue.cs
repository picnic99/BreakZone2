using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class PopValue : UIItemBase
{
    private TextMeshProUGUI commonTxt { get { return UIBase.GetBind<TextMeshProUGUI>(Root, "commonTxt"); } }

    private Transform targetPos;

    public PopValue(GameObject parent, Transform target, float value)
    {
        Root = ResourceManager.GetInstance().GetObjInstance<GameObject>("prefabs/UI/" + RegPrefabs.PopValue);
        targetPos = target;
        Root.transform.SetParent(parent.transform);
        commonTxt.text = value.ToString();
        Root.transform.position = CameraManager.GetInstance().CamRoot.WorldToScreenPoint(targetPos.position);

        //TweenManager.GetInstance().MoveTo(Root.transform, Root.transform.position + new Vector3(0,100,0) ,2f);

        Root.transform.DOMove(Root.transform.position + new Vector3(0, 100, 0), 0.5f).SetEase(Ease.OutExpo);
        TimeManager.GetInstance().AddOnceTimer(this, 0.5f, OnEnd);
        //Root.transform.DOMove(Root.transform.position + new Vector3(0, -100, 0), 1f).SetEase(Ease.InExpo).SetDelay(0.3f);
        //TimeManager.GetInstance().AddFrameLoopTimer(this, 0, 3f, OnUpdate, OnDestory);
    }

    public void OnEnd()
    {
        MonoBridge.GetInstance().DestroyOBJ(Root);
}

}
