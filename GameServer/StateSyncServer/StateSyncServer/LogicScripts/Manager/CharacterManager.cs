using Msg;
using StateSyncServer.LogicScripts.VirtualClient.Bases;
using StateSyncServer.LogicScripts.VirtualClient.Characters;
using StateSyncServer.LogicScripts.VirtualClient.Configer;
using StateSyncServer.LogicScripts.VirtualClient.Enum;
using StateSyncServer.LogicScripts.VirtualClient.States;
using StateSyncServer.LogicScripts.VO;
using System.Collections.Generic;

namespace StateSyncServer.LogicScripts.Manager
{
    class CharacterManager : Manager<CharacterManager>
    {
        public int curCrtIdIndex = 1;

        public static string PLAYER_OPT_CHANGE = "PLAYER_OPT_CHANGE";
        public static string PLAYER_ADD_TO_SCENE = "PLAYER_ADD_TO_SCENE";
        public static string CHARACTER_DIE = "CHARACTER_DIE";

        /// <summary>
        /// Playerid - Character
        /// </summary>
        private Dictionary<int, Character> datas = new Dictionary<int, Character>();

        public CharacterManager()
        {

        }

        public override void AddEventListener()
        {
            base.AddEventListener();
            //On(PLAYER_OPT_CHANGE, OnPlayeOptChange);
            //On(PLAYER_ADD_TO_SCENE, OnPlayerAddToScene);
            On(CHARACTER_DIE, OnCharacterDie);
            On(CharacterEvent.PROPERTY_CHANGE, OnCharacterDie);
        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            //Off(PLAYER_OPT_CHANGE, OnPlayeOptChange);
            //Off(PLAYER_ADD_TO_SCENE, OnPlayerAddToScene);
            Off(CHARACTER_DIE, OnCharacterDie);
            Off(CharacterEvent.PROPERTY_CHANGE, OnCharacterDie);

        }

        /// <summary>
        /// 当角色死亡
        /// </summary>
        /// <param name="args"></param>
        public void OnCharacterDie(object[] args)
        {
            Character crt = args[0] as Character;
            if (crt != null)
            {
                crt.eventDispatcher.Event(CharacterEvent.CHANGE_STATE, crt, StateType.Die);
            }
        }

        /// <summary>
        /// 当角色的属性发生改变
        /// </summary>
        /// <param name="args"></param>
        public void OnPropertyChange(object[] args)
        {
            PropertyType type = (PropertyType)args[0];
            PropertyValue value = (PropertyValue)args[1];
            Character crt = (Character)args[2];


        }

        /// <summary>
        /// 当玩家进入场景
        /// </summary>
        /// <param name="args"></param>
        public void OnPlayerAddToScene(int pId)
        {
            int playerId = pId;
            //检测是否已经有实体存在
            //存在：初始化实体的数据 如属性值 当前的状态 位置旋转 携带的技能和buff
            //不存在：创建一个新的实体
            Character character = FindCharacter(playerId);
            Player player = PlayerManager.GetInstance().FindPlayer(playerId);
            if(character == null)
            {
                character = CreateCharacter(player.lastSelectCrtId,playerId);
            }
            character.InitData();
        }

        public Character CreateCharacter(int crtId, int playerId)
        {
            VirtualClient.VO.CharacterVO vo = CharacterConfiger.GetInstance().GetCharacterById(crtId);
            var c = new Character(vo,playerId);
            datas.Add(playerId, c);
            return c;
        }

        public Character FindCharacter(int playerId)
        {
            if (datas.ContainsKey(playerId))
            {
                return datas[playerId];
            }
            return null;
        }

        public bool RemoveCharacter(Character crt)
        {
            int key = 0;
            foreach (var item in datas)
            {
                if (item.Value == crt)
                {
                    key = item.Key;
                    break;
                }
            }
            if (key > 0)
            {
                datas.Remove(key);
                return true;
            }
            return false;
        }

        public void Tick()
        {
            foreach (var item in datas)
            {
                item.Value.Tick();
            }
        }
    }
}
