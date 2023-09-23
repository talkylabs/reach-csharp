using System;
using System.Reflection;
using NUnitLite;

namespace Reach.Tests
{
    class Program
    {
        static int Main(string[] args)
        {
#if NET35
            return new AutoRun(typeof(ReachTest).Assembly).Execute(args);
#else
            return new AutoRun(typeof(ReachTest).GetTypeInfo().Assembly).Execute(args);
#endif
        }
    }
}
