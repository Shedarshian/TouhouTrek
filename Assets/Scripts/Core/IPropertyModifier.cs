namespace ZMDFQ
{
    interface IPropertyModifier<T>
    {
        string propName { get; }
        void modify(ref T value);
    }
}
