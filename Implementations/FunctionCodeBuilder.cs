using System.Text;

namespace CodeBuilder.Implementations;

public class FunctionCodeBuilder : CodeBuilder<IFunctionBuilder, FunctionCodeBuilder>, IFunctionBuilder
{
    protected ICodeLineBuilder BaseCall;
    private ICodeBuilder _codeBlock;
    protected readonly List<ICodeLineBuilder> Parameters = new();
    private bool _isStatic;
    private bool _override;
    private string _returnType = string.Empty;
    private bool _isVirtual;
    private bool _isAsync;


    private IList<ICodeLineBuilder> Attributes { get; } = new List<ICodeLineBuilder>();
    protected AccessModifier AccessModifier { get; private set; } = AccessModifier.Private;


    public FunctionCodeBuilder(ICodeBuilder parent, string name) : base(parent, name)
    {
    }

    public ParameterCodeBuilder AddParameter(string type, string parameterName)
    {
        var builder = new ParameterCodeBuilder(this, parameterName, type);
        Parameters.Add(builder);
        return builder;
    }


    public FunctionCallCodeBuilder AddBaseCall(bool isStatement = true)
    {
        var builder = new FunctionCallCodeBuilder(this, "base", isStatement);
        BaseCall = builder;
        return builder;
    }

    protected override void DoBuild(StringBuilder sb, int indent)
    {
        sb.BuildLines(Attributes.ToList(), indent);
        BuildHeader(sb, indent);
        _codeBlock?.Build(sb, indent);
    }

    protected virtual void BuildHeader(StringBuilder sb, int indent)
    {
        sb.AppendLine(
            ClassBuilder.CreateFunctionHeader(
                Name,
                Parameters.Select(it => it.BuildLine(indent)),
                AccessModifier,
                baseCall: BaseCall?.BuildLine(indent)!,
                returnType: _returnType,
                isStatic: _isStatic,
                @override: _override,
                isStatement: false,
                isVirtual: _isVirtual,
                isAsync: _isAsync
            ),
            indent);
    }

    public CodeBlockCodeBuilder AddImplementation()
    {
        var builder = new CodeBlockCodeBuilder(this, "CodeBlock", isInitializationBlock: false);
        _codeBlock = builder;
        return builder;
    }

    public FunctionCodeBuilder Modifier(AccessModifier modifier)
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

    public FunctionCodeBuilder Static()
    {
        _isStatic = true;
        return this;
    }

    public FunctionCodeBuilder ReturnType(string returnType)
    {
        _returnType = returnType;
        return this;
    }

    public FunctionCodeBuilder Override()
    {
        _override = true;
        return this;
    }

    public FunctionCodeBuilder Async()
    {
        _isAsync = true;
        return this;
    }

    public FunctionCodeBuilder Virtual()
    {
        _isVirtual = true;
        return this;
    }

    protected override FunctionCodeBuilder GetBuilder()
    {
        return this;
    }
}