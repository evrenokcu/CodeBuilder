namespace CodeBuilder.Implementations;

public interface IClassBuilder : ICodeBuilder
{
    public FunctionCodeBuilder AddConstructor();
    public ClassCodeBuilder AddBaseType(string type);
    public FunctionCodeBuilder AddFunction(string name);
    public AttributeCodeBuilder AddAttribute(string attributeName);
    public ClassCodeBuilder AddField(string name, string type, string value = "");
    public PropertyCodeBuilder AddProperty(string name, string type, string value = "");
    public ClassCodeBuilder AddFields<T>(IList<T> list, Func<ClassCodeBuilder, T, FieldCodeBuilder> converter);
    public ClassCodeBuilder AddProperties<T>(IList<T> list, Func<ClassCodeBuilder, T, PropertyCodeBuilder> converter);
    public ClassCodeBuilder Partial();
    public ClassCodeBuilder Static();
}