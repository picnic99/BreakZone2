using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System.Collections.Generic;

namespace StateSyncServer.LogicScripts.VirtualClient.Configer
{
    /// <summary>
    /// 角色的数据配置解析类
    /// </summary>
    public class CharacterConfiger : Singleton<CharacterConfiger>
    {
        private List<CharacterVO> List;
        private List<PropertyVO> propertyList;

        public void Init()
        {
            if (List == null) List = new List<CharacterVO>();
            if (propertyList == null) propertyList = new List<PropertyVO>();
            foreach (var item in Configer.Tables.TbCharacter.DataList)
            {
                var anim = new CharacterVO();
                anim.character = item;
                List.Add(anim);
            }
            foreach (var item in Configer.Tables.TbProperty.DataList)
            {
                var p = new PropertyVO();
                p.property = item;
                propertyList.Add(p);
            }
        }
        public CharacterVO GetCharacterById(int id)
        {
            var vo = List.Find((item) => { return item.character.Id == id; });
            return vo;
        }

        public CharacterVO[] GetAllCharacter()
        {
            return List.ToArray();
        }

        public PropertyVO GetPropertyById(int id)
        {
            CharacterVO cVO = GetCharacterById(id);
            if (cVO != null)
            {
                var vo = propertyList.Find((item) => { return item.property.Id == cVO.character.PropertyId; });
                return vo;
            }
            return null;
        }

        public CharacterVO[] GetRealCharacters()
        {
            List<CharacterVO> result = new List<CharacterVO>();
            foreach (var item in List)
            {
                if (!item.character.IsFake)
                {
                    result.Add(item);
                }
            }
            return result.ToArray();
        }
    }
}