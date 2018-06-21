using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using rMind.Core;
using System.Linq.Expressions;

namespace rMind.Device
{
    public class MindBuilder
    {
        class A
        {
            public int Get(){ return 1; }
        }
        class B
        {
            public void Set(int v) { Console.WriteLine(string.Format("From Reflection {0}\n\n", v)); }
        }


        public static void Build(IMindCore core)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var AT = assembly.GetTypes().Where(x => x.Name == "A").FirstOrDefault();
            var BT = assembly.GetTypes().Where(x => x.Name == "B").FirstOrDefault();


            var AI = Activator.CreateInstance(AT, new object[] { });
            var BI = Activator.CreateInstance(BT, new object[] { });

                        
            var setter = BT.GetMethod("Set");
            var getter = AT.GetMethod("Get");

            
            var fu = typeof(Func<>).MakeGenericType(getter.ReturnType);
            Func<object> ff = () => getter.Invoke(AI, new object[] { });

            var a = setter.Invoke(BI, new object[] { getter.Invoke(AI, new object[] { }) });

            Func<bool> f = () =>
            {
                setter.Invoke(BI, new object[] { ff.Invoke() });
                return true;   
            };

            f(); 
        }
    }
}
