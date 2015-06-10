using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo_FromClassLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly asm = Assembly.Load(AssemblyName.GetAssemblyName("Person_Library.dll"));
            Module mod = asm.GetModule("Person_Library.dll");

            Console.WriteLine("Оголошені типи даних:");
            foreach (Type t in mod.GetTypes())
            {
                Console.WriteLine(t.FullName);
            }
            Console.WriteLine();

            Type Person = mod.GetType("Person_Library.Person") as Type;
            object person = Activator.CreateInstance(Person, new object[] {"Ivan", "Ivanov", 25});

            Person.GetMethod("Print").Invoke(person, null);
        }
    }
}
