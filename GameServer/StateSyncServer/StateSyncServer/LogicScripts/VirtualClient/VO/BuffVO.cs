﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateSyncServer.LogicScripts.VirtualClient.VO
{
    public class BuffVO
    {
        public int id;
        public string buffName;
        public float durationTime;
        public object[] buffParam;
        public Action<object[]> callBack;

        public BuffVO(string buffName)
        {
            this.buffName = buffName;
        }
        public BuffVO(string buffName, float durationTime)
        {
            this.buffName = buffName;
            this.durationTime = durationTime;
        }
        public BuffVO(string buffName, Action<object[]> callBack, params object[] buffParam)
        {
            this.buffName = buffName;
            this.callBack = callBack;
            this.buffParam = buffParam;
        }
    }
}
