using System.Text;
using CodeBuilder.Implementations.IfStatementBuilder;

namespace CodeBuilder.Implementations;

public class IfCodeBuilder : CodeBuilder<IIfBuilder, IfCodeBuilder>, IIfBuilder
{
    private ICodeBuilder _codeBlock;
    private ICodeBuilder _elseBlock;

    public IfCodeBuilder(ICodeBuilderBase parent, string name) : base(parent, name)
    {
        Condition = new RootCondition(this);
    }

    protected override void DoBuild(StringBuilder sb, int indent)
    {
        sb.AppendLine(ClassBuilder.CreateIfStatement(Condition.BuildLine(indent)), indent);
        _codeBlock.Build(sb, indent);
        if (null != _elseBlock)
        {
            sb.AppendLine("else", indent);
            _elseBlock.Build(sb, indent);
        }
    }

    public CodeBlockCodeBuilder Do()
    {
        var builder = new CodeBlockCodeBuilder(this, "IfBlock", isInitializationBlock: false);
        _codeBlock = builder;
        return builder;
    }

    public CodeBlockCodeBuilder Else()
    {
        var builder = new CodeBlockCodeBuilder(this, "ElseBlock", isInitializationBlock: false);
        _elseBlock = builder;
        return builder;
    }

    protected override IfCodeBuilder GetBuilder()
    {
        return this;
    }

    public RootCondition Condition { get; private set; }
}