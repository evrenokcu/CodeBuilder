namespace CodeBuilder.Implementations;

public class AssignmentCodeBuilder : CodeLineBuilder<IAssignmentBuilder, AssignmentCodeBuilder>, IAssignmentBuilder
{
    private ICodeBuilderBase Value { get; set; }
    private string StringValue { get; }
    private bool IsVarAssignment { get; set; }
    private bool IsThisAssignment { get; set; }
    private bool IsCommaInsteadOfSemicolon { get; }
    private bool IsStatement { get; }
    private string Type { get; set; }
    private bool IsThrowExceptionIfNull { get; set; }

    private bool IsEventAssignment { get; set; }
    private string TypeCastClass { get; set; }

    public AssignmentCodeBuilder(
        ICodeBuilderBase parent,
        string variableName,
        string value,
        bool useCommaInsteadOfSemicolon,
        bool isStatement)
        : base(parent, variableName)
    {
        StringValue = string.Empty;
        StringValue = value;
        IsCommaInsteadOfSemicolon = useCommaInsteadOfSemicolon;
        IsStatement = isStatement;
    }

    public AssignmentCodeBuilder VarAssignment()
    {
        IsVarAssignment = true;
        return this;
    }

    public AssignmentCodeBuilder ThisAssignment()
    {
        IsThisAssignment = true;
        return this;
    }

    public override string BuildLine(int indent)
    {
        return ClassBuilder.CreateAssignment(string.Empty,
            Name,
            Value?.BuildLine(indent) ?? StringValue,
            isVarAssignment: IsVarAssignment,
            isCommaInsteadOfSemicolon: IsCommaInsteadOfSemicolon,
            isStatement: IsStatement,
            classType: Type,
            operation: IsEventAssignment
                ? "+="
                : "=",
            isThisAssignment: IsThisAssignment,
            isThrowExceptionIfNull: IsThrowExceptionIfNull,
            typeCastClass: TypeCastClass);
    }

    public T AssignTo<T>(Func<ICodeBuilderBase, T> factory) where T : ICodeBuilderBase
    {
        var valueCodeBuilder = factory.Invoke(this);
        Value = valueCodeBuilder;
        return valueCodeBuilder;
    }

    public AssignmentCodeBuilder VariableType(string variableType)
    {
        Type = variableType;
        return this;
    }

    public AssignmentCodeBuilder EventAssignment()
    {
        IsEventAssignment = true;
        return this;
    }

    public AssignmentCodeBuilder ThrowExceptionIfNull()
    {
        IsThrowExceptionIfNull = true;
        return this;
    }

    public AssignmentCodeBuilder TypeCast(string className)
    {
        TypeCastClass = className;
        return this;
    }

    protected override AssignmentCodeBuilder GetBuilder()
    {
        return this;
    }
}