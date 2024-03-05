using System.Collections.Generic;

public class AnimCoverVO
{
    public string animName;
    public int sort;

    public AnimCoverVO(string animName, int sort = 0)
    {
        this.animName = animName;
        this.sort = sort;
    }
}

/// <summary>
/// 动画片段覆盖数据
/// 用于记录由技能或者buff导致的动画片段切换
/// </summary>
public class AnimCoverData
{
    public Character character;
    public Dictionary<string, List<AnimCoverVO>> coverDatas;

    public AnimCoverData(Character character)
    {
        this.character = character;
        coverDatas = new Dictionary<string, List<AnimCoverVO>>();
    }

    public void Add(string state, AnimCoverVO vo)
    {
        if (coverDatas.TryGetValue(state, out List<AnimCoverVO> vos))
        {
            vos.Add(vo);
            Sort(vos);
        }
        else
        {
            List<AnimCoverVO> list = new List<AnimCoverVO>();
            list.Add(vo);
            coverDatas.Add(state, list);
        }
        character.eventDispatcher.Event(CharacterEvent.ANIM_CHANGE, state);
    }
    public void Remove(string state, AnimCoverVO vo)
    {
        if (coverDatas.TryGetValue(state, out List<AnimCoverVO> vos))
        {
            vos.Remove(vo);
            Sort(vos);
            //character.eventDispatcher.Event(CharacterEvent.ANIM_CHANGE, state);
        }
    }

    public AnimCoverVO GetHead(string state)
    {
        if (coverDatas.TryGetValue(state, out List<AnimCoverVO> vos))
        {
            if (vos.Count > 0)
            {
                return vos[0];
            }
        }
        return null;
    }


    public void Sort(List<AnimCoverVO> vos)
    {
        if (vos.Count <= 1) return;
        vos.Sort((AnimCoverVO a, AnimCoverVO b) =>
        {
            return b.sort - a.sort;
        });
    }
}