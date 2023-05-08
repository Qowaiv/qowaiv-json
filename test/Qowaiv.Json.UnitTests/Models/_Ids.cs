#pragma warning disable S2094 // Classes should not be empty
#pragma warning disable S3453 // Classes should not have only "private" constructors

using Qowaiv.Identifiers;

namespace Specs.Models;

public sealed class SomeIdBehavior : UuidBehavior { }

public sealed class IdBehaviorWithoutCtor : UuidBehavior
{
    private IdBehaviorWithoutCtor() { }
}

public abstract class AbstractIdBehavior : GuidBehavior { }
