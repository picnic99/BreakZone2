using System.Collections.Generic;
/// <summary>
/// 角色的数据配置解析类
/// </summary>
public class CharacterConfiger : Singleton<CharacterConfiger>, Manager
{
    private List<CharacterVO> List;

    public void Init()
    {
        if(List == null) List = new List<CharacterVO>();
        foreach (var item in Configer.Tables.TbCharacter.DataList)
        {
            var anim = new CharacterVO();
            anim.character = item;
            List.Add(anim);
        }
    }
    public CharacterVO GetCharacterById(int id)
    {
        var vo = List.Find((item) => { return item.character.Id == id; });
        return vo;
    }
}