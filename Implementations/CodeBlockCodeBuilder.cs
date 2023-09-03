using System.Text;

namespace CodeBuilder.Implementations;

public class CodeBlockCodeBuilder : CodeBuilder<ICodeBlockBuilder, CodeBlockCodeBuilder>, ICodeBlockBuilder
{
    private bool _wrapCurlyBracket = true;
    private bool _mapsToImplementation;
    private readonly bool _isInitializationBlock;

    private readonly List<ICodeLineBuilder> _subLines = new();

    public CodeBlockCodeBuilder(ICodeBuilder parent, string name, bool isInitializationBlock) : base(parent, name)
    {
        _isInitializationBlock = isInitializationBlock;
    }

    public AssignmentCodeBuilder AddAssignment(string variableName, string stringValue = "")
    {
        var builder = new AssignmentCodeBuilder(this, variableName, stringValue,
            useCommaInsteadOfSemicolon: _isInitializationBlock, isStatement: !_isInitializationBlock);
        _subLines.Add(builder);
        return builder;
    }

    public ReturnCodeBuilder AddReturn(string returnVariable = "returnVar")
    {
        var builder = new ReturnCodeBuilder(this, returnVariable);
        _subLines.Add(builder);
        return builder;
    }

    public CodeBlockCodeBuilder MapsTo()
    {
        _mapsToImplementation = true;
        _wrapCurlyBracket = false;
        return this;
    }

    public FunctionCallCodeBuilder AddFunctionCall(string name)
    {
        var builder = new FunctionCallCodeBuilder(this, name);
        _subLines.Add(builder);
        return builder;
    }

    public ConstructorCallCodeBuilder AddConstructorCall(string name)
    {
        var builder = new ConstructorCallCodeBuilder(this, name);
        _subLines.Add(builder);
        return builder;
    }

    public CodeBlockCodeBuilder AddParameters(params string[] parameters)
    {
        if (!_isInitializationBlock)
            throw new InvalidOperationException("Parameters are only valid for initialization block");
        parameters?.ToList().ForEach(it => _subLines.Add(new ValueCodeBuilder(this, it)));
        return this;
    }
    public CodeBlockCodeBuilder AddArgumentNullCheck(string argument)
    {
        var builder = new ArgumentNullCheckCodeBuilder(this, argument);
        _subLines.Add(builder);
        return this;
    }

    public CodeBlockCodeBuilder AddComment(string comment)
    {
        var builder = new CommentCodeBuilder(this, comment);
        _subLines.Add(builder);
        return this;
    }

    protected override void DoBuild(StringBuilder sb, int indent)
    {
    }

    protected override void AfterSubBuild(StringBuilder sb, int indent)
    {
    }

    public IfCodeBuilder AddIfStatement(string name)
    {
        var builder = new IfCodeBuilder(this, name);
        _subLines.Add(builder);
        return builder;
    }

    protected override CodeBlockCodeBuilder GetBuilder()
    {
        return this;
    }

    protected override StringBuilder BuildTemplate(StringBuilder sb, int indent = 0)
    {
        if (_wrapCurlyBracket)
        {
            if (_isInitializationBlock) sb.AppendLine();
            sb.AppendLine("{", indent);
        }

        if (_mapsToImplementation)
            sb.Append(" => ", indent);
        var itemCount = -1;
        if (_isInitializationBlock) itemCount = _subLines.Count;

        var index = 0;
        _subLines.ForEach(it =>
        {
            if (it is ICodeBuilder codeBlock) codeBlock.Build(sb, indent + 1);
            else if (it is ICodeLineBuilder codeLine)
                sb.AppendLine(codeLine.BuildLine(indent) + ",".IfTrue(_isInitializationBlock && index != itemCount - 1),
                    indent + 1);
            index++;
        });

        DoBuild(sb, indent);
        AfterSubBuild(sb, indent);
        if (_wrapCurlyBracket) sb.AppendLine("}", indent);
        return sb;
    }

    public CodeBlockCodeBuilder ReturnOkResultOfFunction(Action<FunctionCallCodeBuilder> functionAction)
    {
        var returnStatement = new ReturnCodeBuilder(this, string.Empty);
        var builder = new FunctionCallCodeBuilder(returnStatement, "Ok", false).InstanceOfClass("Result");
        returnStatement.ReturnWith(_ => builder);
        functionAction.Invoke(builder);
        _subLines.Add(returnStatement);
        return this;
    }

    public CodeBlockCodeBuilder Return()
    {
        return AddCode("return;");
    }
    public CodeBlockCodeBuilder Return(string returnVariableName)
    {
        return AddCode($"return {returnVariableName};");
    }

    public CodeBlockCodeBuilder AddCode(string codeLine)
    {
        _subLines.Add(new ValueCodeBuilder(this, codeLine));
        return this;
    }
}