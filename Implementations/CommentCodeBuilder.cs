namespace CodeBuilder.Implementations;

public class CommentCodeBuilder : CodeLineBuilder<ICommentCodeBuilder, CommentCodeBuilder>, ICommentCodeBuilder
{
    public CommentCodeBuilder(ICodeBuilderBase parent, string comment) : base(parent, comment)
    {
    }

    public override string BuildLine(int indent) =>
        ClassBuilder.CreateComment(Name);

    protected override CommentCodeBuilder GetBuilder()
    {
        return this;
    }
}