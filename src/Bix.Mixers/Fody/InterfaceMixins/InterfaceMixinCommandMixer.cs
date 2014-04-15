﻿using Bix.Mixers.Fody.ILCloning;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bix.Mixers.Fody.InterfaceMixins
{
    internal class InterfaceMixinCommandMixer
    {
        public InterfaceMixinCommandMixer(TypeDefinition interfaceType, TypeDefinition mixinType, TypeDefinition target)
        {
            Contract.Requires(interfaceType != null);
            Contract.Requires(interfaceType.IsInterface);
            Contract.Requires(mixinType != null);
            Contract.Requires(mixinType.IsClass);
            Contract.Requires(target != null);
            Contract.Requires(target.Module != null);
            Contract.Requires(!target.IsValueType);
            Contract.Requires(!target.IsPrimitive);

            Contract.Ensures(this.InterfaceType != null);
            Contract.Ensures(this.InterfaceType.IsInterface);
            Contract.Ensures(this.MixinType != null);
            Contract.Ensures(this.MixinType.IsClass);
            Contract.Ensures(this.Target != null);
            Contract.Ensures(this.Target.IsClass);
            Contract.Ensures(this.TargetModule != null);
            Contract.Ensures(this.Source != null);

            if (!mixinType.Interfaces.Any(@interface => @interface.FullName == interfaceType.FullName))
            {
                throw new ArgumentException("Must implement the interface specified in the interfaceType argmuent", "mixinType");
            }

            this.InterfaceType = interfaceType;
            this.MixinType = mixinType;
            this.Source = TypeSourceWithRoot.CreateWithRootSourceAndTarget(mixinType, target);
            this.TargetModule = target.Module;
            this.Target = target;
        }

        public TypeDefinition InterfaceType { get; private set; }

        public TypeDefinition MixinType { get; private set; }

        private TypeSourceWithRoot Source { get; set; }

        private ModuleDefinition TargetModule { get; set; }

        private TypeDefinition target;
        public TypeDefinition Target
        {
            get { return this.target; }
            private set
            {
                Contract.Requires(value != null);
                Contract.Ensures(this.Target != null);

                if (value.Interfaces.Any(@interface => @interface.Resolve() == this.Source.Source))
                {
                    throw new ArgumentException("Cannot set a target type that already implements the interface to be mixed", "value");
                }

                this.target = value;
            }
        }

        public void Execute()
        {
            this.Target.Interfaces.Add(this.TargetModule.Import(this.InterfaceType));
            var typeCloner = new TypeCloner(this.Target, this.Source);
            typeCloner.CloneStructure();
            typeCloner.CloneLogic();
        }
    }
}