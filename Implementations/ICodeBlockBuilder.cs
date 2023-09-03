namespace CodeBuilder.Implementations;

public interface ICodeBlockBuilder : ICodeBuilder
{
    public AssignmentCodeBuilder AddAssignment(string variableName, string stringValue = "");
    public ReturnCodeBuilder AddReturn(string returnVariable = "ReturnVar");
    public FunctionCallCodeBuilder AddFunctionCall(string name);
    public ConstructorCallCodeBuilder AddConstructorCall(string name);
    public CodeBlockCodeBuilder MapsTo();
}