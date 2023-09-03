namespace CodeBuilder.Implementations;

public interface IPropertyBuilder : ICodeBuilder
{
    public AttributeCodeBuilder AddAttribute(string name);
    public PropertyCodeBuilder Constant();
    public PropertyCodeBuilder Modifier(AccessModifier modifier);
    public PropertyCodeBuilder Static();
    public PropertyCodeBuilder PublicSetModifier();
}