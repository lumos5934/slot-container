
namespace LLib
{
    public class ExampleItem : IItem<string>
    {
        public string Key { get; private set; }
        public int MaxStack { get; private set; }

        public string Name { get; private set; }

        public ExampleItem(string key, string name, int maxStack)
        {
            Key = key;
            Name = name;
            MaxStack = maxStack;
        }

        public override string ToString()
        {
            return $"{Name} ({Key})";
        }
    }
}
