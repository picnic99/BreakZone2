
using Assets.LogicScripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.LogicScripts.Client.Manager
{
    public class EventDispatcher
    {
        public Dictionary<string, Delegate> events = new Dictionary<string, Delegate>();

        private void AddDelegate(string eventName, Delegate callback)
        {
            Delegate value;
            if (!events.TryGetValue(eventName, out value))
            {
                //key不存在就添加
                events.Add(eventName, callback);
            }
            else
            {
                //key存在判断value是否为空，为空就替换，不为空就多播
                value = value != null ? Delegate.Combine(value, callback) : callback;
                events[eventName] = value;
            }
        }

        private void RemoveDelegate(string eventName, Delegate callback)
        {
            Delegate func;
            if (events.TryGetValue(eventName, out func))
            {
                if (func != null)
                {
                    func = Delegate.Remove(func, callback);
                    events[eventName] = func;
                }
                else
                {
                    CommonUtils.Logout("Key:" + eventName + "不存在！");
                }
            }
        }

        public void On(string eventName, Action<object[]> callback)
        {
            AddDelegate(eventName, callback);
        }
        public void On(int eventName, Action<object[]> callback)
        {
            AddDelegate(eventName + "", callback);
        }

        public void Off(string eventName, Action<object[]> callback)
        {
            RemoveDelegate(eventName, callback);
        }
        public void Off(int eventName, Action<object[]> callback)
        {
            RemoveDelegate(eventName + "", callback);
        }

        public void Event(string eventName, params object[] args)
        {
            events.TryGetValue(eventName, out Delegate call);
            if (call != null)
            {
                ((Action<object[]>)call).Invoke(args);
            }
        }
        public void Event(int eventName, params object[] args)
        {
            events.TryGetValue(eventName + "", out Delegate call);
            if (call != null)
            {
                ((Action<object[]>)call).Invoke(args);
            }
        }
    }
}
