namespace ConsoleDemo
{
    public class BaseClassA
    {
        internal int F0 = 7;
        public int F1 = 4;
        protected int F2 = 7;
        private int F3 = 12;

        public bool F4 = true;
        public string F5 = "Gamma";
        public decimal F6 = 7.123m;
        public EnumA F7 = EnumA.ALFA;

        public int? F8 = null;
        public string F9 = null;


        internal int P0 { get; } = 7;
        public int P1 { get; } = 4;
        protected int P2 { get; } = 7;
        private int P3 { get; } = 12;

        public bool P4 { get; } = true;
        public string P5 { get; } = "Gamma";
        public decimal P6 { get; } = 7.123m;
        public EnumA P7 { get; } = EnumA.ALFA;

        public int? P8 { get; } = null;
        public string P9 { get; } = null;

        public BaseClassA Source { get; set; }
    }
}