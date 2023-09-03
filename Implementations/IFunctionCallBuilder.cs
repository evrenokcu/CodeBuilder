namespace CodeBuilder.Implementations;

public interface IFunctionCallBuilder : ICodeLineBuilder
{
    public FunctionCallParameterCodeBuilder AddParameter(string variable);
    public FunctionCallCodeBuilder AddParameters(params string[] parameters);
}