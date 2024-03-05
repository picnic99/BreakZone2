using StateSyncServer.LogicScripts.Util;
using StateSyncServer.LogicScripts.VirtualClient.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.Configer
{
    /// <summary>
    /// 状态的数据解析类
    /// </summary>
    public class StateConfiger : Singleton<StateConfiger>
    {
        private List<StateVO> List;

        public void Init()
        {
            if (List == null) List = new List<StateVO>();
            foreach (var item in Configer.Tables.TbState.DataList)
            {
                var anim = new StateVO();
                anim.state = item;
                List.Add(anim);
            }
        }

        /// <summary>
        /// 根据状态类型获取状态信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StateVO GetStateByType(string type)
        {
            var vo = List.Find((item) => {
                return item.state.Type == type;
            });
            return vo;
        }
    }
}
