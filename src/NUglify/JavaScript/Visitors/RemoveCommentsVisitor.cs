using System;
using NUglify.JavaScript.Syntax;

namespace NUglify.JavaScript.Visitors
{
	public class RemoveCommentsVisitor : TreeVisitor
	{
		readonly JSParser parser;

		RemoveCommentsVisitor(JSParser parser)
		{
			this.parser = parser;
		}

		public static void Apply(BlockStatement block, JSParser parser)
        {
            if (parser == null)
	            throw new ArgumentNullException(nameof(parser));

            if (block != null)
            {
                // create a new instance of the visitor and apply it to the block
                var visitor = new RemoveCommentsVisitor(parser);
                block.Accept(visitor);
            }
        }

		public override void Visit(StandardComment node)
		{
			if (parser.Settings.CommentMode == JsComment.All)
				return;

			node.Parent.ReplaceChild(node, null);
			node.Parent = null;
		}

		public override void Visit(ImportantComment node)
		{
			if (parser.Settings.CommentMode == JsComment.None)
				return;

			node.Parent.ReplaceChild(node, null);
			node.Parent = null;
		}
	}
}