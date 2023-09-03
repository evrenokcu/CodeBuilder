namespace CodeBuilder.Implementations;

public class FunctionCallCodeBuilder : CodeLineBuilder<IFunctionCallBuilder, FunctionCallCodeBuilder>,
    IFunctionCallBuilder
{
    private readonly IList<ICodeLineBuilder> _parameters = new List<ICodeLineBuilder>();
    private readonly bool _isStatement;
    private string _instanceOf = string.Empty;
    private string _genericType = string.Empty;
    private bool _isAwait;
    private string _prefix = string.Empty;

    public FunctionCallCodeBuilder(ICodeBuilderBase parent, string name, bool isStatement = true) : base(parent, name)
    {
        _isStatement = isStatement;
    }

    public FunctionCallParameterCodeBuilder AddParameter(string variable)
    {
        var builder = new FunctionCallParameterCodeBuilder(this, variable);
        _parameters.Add(builder);
        return builder;
    }

    public FunctionCallCodeBuilder AddParameters(params string[] parameters)
    {
        parameters.ToList().ForEach(it => AddParameter(it));
        return this;
    }
    public FunctionCallCodeBuilder AddParameters(IEnumerable<string> parameters)
    {
        parameters.ToList().ForEach(it => AddParameter(it));
        return this;
    }

    public FunctionCallCodeBuilder Generic(string genericType)
    {
        _genericType = genericType;
        return this;
    }

    public override string BuildLine(int indent)
    {
        return ClassBuilder.CreateFunctionCallStatement(_prefix, Name,
            _parameters.Select(it => it.BuildLine(indent)), isStatement: _isStatement, instanceOfClass: _instanceOf,
            genericType: _genericType, isAwait: _isAwait);
    }

    public FunctionCallCodeBuilder InstanceOfClass(string className)
    {
        _instanceOf = className;
        return this;
    }

    public FunctionCallCodeBuilder Await()
    {
        _isAwait = true;
        return this;
    }

    protected override FunctionCallCodeBuilder GetBuilder()
    {
        return this;
    }

    public FunctionCallCodeBuilder PrefixAsyncIt()
    {
        _prefix = "async it=> await ";
        return this;
    }
}