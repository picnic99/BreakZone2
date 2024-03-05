using Msg;
using StateSyncServer.LogicScripts.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.Manager
{
    class CharacterManager : Manager<CharacterManager>
    {
        public static string PLAYER_OPT_CHANGE = "PLAYER_OPT_CHANGE";
        public static string PLAYER_ADD_TO_SCENE = "PLAYER_ADD_TO_SCENE";

        private Dictionary<int, Character> datas = new Dictionary<int, Character>();

        public CharacterManager()
        {

        }

        public override void AddEventListener()
        {
            base.AddEventListener();
            On(PLAYER_OPT_CHANGE, OnPlayeOptChange);
            On(PLAYER_ADD_TO_SCENE, OnPlayeOptChange);
        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            Off(PLAYER_OPT_CHANGE, OnPlayeOptChange);
            Off(PLAYER_ADD_TO_SCENE, OnPlayeOptChange);
        }

        public void OnPlayeOptChange(object[] args)
        {
            int playerId = (int)args[0];
            GamePlayerOptReq opt = args[1] as GamePlayerOptReq;

            var c = FindCharacter(playerId);
            if (c != null)
            {
                c.ApplyOpt(opt);
            }
        }

        public void OnPlayerAddToScene(object[] args)
        {
            int playerId = (int)args[0];
            //检测是否已经有实体存在
            //存在：初始化实体的数据 如属性值 当前的状态 位置旋转 携带的技能和buff
            //不存在：创建一个新的实体
            Character character = FindCharacter(playerId);
            Player player = PlayerManager.GetInstance().FindPlayer(playerId);
            if(character == null)
            {
                character = CreateCharacter(player.lastSelectCrtId,playerId);
            }
            character.Init();
        }

        public Character CreateCharacter(int crtId, int playerId)
        {
            var c = new Character(playerId);
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
