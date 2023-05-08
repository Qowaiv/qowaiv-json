#pragma warning disable S2094 // Classes should not be empty
#pragma warning disable S3453 // Classes should not have only "private" constructors

using Qowaiv.Customization;

namespace Specs.Models;

public class SomeSvoBehavior : SvoBehavior { }

public sealed class SomeInheritedSvoBehavior : SomeSvoBehavior { }

public sealed class SvoBehaviorWithoutCtor : SvoBehavior
{
    private SvoBehaviorWithoutCtor() { }
}

public abstract class AbstractSvoBehavior : SvoBehavior { }
