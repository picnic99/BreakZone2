using Assets.LogicScripts.Client.Net.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client
{
    /// <summary>
    /// 行为转发器
    /// </summary>
    class ActionManager : Manager<ActionManager>
    {
        public void Handle(Protocol protocol)
        {
/*            bool isFightProto = protocol is PlayerOptReq;

            if (isFightProto)
            {
                //ActionManager.GetInstance().AddAction((PlayerOptReq)protocol);
                FightHandle((PlayerOptReq)protocol);
            }
            else
            {
                LogicHandle(protocol.client, protocol);
            }*/
        }
    }
}
