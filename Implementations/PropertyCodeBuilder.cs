using System.Text;

namespace CodeBuilder.Implementations;

public class PropertyCodeBuilder : CodeBuilder<IPropertyBuilder, PropertyCodeBuilder>, IPropertyBuilder

{
    public enum PropertySetAccessModifier
    {
        NoSet,
        Private,
        Init,
        Public,
        DefaultSet
    }

    private string Type { get; }
    private string Value { get; }
    private ICodeLineBuilder BuilderValue { get; set; }
    private bool IsConstant { get; set; }
    private bool IsStatic { get; set; }
    private bool IsReadOnly { get; set; }
    private bool IsNullable { get; set; }
    private bool IsOverride { get; set; }
    private AccessModifier AccessModifier { get; set; } = AccessModifier.Private;
    private PropertySetAccessModifier SetAccessModifier { get; set; } = PropertySetAccessModifier.NoSet;
    private IList<ICodeLineBuilder> Attributes { get; } = new List<ICodeLineBuilder>();

    public PropertyCodeBuilder(ICodeBuilder parent, string name, string type, string value) : base(parent, name)
    {
        Type = type;
        Value = value;
    }

    public AttributeCodeBuilder AddAttribute(string name)
    {
        var builder = new AttributeCodeBuilder(this, name);
        Attributes.Add(builder);
        return builder;
    }

    public PropertyCodeBuilder With(Action<PropertyCodeBuilder> action)
    {
        action(this);
        return this;
    }

    protected override void DoBuild(StringBuilder sb, int indent)
        => sb
            .BuildLines(Attributes.ToList(), indent)
            .AppendLine(
                ClassBuilder.CreateProperty(Name, Type, BuilderValue?.BuildLine(indent) ?? Value, AccessModifier,
                    SetAccessModifier, IsConstant, IsStatic, isReadOnly: IsReadOnly, IsNullable, IsOverride), indent);

    public PropertyCodeBuilder Constant()
    {
        IsConstant = true;
        return this;
    }
    public PropertyCodeBuilder Override()
    {
        IsOverride = true;
        return this;
    }

    public PropertyCodeBuilder ReadOnly()
    {
        IsReadOnly = true;
        return this;
    }

    public PropertyCodeBuilder Modifier(AccessModifier modifier)
    {
        AccessModifier = modifier;
        return this;
    }

    public PropertyCodeBuilder Static()
    {
        IsStatic = true;
        return this;
    }

    public PropertyCodeBuilder PublicSetModifier()
    {
        SetAccessModifier = PropertySetAccessModifier.Public;
        return this;
    }

    public PropertyCodeBuilder SetModifier()
    {
        SetAccessModifier = PropertySetAccessModifier.Private;
        return this;
    }

    public T WithValue<T>(Func<ICodeBuilderBase, T> factory) where T : ICodeLineBuilder
    {
        var valueCodeBuilder = factory.Invoke(this);
        BuilderValue = valueCodeBuilder;
        return valueCodeBuilder;
    }

    protected override PropertyCodeBuilder GetBuilder()
    {
        return this;
    }

    public PropertyCodeBuilder Nullable()
    {
        IsNullable = true;
        return this;
    }
}