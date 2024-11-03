/*
Auther: John Blue
Time: 2022/5
Platform: VS2017
Object: to show inheritance of class, how to upcast, down cast, determine which class the object belong

Inheritance of C# ::

Public inheritance only

Inheritance ::

is a property to inheriant assets from base class
is mecahnism to inheriant asset

Override ::

is a mechanism for a dirived class to redefine functions in base class to meet its own demand
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheritanceExample
{
    class Base
    {
        public virtual void fun()
        {
            Console.WriteLine("Base: fun");
        }

        // static variable cannot be inherited in C#
        // but C++ can
        public static int num = 3;

        // static function cannot be inherited in C# and C++
        public static void static_fun()
        {
            Console.WriteLine("Base: static fun");
        }
    }

    class Derived1 : Base
    {
        public void fun()
        {
            Console.WriteLine("Derived 1: fun");
        }

        public static void static_fun()
        {
            Console.WriteLine("Derived 1: static fun");
        }
    }

    class Derived2 : Base
    {
        // sealed function cannot be overriden
        // sealed class cannot be inherited
        public sealed override void fun()
        {
            Console.WriteLine("Derived 2: fun");
        }

        // static function cannot be sealed, virtual, override
        /*
        public sealed override static void static_fun()
        {
            Console.WriteLine("Derived 2: static fun");
        }
        */
    }

    class InheritanceExample
    {
        static void Main(string[] args)
        {
            // Declaring an array of object
            // Allocating memory for 2 objects of type Base
            Base[] en = new Base[3];
            //
            // also, C# have ... to check whether the object belong to specific class
            // class Animal { } 
            // class Dog : Animal { }
            // void PrintTypes(Animal a) { 
            // Console.WriteLine(a is Animal);                   // true 
            // Console.WriteLine(a is Dog);                      // true 
            // Console.WriteLine(a.GetType() == typeof(Animal)); // false 
            // Console.WriteLine(a.GetType() == typeof(Dog));    // true
            // }
            //
            en[0] = new Base();
            en[0].fun();
            //en[0].static_fun();// static function could only be used in its scope
            //
            en[1] = new Derived1();
            en[1].fun();
            //
            en[2] = new Derived2();
            if (en[2] is Derived2 && en[2].GetType() == typeof(Derived2))
            {
                en[2].fun();
            }
            Console.WriteLine("en[2] belong to Base:{0}", en[2] is Base);
            Console.WriteLine("en[2] belong to Derived1:{0}", en[2].GetType() == typeof(Derived1));

            Console.ReadKey();
        }
    }
}
