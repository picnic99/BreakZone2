using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.LogicScripts.Client.CSkill
{
    class CSkillRegClass
    {
        public static CSkillRegClass Instance;

        public Dictionary<int, Type> dic = new Dictionary<int, Type>();
        public static CSkillRegClass GetInstance()
        {
            if (Instance == null)
            {
                Instance = new CSkillRegClass();
                Instance.Init();
            }
            return Instance;
        }

        private void Init()
        {
            dic.Add(SkillEnum.BASE_ATK, typeof(BaseAttack));
            dic.Add(SkillEnum.BASE_JUMP, typeof(BaseJump));
            dic.Add(SkillEnum.BASE_ROLL, typeof(BaseRoll));

            dic.Add(SkillEnum.GAILUN_ATK, typeof(CGaiLunAtk));
            dic.Add(SkillEnum.ZHIMINGDAJI, typeof(CZhiMingDaJiSkill));
            dic.Add(SkillEnum.YONGQI, typeof(YongQiSkill));
            dic.Add(SkillEnum.SHENPAN, typeof(CShenPanSkill));
            dic.Add(SkillEnum.ZHENGYI, typeof(CZhenYiSkill));


            dic.Add(SkillEnum.CHAOFUHE, typeof(ChaoFuHeSkill));
            dic.Add(SkillEnum.FUWENJINGGU, typeof(FuWenJingGuSkill));
            dic.Add(SkillEnum.FASHUYONGDONG, typeof(FaShuYongDongSkill));
            dic.Add(SkillEnum.QUJINZHEYUE, typeof(QuJinZheYueSkill));

            dic.Add(SkillEnum.LIQING_ATK, typeof(LiQingAttack));
            dic.Add(SkillEnum.TIANYINBO, typeof(TianYingBoSkill));
            dic.Add(SkillEnum.JINZHONGZHAO, typeof(JinZhongZhaoSkill));
            dic.Add(SkillEnum.CUIJINDUANGU, typeof(CuiJingDuanGuSkill));
            dic.Add(SkillEnum.SHENLONGBAIWEI, typeof(ShengLongBaiWeiSkill));

        }

        public Type GetType(int key)
        {
            if (!dic.ContainsKey(key))
            {
                Debug.LogError("regClass not found" + key + ", pls check register");
                return null;
            }
            Type t = dic[key];
            return t;
        }
    }
}
