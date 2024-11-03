/*
Auther: John Blue
Time: 2022/5
Platform: VS2017
Object: Polymorphism and Interface

Interface::

a method in interface is a "abstract method" by default

Abstract::

we can also use abstract in class so that the method or the class can be abstract
, forcing the sub-class to override the class and the method

not that the abstract class can not be new directly

public abstract class AbstractExample {
    int x;
    public void abstract abstractMethod() {}
}

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymorphism_and_Interface
{
    // interface
    interface Length
    {
        //float st_length = 333; // cannot have variable
        void set_length(float lt); // public abstract
    }
    interface Width
    {
        void set_width(float wd);
    }
    interface Breadth
    {
        void set_width(float bd);
        void set_breadth(double bd);
    }

    // inheritence of interface
    interface Shape : Length, Width
    {
        float area();
    }

    // implementation of interface or ... polymorphism
    class Geo_Length : Length
    {
        public float length;
        public void set_length(float lt)
        {
            Console.WriteLine("Geo_Length.set_length()");
            length = lt;
        }
    }

    // implementation of Shape interface
    class Rectangle : Shape
    {// same as implements Length, Width
        public float length;
        public float width;
        public void set_length(float lt)
        {
            Console.WriteLine("Geo_Length.set_length()");
            length = lt;
        }
        public void set_width(float wd)
        {
            Console.WriteLine("Rectangle.set_width()");
            width = wd;
        }
        public float area()
        {
            return length * width;
        }
    }

    class Triangle : Geo_Length, Shape, Breadth
    {
        // already inherited from Geo_Length
        //public float length;
        //public void set_length(float lt) { ... }

        // width & breadth
        public float width;
        public float breadth;
        public void set_width(float wd)
        {
            Console.WriteLine("Triangle.set_width()");
            width = wd;
        }

        public void set_breadth(double bd)
        {
            Console.WriteLine("Triangle.set_breadth()");
            breadth = (float)bd;
        }
        public float area()
        {
            float s = (length + width + breadth) / 2;
            return (float)Math.Sqrt(s * (s - length) * (s - width) * (s - breadth));
        }
    }

    class Polymorphism_and_Interface
    {
        // main
        static void Main(string[] args)
        {
            // Geo_Length
            Console.WriteLine("Geo_Length >>");
            Console.WriteLine("new g");
            Geo_Length g = new Geo_Length();
            g.set_length(3);
            Console.WriteLine("g.length = " + g.length + "\n");

            // Rectangle
            Console.WriteLine("Rectangle >>");
            Console.WriteLine("new r");
            Rectangle r = new Rectangle();
            r.set_length(3);
            r.set_width(3);
            Console.WriteLine("r.area = " + r.area() + "\n");

            // Triangle
            Console.WriteLine("Triangle >>");
            Console.WriteLine("new t");
            Triangle t = new Triangle();
            t.set_length(5);
            t.set_width(4);
            t.set_breadth(3);
            Console.WriteLine("t.area = " + t.area() + "\n");

            Console.ReadKey();
        }
    }
}
