using FluentAssertions;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Customization;
using Qowaiv.Identifiers;
using Qowaiv.Internals;
using System.Collections.Generic;

namespace TypeHelper_specs;

public class Is_no_ID_Behavior
{
    [Test]
    public void Misses_interface() => TypeHelper.IdBehavior(typeof(object)).Should().BeNull();

    [Test]
    public void No_empty_ctor() => TypeHelper.IdBehavior(typeof(BehaviorWithoutCtor)).Should().BeNull();
}

public class Is_no_SVO_Behavior
{
    [Test]
    public void Misses_base_type() => TypeHelper.SvoBehavior(typeof(object)).Should().BeNull();

    [Test]
    public void No_empty_ctor() => TypeHelper.SvoBehavior(typeof(SvoBehaviorWithoutCtor)).Should().BeNull();
}


public class Is_ID_Behavior
{
    [Test]
    public void With_interface_and_ctor() 
        => TypeHelper.IdBehavior(typeof(SomeBehavior)).Should().Be(typeof(Id<SomeBehavior>));
}

public class Is_SVO_dBehavior
{
    [Test]
    public void With_base_type_and_ctor()
        => TypeHelper.SvoBehavior(typeof(SomeSvoBehavior)).Should().Be(typeof(Svo<SomeSvoBehavior>));

    [Test]
    public void With_ancestor_type_and_ctor()
        => TypeHelper.SvoBehavior(typeof(SomeInheritedSvoBehavior)).Should().Be(typeof(Svo<SomeInheritedSvoBehavior>));
}

public class CandidateTypes
{
    [Test]
    public void GetCandidateTypes_skips_unsupported_converts_ids()
    {
        var types = new[]
        { 
            typeof(object), 
            typeof(Uuid),
            typeof(EmailAddress),
            typeof(SomeBehavior),
            typeof(SomeSvoBehavior),
            typeof(List<int>),
            typeof(List<>) 
        };

        TypeHelper.GetCandidateTypes(types)
            .Should().BeEquivalentTo(new[]
            {
                typeof(object),
                typeof(Uuid),
                typeof(EmailAddress),
                typeof(Id<SomeBehavior>),
                typeof(Svo<SomeSvoBehavior>),
            });
    }
}

public sealed class SomeBehavior : UuidBehavior { }

public class SomeSvoBehavior : SvoBehavior { }

public sealed class SomeInheritedSvoBehavior : SomeSvoBehavior { }

#pragma warning disable S3453 // Classes should not have only "private" constructors
// The behavior we want to test.

public sealed class BehaviorWithoutCtor : UuidBehavior
{
    private BehaviorWithoutCtor() { }
}

public sealed class SvoBehaviorWithoutCtor : SvoBehavior
{
    private SvoBehaviorWithoutCtor() { }
}
