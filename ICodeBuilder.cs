using System.Text;

namespace CodeBuilder;

public interface ICodeBuilder : ICodeLineBuilder
{
    StringBuilder Build(StringBuilder sb, int indent);
}