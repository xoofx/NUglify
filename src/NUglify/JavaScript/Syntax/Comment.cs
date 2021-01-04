using NUglify.JavaScript.Visitors;

namespace NUglify.JavaScript.Syntax
{
    public class Comment : AstNode
    {
        // Indicates if this was a multiline comment (by syntax only, doesn't necessarily contain a \n)
	    public bool IsMultiLine { get; set; }
	    public bool IsImportant { get; set; }
        public string Value { get; set; }

        // this is for determining if a node in a block AFTER a return/break/continue should be removed. We don't want to remove an important comment, so SAY it's a declaration.
        public override bool IsDeclaration => true;

        public Comment(SourceContext context, bool isImportant, bool isMultiLine) : base(context)
        {
	        IsImportant = isImportant;
	        IsMultiLine = isMultiLine;
	        Value = Context.Code;
        }

        public override void Accept(IVisitor visitor)
        {
	        visitor?.Visit(this);
        }
    }
}
