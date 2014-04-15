﻿using Bix.Mixers.Fody.Core;
using Bix.Mixers.Fody.InterfaceMixins;
using Bix.Mixers.Fody.TestMixinInterfaces;
using Bix.Mixers.Fody.TestMixins;
using Bix.Mixers.Fody.Tests.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bix.Mixers.Fody.Tests.InterfaceMixinTests
{
    [TestFixture]
    internal class ImplicitAndExplicitImplementationFixture
    {
        [Test]
        public void CanImplementImplicitly()
        {
            var config = new BixMixersConfigType();

            config.MixCommandConfig = new MixCommandConfigTypeBase[]
            {
                new InterfaceMixinConfigType
                {
                    InterfaceMap = new InterfaceMapType[]
                    {
                        new InterfaceMapType
                        {
                            Interface = typeof(IInterfaceForImplicitExplicitTesting).GetShortAssemblyQualifiedName(),
                            Mixin = typeof(ImplicitExplicitTestingImplicitOnlyMixin).GetShortAssemblyQualifiedName()
                        }
                    }
                },
            };

            var assembly = ModuleWeaverHelper.WeaveAndLoadTestTarget(config);
            var targetType = assembly.GetType(typeof(Bix.Mixers.Fody.TestMixinTargets.InterfaceForImplicitExplicitTestingTarget).FullName);

            Assert.That(typeof(IInterfaceForImplicitExplicitTesting).IsAssignableFrom(targetType));
            targetType.ValidateMemberCountsAre(1, 3, 0, 0, 0, 0);

            Assert.That(targetType.GetConstructor(new Type[0]) != null, "Lost existing default constructor");

            var instanceObject = Activator.CreateInstance(targetType, new object[0]);
            Assert.That(instanceObject is IInterfaceForImplicitExplicitTesting);

            Assert.That("Implicit 1".Equals(
                targetType.GetMethod("Method1", TestContent.BindingFlagsForMixedMembers).Invoke(instanceObject, new object[] { })));
            Assert.That("Implicit 2".Equals(
                targetType.GetMethod("Method2", TestContent.BindingFlagsForMixedMembers).Invoke(instanceObject, new object[] { })));
            Assert.That("Implicit 3".Equals(
                targetType.GetMethod("Method3", TestContent.BindingFlagsForMixedMembers).Invoke(instanceObject, new object[] { })));

            var instance = (IInterfaceForImplicitExplicitTesting)instanceObject;

            Assert.That("Implicit 1".Equals(instance.Method1()));
            Assert.That("Implicit 2".Equals(instance.Method2()));
            Assert.That("Implicit 3".Equals(instance.Method3()));
        }

        [Test]
        public void CanImplementExplicitly()
        {
            var config = new BixMixersConfigType();

            config.MixCommandConfig = new MixCommandConfigTypeBase[]
            {
                new InterfaceMixinConfigType
                {
                    InterfaceMap = new InterfaceMapType[]
                    {
                        new InterfaceMapType
                        {
                            Interface = typeof(IInterfaceForImplicitExplicitTesting).GetShortAssemblyQualifiedName(),
                            Mixin = typeof(ImplicitExplicitTestingExplicitOnlyMixin).GetShortAssemblyQualifiedName()
                        }
                    }
                },
            };

            var assembly = ModuleWeaverHelper.WeaveAndLoadTestTarget(config);
            var targetType = assembly.GetType(typeof(Bix.Mixers.Fody.TestMixinTargets.InterfaceForImplicitExplicitTestingTarget).FullName);

            Assert.That(typeof(IInterfaceForImplicitExplicitTesting).IsAssignableFrom(targetType));
            targetType.ValidateMemberCountsAre(1, 3, 0, 0, 0, 0);

            Assert.That(targetType.GetConstructor(new Type[0]) != null, "Lost existing default constructor");

            var instanceObject = Activator.CreateInstance(targetType, new object[0]);
            Assert.That(instanceObject is IInterfaceForImplicitExplicitTesting);

            Assert.That(targetType.GetMethod("Method1", TestContent.BindingFlagsForMixedMembers) == null);
            Assert.That(targetType.GetMethod("Method2", TestContent.BindingFlagsForMixedMembers) == null);
            Assert.That(targetType.GetMethod("Method3", TestContent.BindingFlagsForMixedMembers) == null);

            var instance = (IInterfaceForImplicitExplicitTesting)instanceObject;

            Assert.That("Explicit 1".Equals(instance.Method1()));
            Assert.That("Explicit 2".Equals(instance.Method2()));
            Assert.That("Explicit 3".Equals(instance.Method3()));
        }

        [Test]
        public void CanImplementMixed()
        {
            var config = new BixMixersConfigType();

            config.MixCommandConfig = new MixCommandConfigTypeBase[]
            {
                new InterfaceMixinConfigType
                {
                    InterfaceMap = new InterfaceMapType[]
                    {
                        new InterfaceMapType
                        {
                            Interface = typeof(IInterfaceForImplicitExplicitTesting).GetShortAssemblyQualifiedName(),
                            Mixin = typeof(ImplicitExplicitTestingMixedMixin).GetShortAssemblyQualifiedName()
                        }
                    }
                },
            };

            var assembly = ModuleWeaverHelper.WeaveAndLoadTestTarget(config);
            var targetType = assembly.GetType(typeof(Bix.Mixers.Fody.TestMixinTargets.InterfaceForImplicitExplicitTestingTarget).FullName);

            Assert.That(typeof(IInterfaceForImplicitExplicitTesting).IsAssignableFrom(targetType));
            targetType.ValidateMemberCountsAre(1, 4, 0, 0, 0, 0);

            Assert.That(targetType.GetConstructor(new Type[0]) != null, "Lost existing default constructor");

            var instanceObject = Activator.CreateInstance(targetType, new object[0]);
            Assert.That(instanceObject is IInterfaceForImplicitExplicitTesting);

            Assert.That("Implicit 1".Equals(
                targetType.GetMethod("Method1", TestContent.BindingFlagsForMixedMembers).Invoke(instanceObject, new object[] { })));
            Assert.That("Independent 2".Equals(
                targetType.GetMethod("Method2", TestContent.BindingFlagsForMixedMembers).Invoke(instanceObject, new object[] { })));
            Assert.That(targetType.GetMethod("Method3", TestContent.BindingFlagsForMixedMembers) == null);

            var instance = (IInterfaceForImplicitExplicitTesting)instanceObject;

            Assert.That("Implicit 1".Equals(instance.Method1()));
            Assert.That("Explicit 2".Equals(instance.Method2()));
            Assert.That("Explicit 3".Equals(instance.Method3()));
        }
    }
}