using System.Text;

namespace CodeBuilder.Implementations;

public sealed class InterfaceFunctionCodeBuilder : CodeBuilder<IInterfaceFunctionBuilder, InterfaceFunctionCodeBuilder>,
    IInterfaceFunctionBuilder
{
    private ICodeLineBuilder _baseCall;
    private ICodeBuilder _codeBlock;
    private readonly List<ICodeLineBuilder> _parameters = new();
    private bool _isStatic;
    private bool _override;
    private string _returnType = string.Empty;

    private IList<ICodeLineBuilder> Attributes { get; } = new List<ICodeLineBuilder>();
    private AccessModifier AccessModifier { get; set; } = AccessModifier.None;


    public InterfaceFunctionCodeBuilder(ICodeBuilder parent, string name) : base(parent, name)
    {
    }

    public ParameterCodeBuilder AddParameter(string type, string parameterName)
    {
        var builder = new ParameterCodeBuilder(this, parameterName, type);
        _parameters.Add(builder);
        return builder;
    }


    public FunctionCallCodeBuilder AddBaseCall(bool isStatement = true)
    {
        var builder = new FunctionCallCodeBuilder(this, "base", isStatement);
        _baseCall = builder;
        return builder;
    }

    protected override void DoBuild(StringBuilder sb, int indent)
    {
        sb.BuildLines(Attributes.ToList(), indent);
        BuildHeader(sb, indent);
        _codeBlock?.Build(sb, indent);
    }

    private void BuildHeader(StringBuilder sb, int indent)
    {
        sb.AppendLine(
            ClassBuilder.CreateFunctionHeader(
                Name,
                _parameters.Select(it => it.BuildLine(indent)),
                AccessModifier,
                baseCall: _baseCall?.BuildLine(indent)!,
                returnType: _returnType,
                isStatic: _isStatic,
                @override: _override,
                isStatement: true
            ),
            indent);
    }

    public CodeBlockCodeBuilder AddImplementation()
    {
        var builder = new CodeBlockCodeBuilder(this, "CodeBlock", isInitializationBlock: false);
        _codeBlock = builder;
        return builder;
    }

    public InterfaceFunctionCodeBuilder Modifier(AccessModifier modifier)
    {
        AccessModifier = modifier;
        return this;
    }

    public AttributeCodeBuilder AddAttribute(string name)
    {
        var builder = new AttributeCodeBuilder(this, name);
        Attributes.Add(builder);
        return builder;
    }

    public InterfaceFunctionCodeBuilder Static()
    {
        _isStatic = true;
        return this;
    }

    public InterfaceFunctionCodeBuilder ReturnType(string returnType)
    {
        _returnType = returnType;
        return this;
    }

    public InterfaceFunctionCodeBuilder Override()
    {
        _override = true;
        return this;
    }

    protected override InterfaceFunctionCodeBuilder GetBuilder()
    {
        return this;
    }
}