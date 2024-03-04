using Assets.LogicScripts.Client.VO;
using UnityEngine;

namespace Assets.LogicScripts.Client.Entity
{
    /// <summary>
    /// 角色实体
    /// </summary>
    public class Character
    {
        /// <summary>
        /// 具体控制的角色
        /// </summary>
        public GameObject CrtObj { get; set; }
        /// <summary>
        /// 角色的动画状态机
        /// </summary>
        public CharacterAnimator CharacterAnimator { get; set; }
        /// <summary>
        /// 角色的数据
        /// </summary>
        public CharacterDataVO CrtData { get; set; }

        public CharacterVO CrtVO { get; set; }

        public Character(int CrtId)
        {
            if(CrtId > 0)
            {
                var vo = CharacterConfiger.GetInstance().GetCharacterById(CrtId);
                CrtVO = vo;

                CrtObj = ResourceManager.GetInstance().GetCharacterInstance<GameObject>(vo.character.ModePath);
                var anim = CrtObj.GetComponent<Animator>();
                CharacterAnimator = new CharacterAnimator();
                CharacterAnimator.Init(anim);
                //初始化角色位置
                CrtObj.transform.position = Vector3.zero;
                CrtObj.transform.rotation = Quaternion.identity;
                CrtData = new CharacterDataVO();
                //初始化角色动画 默认播放Idle
                AnimManager.GetInstance().PlayAnim(CharacterAnimator, "DEFAULT_IDLE");
            }
        }

        public void SetActive(bool b)
        {
            if(CrtObj != null)
            {
                CrtObj.SetActive(b);
            }
        }

        public void ApplyCrtData()
        {
            CrtObj.transform.position = CrtData.Pos;
            CrtObj.transform.rotation = Quaternion.AngleAxis(CrtData.Rot, Vector3.up);
        }
    }
}