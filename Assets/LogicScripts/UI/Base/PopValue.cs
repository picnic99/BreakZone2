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
        Root = ResourceManager.GetInstance().GetObjInstance<GameObject>("UI/" + RegPrefabs.PopValue);
        targetPos = target;
        Root.transform.SetParent(parent.transform);
        commonTxt.text = value.ToString();
        Root.transform.position = Camera.current.WorldToScreenPoint(targetPos.position);

        //TweenManager.GetInstance().MoveTo(Root.transform, Root.transform.position + new Vector3(0,100,0) ,2f);

        Root.transform.DOMove(Root.transform.position + new Vector3(0, 100, 0), 0.5f).SetEase(Ease.OutExpo);
        Root.transform.DOMove(Root.transform.position + new Vector3(0, -100, 0), 1f).SetEase(Ease.InExpo).SetDelay(0.3f);
        //TimeManager.GetInstance().AddFrameLoopTimer(this, 0, 3f, OnUpdate, OnDestory);
    }

    public void OnEnd()
    {
        MonoBridge.GetInstance().DestroyOBJ(Root);
}

}
