using System.Collections.Generic;

namespace ConsoleDemo
{
    public class ClassB : BaseClassA
    {
        public ClassD P21 { get; } = new ClassD(2);

        protected EnumA P22 { get; } = EnumA.ALFA;
        private EnumA P23 { get; } = EnumA.ALFA;
        internal EnumA P24 { get; } = EnumA.BETA;

        public EnumA P25 { get; } = EnumA.BETA;

        public Dictionary<int, string> DictA { get; } = new Dictionary<int, string>();
        public HashSet<string> HashA { get; } = new HashSet<string>();

        public ClassE PropDE1 { get; } = new ClassE(2);
        public ClassE PropDE2 { get; } = new ClassE(3);

        public List<ClassD> ListA = new List<ClassD> { new ClassD(2), new ClassD(4), new ClassD(5) };
        public List<ClassD> ListB = new List<ClassD> { new ClassD(2) };
        public List<ClassE> ListC = new List<ClassE> { new ClassE(2), new ClassE(2) };

        public CollectionA ListCA = new CollectionA();
        public CollectionA ListCB = new CollectionA();

        public ClassD[] ArrayA { get; set; } = new ClassD[3];
        public ClassD[] ArrayB = new ClassD[3];

        public ClassD PropCollA { get; set; }

        public ClassB()
        {
            DictA.Add(1, "alfa");
            DictA.Add(2, "beta");

            HashA.Add("alfa");
            HashA.Add("beta");

            ListCB.Add(new ItemA() { Name = "alfa" });
        }
    }
}