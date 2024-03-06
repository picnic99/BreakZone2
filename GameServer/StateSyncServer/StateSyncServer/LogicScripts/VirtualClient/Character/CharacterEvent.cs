namespace StateSyncServer.LogicScripts.VirtualClient.Characters
{
    /// <summary>
    /// 角色事件
    /// </summary>
    public class CharacterEvent
    {
        //攻击
        public static string ATK = "CharacterEvent_ATK";
        //准备攻击
        public static string PRE_ATK = "CharacterEvent_PRE_ATK";
        //状态结束
        public static string STATE_OVER = "CharacterEvent_STATE_OVER";
        //施法技能
        public static string DO_SKILL = "CharacterEvent_DO_SKILL";
        //释放普攻
        public static string DO_ATK = "CharacterEvent_DO_ATK";
        //改变状态
        public static string CHANGE_STATE = "CharacterEvent_CHANGE_STATE";
        //添加状态
        public static string ADD_STATE = "CharacterEvent_ADD_STATE";
        //造成伤害
        public static string MAKE_DAMAGE = "CharacterEvent_MAKE_DAMAGE";
        //受到伤害
        public static string GET_DAMAGE = "CharacterEvent_GET_DAMAGE";
        //属性值改变
        public static string PROPERTY_CHANGE = "CharacterEvent_PROPERTY_CHANGE";
        //动画发生改变
        public static string ANIM_CHANGE = "CharacterEvent_ANIM_CHANGE";

        //需要拓展各种时机点的事件 如攻击开始 攻击中 攻击伤害触发 攻击结束等等
    }
}