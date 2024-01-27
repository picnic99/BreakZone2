using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenManager : Manager<TweenManager>
{
    public List<Tweener> tweens = new List<Tweener>();

    public void MoveTo(Transform from, Transform to, float time, Ease ease = Ease.Linear)
    {
        MoveTo(from, to.position, time, ease);
    }

    public void MoveTo(Transform from, Vector3 to, float time, Ease ease = Ease.Linear)
    {
        from.DOMove(to, time).SetEase(ease);

    }
    public void MoveLocalTo(Transform from, Vector3 to, float time, Ease ease = Ease.Linear)
    {
        from.DOLocalMove(to, time).SetEase(ease);

    }


    public void MoveDirectTo(Transform from, Vector3 target, float time)
    {
        from.DOMove(target, time).SetRelative();
    }

    public Tweener FollowMoveTo(Transform from, Transform to, float time)
    {
        Tweener tween = null;
        tween = from.DOMove(to.position, time).OnUpdate(() =>
        {
            tween.ChangeEndValue(to.position, time, true);
            if (Vector3.Distance(from.position, to.position) <= 0.5f)
            {
                tween.Kill();
                tweens.Remove(tween);
            }
        }).SetSpeedBased();
        from.DOLookAt(to.position, 0.1f).SetAutoKill();
        //tween.SetEase(Ease.InOutSine);
        tweens.Add(tween);
        return tween;
    }

    public void Init()
    {

    }
}
