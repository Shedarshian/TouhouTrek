namespace ZMDFQ
{
    /// <summary>
    /// 属性修正器接口，用于实现属性修正效果，比如类似“手牌上限+1”这样的技能效果。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IPropertyModifier<T>
    {
        /// <summary>
        /// 要修正的属性名称，建议配合nameof使用。
        /// </summary>
        string propName { get; }
        /// <summary>
        /// 属性修正计算方式。
        /// </summary>
        /// <param name="value">修正前属性值</param>
        void modify(ref T value);
    }
}
