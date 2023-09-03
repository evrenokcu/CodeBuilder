namespace CodeBuilder.Implementations;

public class AttributeCodeBuilder : CodeLineBuilder<IAttributeBuilder, AttributeCodeBuilder>, IAttributeBuilder
{
    private ICodeBuilderBase Value { get; set; }
    private IList<string> AttributeValues { get; } = new List<string>();

    public AttributeCodeBuilder(ICodeBuilderBase parent, string name) : base(parent, name)
    {
    }

    public AttributeCodeBuilder AddValues(params string[] values)
    {
        foreach (var item in values.ToList())
        {
            AttributeValues.Add(item);
        }

        return this;
    }

    public AttributeCodeBuilder AttributeWith<T>(Func<AttributeCodeBuilder, T> factory) where T : ICodeBuilderBase
    {
        var valueCodeBuilder = factory.Invoke(this);
        Value = valueCodeBuilder;
        return this;
    }

    public override string BuildLine(int indent) =>
        Value == null
            ? ClassBuilder.CreateAttribute(Name, AttributeValues)
            : ClassBuilder.CreateAttributeWithValue(Value.BuildLine(indent));

    protected override AttributeCodeBuilder GetBuilder()
    {
        return this;
    }
}