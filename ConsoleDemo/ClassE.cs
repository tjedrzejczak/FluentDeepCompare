namespace ConsoleDemo
{
    public class ClassE
    {
        public int N0 { get; set; }
        public int N1 { get; set; }
        public int N2 { get; set; }

        public ClassE(int n)
        {
            N0 = n - 1;
            N1 = n;
            N2 = n * n;
        }
    }
}