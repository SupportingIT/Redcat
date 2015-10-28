// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection.Tests.Fakes;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    public class TypeWithParameterizedAndNullaryConstructor
    {
        public TypeWithParameterizedAndNullaryConstructor()
            : this(new FakeService())
        {

        }

        public TypeWithParameterizedAndNullaryConstructor(IFakeService fakeService)
        {
        }
    }
}
