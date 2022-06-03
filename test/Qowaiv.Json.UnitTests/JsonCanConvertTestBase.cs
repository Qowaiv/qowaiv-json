using NUnit.Framework;
using Qowaiv.Json.UnitTests.Models;
using System;

namespace Qowaiv.Json.UnitTests;

/// <remarks>
/// This abstract base class should help to guarantee that different
/// implementations have the same behaviour.
/// </remarks>
public abstract class JsonCanConvertTestBase
{
    [Test]
    public void CanConvert_Null_False()
    {
        Assert.IsFalse(CanConvert(null));
    }

    [Test]
    public void CanConvert_Object_False()
    {
        Assert.IsFalse(CanConvert(typeof(object)));
    }

    [Test]
    public void CanConvert_Int32_False()
    {
        Assert.IsFalse(CanConvert(typeof(int)));
    }

    [Test]
    public void CanConvert_SvoWithFromJsonBoolOnly_False()
    {
        Assert.IsFalse(CanConvert(typeof(SvoWithFromJsonBoolOnly)));
    }

    [Test]
    public void CanConvert_SvoWithFromJsonStringOnly_True()
    {
        Assert.IsTrue(CanConvert(typeof(SvoWithFromJsonStringOnly)));
    }

    [Test]
    public void CanConvert_SvoWithFromJson_True()
    {
        Assert.IsTrue(CanConvert(typeof(SvoWithFromJson)));
    }

    [Test]
    public void CanConvert_NullableSvoWithFromJson_True()
    {
        Assert.IsTrue(CanConvert(typeof(SvoWithFromJson?)));
    }

    [Test]
    public void CanConvert_SvoWithFromJsonClass_True()
    {
        Assert.IsTrue(CanConvert(typeof(SvoWithFromJsonClass)));
    }

    protected abstract bool CanConvert(Type type);
}
