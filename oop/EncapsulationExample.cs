/*
Auther: John Blue
Time: 2022/5
Platform: VS2017
Object: Encapsulation

(*)friend:
C# has no "friend" mechanism

private:
only itsheld can use

protected:
only itshlef and its child class can use

public:
all class or other process can use

static:
only in this scope can use

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncapsulationExample
{
    class EncapsulationExample
    {
        private int privateVariable;
        protected int protectedVariable;
        public int publicVariable;
        int packageVariable;

        private void privateObjectMethod() { }
        protected void protectedObjectMethod() { }
        public void publicObjectMethod() { }
        void packageObjectMethod() { }

        static private void privateClassMethod() { }
        static protected void protectedClassMethod() { }
        static public void publicClassMethod() { }
        static void packageClassMethod() { }

        static void Main(string[] args)
        {
            EncapsulationExample en = new EncapsulationExample();

            Console.ReadKey();
        }
    }
}
