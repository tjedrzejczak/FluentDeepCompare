using System.Collections.Generic;

namespace ConsoleDemo
{
    public class ItemA
    {
        public string Name { get; set; }
    }

    public class CollectionA : List<ItemA>
    {
        public string Description { get; set; }
    }
}