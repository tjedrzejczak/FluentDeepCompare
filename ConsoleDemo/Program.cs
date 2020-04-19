using System;
using FluentDeepCompare;

namespace ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var p = new Program();
                p.DemoGlobalConfiguration();
                p.DemoLocalConfiguration();
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Press enter ...");
            Console.ReadLine();
        }

        private void DemoGlobalConfiguration()
        {
            ObjectComparer.Configure(c => c
                .IncludePublicProperties()
                .IncludePublicFields()
            );

            var objB = new ClassB();
            var objC = new ClassC();

            var result = ObjectComparer.Compare(objB, objC);
            Console.WriteLine($"Result:{result.AreEqual}");
            Console.WriteLine(result.AggregateAllMessages());
            Console.WriteLine();
        }

        private void DemoLocalConfiguration()
        {
            var comparer = ObjectComparer.Create(useGlobal: true, c => c
                .IncludeCollections()
                .IncludeNotPublicFields()
                .IncludePublicFields()
                .Ignore<ClassE>(x => x.N1)
            );

            var objB = new ClassB() { Source = new BaseClassA() };
            var objC = new ClassC() { Source = new BaseClassA() };

            var result = comparer.Compare(objB, objC);
            Console.WriteLine($"Result:{result.AreEqual}");
            Console.WriteLine(result.AggregateAllMessages());
            Console.WriteLine();
        }
    }
}