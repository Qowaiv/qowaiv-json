using FluentAssertions;
using NUnit.Framework;
using Qowaiv;
using Qowaiv.Identifiers;
using Qowaiv.Internals;
using System.Collections.Generic;

namespace TypeHelper_specs;

public class Is_no_IdBehavior
{
    [Test]
    public void Misses_interface() => TypeHelper.IsIdBehavior(typeof(object)).Should().BeFalse();

    [Test]
    public void No_empty_ctor() => TypeHelper.IsIdBehavior(typeof(BehaviorWithoutCtor)).Should().BeFalse();

    [Test]
    public void is_abstract() => TypeHelper.IsIdBehavior(typeof(AbstractBehavior)).Should().BeFalse();
}

public class Is_IdBehavior
{
    [Test]
    public void With_interface_and_ctor() => TypeHelper.IsIdBehavior(typeof(SomeBehavior)).Should().BeTrue();

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
            typeof(AbstractBehavior),
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
            });
    }
}

public sealed class SomeBehavior : UuidBehavior { }

#pragma warning disable S3453 // Classes should not have only "private" constructors
// The behavior we want to test.
public sealed class BehaviorWithoutCtor : UuidBehavior
{
    private BehaviorWithoutCtor() { }
}

public abstract class AbstractBehavior : UuidBehavior { }
