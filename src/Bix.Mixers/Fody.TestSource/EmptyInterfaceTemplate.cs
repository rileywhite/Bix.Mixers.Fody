﻿using Bix.Mixers.Fody.ILCloning;
using Bix.Mixers.Fody.TestInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bix.Mixers.Fody.TestSource
{
    public class EmptyInterfaceTemplate : IEmptyInterface
    {
        [Skip]
        public EmptyInterfaceTemplate() { }
    }
}
