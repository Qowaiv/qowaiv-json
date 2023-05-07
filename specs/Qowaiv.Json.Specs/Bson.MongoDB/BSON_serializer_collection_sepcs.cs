using FluentAssertions;
using Qowaiv.Bson.MongoDB;
using Qowaiv.Identifiers;
using Specs.Models;

namespace BSON.MongoDB.BSON_serializer_collection_sepcs;

public class Adds
{
    [Test]
    public void exportable_public_types_from_assemlby_by_default() 
        => new QowaivBsonSerializerCollection
        {
            typeof(Qowaiv.Date).Assembly,
        }
        .Should().HaveCount(29);

    [Test]
    public void all_types_from_assemlby_when_specified()
    {
        var collection = new QowaivBsonSerializerCollection();
        collection.Add(GetType().Assembly, exportableTypesOnly: false);
        collection.Types.Where(t => !t.IsPublic).Should().NotBeEmpty();
        collection.Types.Where(t => t.IsPublic).Should().NotBeEmpty();
    }

    [Test]
    public void SVO_with_FromJson_string_only()
        => new QowaivBsonSerializerCollection
        {
            typeof(SvoWithFromJsonStringOnly)
        }
        .Should().BeEquivalentTo(new[] { new QowaivBsonSerializer<SvoWithFromJsonStringOnly>() });

    [Test]
    public void SVO_with_FromJson()
        => new QowaivBsonSerializerCollection
        {
            typeof(SvoWithFromJson)
        }
        .Should().BeEquivalentTo(new[] { new QowaivBsonSerializer<SvoWithFromJson>() });

    [Test]
    public void Nullable_SVO_with_FromJson_as_not_nullable()
        => new QowaivBsonSerializerCollection
        {
            typeof(SvoWithFromJson?)
        }
        .Should().BeEquivalentTo(new[] { new QowaivBsonSerializer<SvoWithFromJson>() });

    [Test]
    public void SVO_class_with_FromJson()
        => new QowaivBsonSerializerCollection
        {
            typeof(SvoWithFromJsonClass)
        }
        .Should().BeEquivalentTo(new[] { new QowaivBsonSerializer<SvoWithFromJsonClass>() });

    [Test]
    public void Qowaiv_ID_via_behavior()
        => new QowaivBsonSerializerCollection
        {
            typeof(ForModel),
        }
        .Should().BeEquivalentTo(new[] { new QowaivBsonSerializer<Id<ForModel>>() });

    [Test]
    public void Qowaiv_generic_SVO_via_behavior()
        => new QowaivBsonSerializerCollection
        {
            typeof(ForModel),
        }
        .Should().BeEquivalentTo(new[] { new QowaivBsonSerializer<Id<ForModel>>() });

    [Test]
    public void returns_true()
       => new QowaivBsonSerializerCollection()
       .Add(typeof(SvoWithFromJson)).Should().BeTrue();
}

public class Does_not_add
{
    [Test]
    public void SVO_without_FromJson()
        => new QowaivBsonSerializerCollection
        {
            typeof(int)
        }
        .Should().BeEmpty();

    [Test]
    public void SVO_with_FromJson_bool_only()
        => new QowaivBsonSerializerCollection
        {
            typeof(SvoWithFromJsonBoolOnly)
        }
        .Should().BeEmpty();

    [Test]
    public void DTO()
        => new QowaivBsonSerializerCollection
        {
            typeof(DtoClass)
        }
        .Should().BeEmpty();

    [Test]
    public void Adding_type_already_added()
    {
        var colletion = new QowaivBsonSerializerCollection
        {
            typeof(SvoWithFromJson)
        };
        colletion.Add(typeof(SvoWithFromJson)).Should().BeFalse();

        colletion.Should().BeEquivalentTo(new[] { new QowaivBsonSerializer<SvoWithFromJson>() });
    }
}

public class Clears
{
    [Test]
    public void serializers_in_collection()
    {
        var collection = new QowaivBsonSerializerCollection
        {
            typeof(Qowaiv.Date).Assembly,
        };
        collection.Should().NotBeEmpty();
        collection.Clear();
        collection.Should().BeEmpty();
    }
}
