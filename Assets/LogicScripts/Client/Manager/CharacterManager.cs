using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client.Manager
{
    public class CharacterManager : Manager<CharacterManager>
    {
        /// <summary>
        /// playerId - character
        /// </summary>
        public Dictionary<int, Assets.LogicScripts.Client.Entity.Character> datas = new Dictionary<int, Assets.LogicScripts.Client.Entity.Character>();

        public Assets.LogicScripts.Client.Entity.Character CreateCharacter(int crtId, int playerId)
        {
            var c = new Assets.LogicScripts.Client.Entity.Character(crtId);
            datas.Add(playerId, c);
            return c;
        }

        public Assets.LogicScripts.Client.Entity.Character FindCharacter(int playerId)
        {
            if (datas.ContainsKey(playerId))
            {
                return datas[playerId];
            }
            return null;
        }

        public bool RemoveCharacter(Assets.LogicScripts.Client.Entity.Character crt)
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

        public override void AddEventListener()
        {
            base.AddEventListener();
        }

        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
        }
    }
}
