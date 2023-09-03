using System.Text;

namespace CodeBuilder.Implementations;

public class ClassCodeBuilder : CodeBuilder<IClassBuilder, ClassCodeBuilder>, IClassBuilder
{
    private bool IsPartial { get; set; }
    private bool IsStatic { get; set; }
    private List<string> BaseTypes { get; } = new();
    private IList<ICodeLineBuilder> Fields { get; } = new List<ICodeLineBuilder>();
    private IList<ICodeBuilder> Properties { get; } = new List<ICodeBuilder>();
    private IList<ICodeBuilder> Functions { get; } = new List<ICodeBuilder>();
    private IList<ICodeLineBuilder> Attributes { get; } = new List<ICodeLineBuilder>();
    private IList<ICodeBuilder> SubClasses { get; } = new List<ICodeBuilder>();

    //todo: remove this
    private string TempCode { get; set; } = string.Empty;

    private ClassCodeBuilder(ICodeBuilder parent, string name) : base(parent, name)
    {
    }

    public ClassCodeBuilder(string name) : base(NullBuilder.Instance, name)
    {
    }

    public ClassCodeBuilder AddBaseType(string type)
    {
        if (!string.IsNullOrWhiteSpace(type))
        {
            BaseTypes.Add(type);
        }

        return this;
    }

    public ClassCodeBuilder AddBaseTypes(params string[] types)
    {
        types.ToList().ForEach(t => AddBaseType(t));
        return this;
    }

    public ClassCodeBuilder AddBaseTypes(IEnumerable<string> types)
    {
        types.ToList().ForEach(it => AddBaseType(it));
        return this;
    }

    public FunctionCodeBuilder AddFunction(string name)
    {
        var builder = new FunctionCodeBuilder(this, name);
        SubBuilders.Add(builder);
        return builder;
    }

    public FunctionCodeBuilder AddFunction(bool condition, string name)
    {
        if (!condition) return new NullFunctionCodeBuilder(this, name);
        return AddFunction(name);
    }

    public AttributeCodeBuilder AddAttribute(string attributeName)
    {
        var builder = new AttributeCodeBuilder(this, attributeName);
        Attributes.Add(builder);
        return builder;
    }

    public ClassCodeBuilder AddField(string name, string type, string value = "")
    {
        var builder = new FieldCodeBuilder(this, name, type, value);
        Fields.Add(builder);
        return this;
    }

    public PropertyCodeBuilder AddProperty(string name, string type, string value = "")
    {
        var builder = new PropertyCodeBuilder(this, name, type, value);
        Properties.Add(builder);
        return builder;
    }

    public ClassCodeBuilder AddFields<T>(IList<T> list, Func<ClassCodeBuilder, T, FieldCodeBuilder> converter)
    {
        list.ToList().ForEach(listItem => Fields.Add(converter.Invoke(this, listItem)));
        return this;
    }

    public ClassCodeBuilder AddProperties<T>(IList<T> list, Func<ClassCodeBuilder, T, PropertyCodeBuilder> converter)
    {
        list.ToList().ForEach(listItem => converter.Invoke(this, listItem));
        return this;
    }

    public ClassCodeBuilder WithTempCode(string s)
    {
        TempCode = s;
        return this;
    }

    public FunctionCodeBuilder AddConstructor()
    {
        var builder = new ConstructorFunctionCodeBuilder(this, Name);
        Functions.Add(builder);
        return builder;
    }

    public ClassCodeBuilder AddClass(string name)
    {
        var builder = new ClassCodeBuilder(this, name);
        SubClasses.Add(builder);
        return builder;
    }

    public ClassCodeBuilder Partial()
    {
        IsPartial = true;
        return this;
    }

    public ClassCodeBuilder Static()
    {
        IsStatic = true;
        return this;
    }

    protected override void DoBuild(StringBuilder sb, int indent)
    {
        sb.BuildLines(Attributes, indent)
            .AppendLine(ClassBuilder.CreatePublicClassHeader(Name, IsPartial, IsStatic, BaseTypes.JoinStrings(", ")),
                indent)
            .AppendLine("{", indent)
            .BuildLines(Fields, indent + 1)
            .BuildCodes(Properties, indent + 1)
            .BuildCodes(Functions, indent + 1)
            .BuildCodes(SubClasses, indent + 1);
    }

    protected override void AfterSubBuild(StringBuilder sb, int indent)
    {
        sb.AppendLine(TempCode);
        sb.AppendLine("}", indent);
    }

    protected override ClassCodeBuilder GetBuilder()
    {
        return this;
    }
}