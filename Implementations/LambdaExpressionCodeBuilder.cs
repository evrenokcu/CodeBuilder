using System.Text;

namespace CodeBuilder.Implementations;

public class LambdaExpressionCodeBuilder : CodeBuilder<ILambdaExpressionBuilder, LambdaExpressionCodeBuilder>,
    ILambdaExpressionBuilder
{
    private ICodeBuilderBase Value { get; set; }
    private ICodeBlockBuilder CodeBlock { get; set; }
    private List<string>? LambdaParameters { get; set; } = new();
    private bool isAsync = false;

    protected readonly IList<ICodeLineBuilder> Parameters = new List<ICodeLineBuilder>();
    private readonly bool _isStatement;

    public LambdaExpressionCodeBuilder(ICodeBuilderBase parent, string name, bool isStatement = true) : base(parent,
        name)
    {
        _isStatement = isStatement;
    }

    public LambdaExpressionCodeBuilder MapsTo<T>(Func<LambdaExpressionCodeBuilder, T> factory)
        where T : ICodeBuilderBase
    {
        var valueCodeBuilder = factory.Invoke(this);
        Value = valueCodeBuilder;
        return this;
    }

    public LambdaExpressionCodeBuilder MapsToBlock(Action<CodeBlockCodeBuilder> action, bool isStatement = true)
    {
        var codeBlock = new CodeBlockCodeBuilder(this, string.Empty, isStatement);
        CodeBlock = codeBlock;
        action.Invoke(codeBlock);
        return this;
    }

    protected override void DoBuild(StringBuilder sb, int indent)
    {
        if (null != Value)
            sb.AppendLine(ClassBuilder.CreateLambdaStatement(Value.BuildLine(indent), _isStatement, LambdaParameters), indent);
        else
        {
            if (isAsync) sb.AppendLine("async ()=>{", indent);
            else sb.AppendLine("()=>{", indent);

            CodeBlock?.Build(sb, indent + 1);
            sb.AppendLine("}", indent);
        }
    }

    protected override LambdaExpressionCodeBuilder GetBuilder()
    {
        return this;
    }
    public LambdaExpressionCodeBuilder AddLambdaParameter(string parameter)
    {
        LambdaParameters.Add(parameter);
        return this;
    }

    public LambdaExpressionCodeBuilder Async()
    {
        isAsync = true;
        return this;
    }
}