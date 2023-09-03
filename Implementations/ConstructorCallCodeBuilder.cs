using System.Text;

namespace CodeBuilder.Implementations;

public class ConstructorCallCodeBuilder : CodeBuilder<IConstructorCallBuilder, ConstructorCallCodeBuilder>,
    IConstructorCallBuilder
{
    private ICodeBuilder _initialization;

    private readonly IList<ICodeLineBuilder> _parameters = new List<ICodeLineBuilder>();
    private readonly bool _isStatement;
    private bool _throw;

    public ConstructorCallCodeBuilder(ICodeBuilderBase parent, string name, bool isStatement = true) : base(parent,
        name)
    {
        _isStatement = isStatement;
    }

    public FunctionCallParameterCodeBuilder AddParameter(string variable)
    {
        var builder = new FunctionCallParameterCodeBuilder(this, variable);
        _parameters.Add(builder);
        return builder;
    }

    public ConstructorCallCodeBuilder AddParameters(params string[] parameters)
    {
        parameters.ToList().ForEach(it => AddParameter(it));
        return this;
    }

    public ConstructorCallCodeBuilder AddParameters(IEnumerable<string> parameters)
    {
        parameters.ToList().ForEach(it => AddParameter(it));
        return this;
    }

    public CodeBlockCodeBuilder AddInitialization()
    {
        var builder = new CodeBlockCodeBuilder(this, "Initialization", isInitializationBlock: true);
        _initialization = builder;
        return builder;
    }

    public ConstructorCallCodeBuilder Throw()
    {
        _throw = true;
        return this;
    }


    protected override void DoBuild(StringBuilder sb, int indent)
    {
        var prefix = "new ";
        if (_throw) prefix = $"throw {prefix}";
        var functionCall = ClassBuilder.CreateFunctionCallStatement(prefix, Name,
            _parameters.Select(it => it.BuildLine(indent)), isStatement: _isStatement,
            isInitialization: _initialization != null);
        if (_isStatement)
            sb.AppendLine(functionCall, indent);
        else
            sb.Append(functionCall);
        _initialization?.Build(sb, indent + 1);
        if (_isStatement && _initialization != null)
        {
            sb.AppendLine(";", indent);
        }
    }

    protected override ConstructorCallCodeBuilder GetBuilder()
    {
        return this;
    }
}