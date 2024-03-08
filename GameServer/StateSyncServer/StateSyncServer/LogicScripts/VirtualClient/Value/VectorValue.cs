
using System.Numerics;

namespace StateSyncServer.LogicScripts.VirtualClient.Value
{
    public class VectorValue : ValueBase<Vector3>
    {
        public VectorValue(Vector3 value)
        {
            baseValue = value;
            UpdateValue();
        }

        public void ClearAll()
        {
            modGroups = new ModifierGroup<Vector3>();
            UpdateValue();
        }

        public override Vector3 GetGroupTotalValue(ModifierGroup<Vector3> groups)
        {
            Vector3 total = baseValue;
            foreach (var item in modGroups.groups)
            {
                if (item.enable)
                {
                    total += item.value;
                }
            }
            return total;
        }

        public void RemoveY()
        {
            foreach (var item in modGroups.groups)
            {
                if (item.value.Y != 0)
                {
                    item.value.Y = 0;
                }
            }

        }
    }
}