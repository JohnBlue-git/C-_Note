/*
 Auther: John Blue
 Time: 2022/5
 Platform: VS2017
 Object: to show basic usage of class and object

 For class in C#:
 The public modifier specifies that the members or classes can be accessed openly.
 The private modifier specifies that the member can only be accessed in its own class.
 The protected modifier specifies that the member can only be accessed within its own package (as with package-private) and, in addition, by a subclass of its class in another package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExample
{
    class Other
    {
        public static String name = "Other";
    }

    class ObjectExample
    {
        // variable
        private string name;
        private int id;
        private const string const_name = "s";// can only assign at first place
        //private readonly string read_name;// can only be assign or change in constructorgn const variable here

        // constructor
        public ObjectExample(String s)
        {
            name = s;
            id = s.Length;
            //const_name = s;// cannot assign const variable
        }
        // copy constructor
        // 1. there is no const or final for passed object or value in C#
        // 2. the variable is passed in value by default, but can use "ref": ObjectExample(ref string st)
        //    and the reference variable such as Array and Object are passed in reference by default
        public ObjectExample(ObjectExample obj) {
            name = obj.name;
            id = obj.id;
            //const_name = obj.const_name;// cannot assign const variable
        }
    
        // assigment operator not exsits !!!
    
        // other assignement operator
        //public static Box operator +(const Box a, const Box b) ...
        // c = a + b
        //public static bool operator ==(Box a, Box b) ...
        // a == b

        // function
        public void print()
        {
            Console.WriteLine("object >>" + name);
        }

        static void Main(string[] args)
        {
            // o1 o2 are all reference that point to object
            ObjectExample o1 = new ObjectExample("o1");
            ObjectExample o2 = new ObjectExample("oo2");
            //o1 = o2;// workable o1 is just reference

            // copy
            Console.WriteLine("copy >>");
            ObjectExample o3 = new ObjectExample(o1);
            // by default
            //        == compare reference
            // .Equals() compare reference
            // but string class is override with content comparision  
            // but we can override operators and functions
            if (o1 == o3 && o1.Equals(o3))
            {
                Console.WriteLine("o1 == o3");
            }
            else
            {
                Console.WriteLine("o1 != o3");
                Console.WriteLine("but");
                    Console.Write("o1.print() =");
                    o1.print();
                    Console.Write("o3.print() =");
                    o3.print();
                    Console.WriteLine();
            }

            // pt here is just reference, not assignment
            Console.WriteLine("reference >>");
            ObjectExample pt;
            pt = o1;
            pt.print();
            pt = o2;
            pt.print();
            Console.WriteLine("\n");

            // delete
            Console.WriteLine("delete >>");
            o1 = null;// the object that abandom by the reference will be delete automatically
            GC.Collect();// Collect all generations of memory ... still have GC.Collect(1) GC.Collect(2) ... 
            Console.WriteLine("complete >>\n");

            // access static
            Console.WriteLine("static variable >>");
            Console.WriteLine("Other.name: " + Other.name);
            
            // end
            Console.ReadKey();
        }
    }

}
