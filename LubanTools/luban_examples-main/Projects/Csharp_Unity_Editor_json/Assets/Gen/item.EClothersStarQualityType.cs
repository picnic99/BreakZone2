
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace editor.cfg.item
{
    public enum EClothersStarQualityType
    {
        /// <summary>
        /// 一星
        /// </summary>
        ONE = 1,
        /// <summary>
        /// 二星
        /// </summary>
        TWO = 2,
        /// <summary>
        /// 三星
        /// </summary>
        THREE = 3,
        /// <summary>
        /// 四星
        /// </summary>
        FOUR = 4,
        /// <summary>
        /// 五星
        /// </summary>
        FIVE = 5,
        /// <summary>
        /// 六星
        /// </summary>
        SIX = 6,
        /// <summary>
        /// 七星
        /// </summary>
        SEVEN = 7,
        /// <summary>
        /// 八星
        /// </summary>
        EIGHT = 8,
        /// <summary>
        /// 九星
        /// </summary>
        NINE = 9,
        /// <summary>
        /// 十星
        /// </summary>
        TEN = 10,
    }

    public static class EClothersStarQualityType_Metadata
    {
        public static readonly Luban.EditorEnumItemInfo ONE = new Luban.EditorEnumItemInfo("ONE", "一星", 1, "一星");
        public static readonly Luban.EditorEnumItemInfo TWO = new Luban.EditorEnumItemInfo("TWO", "二星", 2, "二星");
        public static readonly Luban.EditorEnumItemInfo THREE = new Luban.EditorEnumItemInfo("THREE", "三星", 3, "三星");
        public static readonly Luban.EditorEnumItemInfo FOUR = new Luban.EditorEnumItemInfo("FOUR", "四星", 4, "四星");
        public static readonly Luban.EditorEnumItemInfo FIVE = new Luban.EditorEnumItemInfo("FIVE", "五星", 5, "五星");
        public static readonly Luban.EditorEnumItemInfo SIX = new Luban.EditorEnumItemInfo("SIX", "六星", 6, "六星");
        public static readonly Luban.EditorEnumItemInfo SEVEN = new Luban.EditorEnumItemInfo("SEVEN", "七星", 7, "七星");
        public static readonly Luban.EditorEnumItemInfo EIGHT = new Luban.EditorEnumItemInfo("EIGHT", "八星", 8, "八星");
        public static readonly Luban.EditorEnumItemInfo NINE = new Luban.EditorEnumItemInfo("NINE", "九星", 9, "九星");
        public static readonly Luban.EditorEnumItemInfo TEN = new Luban.EditorEnumItemInfo("TEN", "十星", 10, "十星");

        private static readonly System.Collections.Generic.List<Luban.EditorEnumItemInfo> __items = new System.Collections.Generic.List<Luban.EditorEnumItemInfo>
        {
            ONE,
            TWO,
            THREE,
            FOUR,
            FIVE,
            SIX,
            SEVEN,
            EIGHT,
            NINE,
            TEN,
        };

        public static System.Collections.Generic.List<Luban.EditorEnumItemInfo> GetItems() => __items;

        public static Luban.EditorEnumItemInfo GetByName(string name)
        {
            return __items.Find(c => c.Name == name);
        }

        public static Luban.EditorEnumItemInfo GetByNameOrAlias(string name)
        {
            return __items.Find(c => c.Name == name || c.Alias == name);
        }

        public static Luban.EditorEnumItemInfo GetByValue(int value)
        {
            return __items.Find(c => c.Value == value);
        }
    }

} 

