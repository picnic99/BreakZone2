using System.Collections.Generic;

public class AnimConfiger : Singleton<AnimConfiger>, Manager
{
    private List<AnimationVO> List;

    public void Init()
    {
        if (List == null) List = new List<AnimationVO>();

        foreach (var item in Configer.Tables.TbAnimation.DataList)
        {
            var anim = new AnimationVO();
            anim.animation = item;
            List.Add(anim);
        }
    }
    public AnimationVO GetAnimByAnimKey(string key)
    {
        var vo = List.Find((item) => { return item.animation.Key == key; });
        return vo;
    }
}