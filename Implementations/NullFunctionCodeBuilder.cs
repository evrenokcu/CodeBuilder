using System.Text;

namespace CodeBuilder.Implementations;

public class NullFunctionCodeBuilder : FunctionCodeBuilder
{
    public NullFunctionCodeBuilder(ICodeBuilder parent, string name) : base(parent, name)
    {
    }
    
    protected override void DoBuild(StringBuilder sb, int indent)
    {
    }
}