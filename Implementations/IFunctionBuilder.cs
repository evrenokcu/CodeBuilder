namespace CodeBuilder.Implementations;

public interface IFunctionBuilder : ICodeBuilder
{
    public ParameterCodeBuilder AddParameter(string type, string parameterName);
    public AttributeCodeBuilder AddAttribute(string name);
    public CodeBlockCodeBuilder AddImplementation();
    public FunctionCallCodeBuilder AddBaseCall(bool isStatement = true);
}