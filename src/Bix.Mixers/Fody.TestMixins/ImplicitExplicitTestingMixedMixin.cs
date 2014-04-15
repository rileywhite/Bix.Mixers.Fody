﻿using Bix.Mixers.Fody.ILCloning;
using Bix.Mixers.Fody.TestMixinInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bix.Mixers.Fody.TestMixins
{
    public class ImplicitExplicitTestingMixedMixin : IInterfaceForImplicitExplicitTesting
    {
        [Skip]
        public ImplicitExplicitTestingMixedMixin() { }

        public string Method1()
        {
            return "Implicit 1";
        }

        public string Method2()
        {
            return "Independent 2";
        }

        string IInterfaceForImplicitExplicitTesting.Method2()
        {
            return "Explicit 2";
        }

        string IInterfaceForImplicitExplicitTesting.Method3()
        {
            return "Explicit 3";
        }
    }
}