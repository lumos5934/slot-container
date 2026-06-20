
namespace LLib
{
    public interface IItem<TKey>
    {
        TKey Key { get; }
        int MaxStack { get; }
    }
}
