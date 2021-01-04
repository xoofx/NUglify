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
		public override void Visit(BlockStatement node)
		{
			if (node != null)
			{
				// iterate backwards as we are likely to remove nodes
				for (var ndx = node.Count - 1; ndx >= 0; --ndx)
					node[ndx].Accept(this);
			}
		}


		public override void Visit(Syntax.Comment node)
		{
			if (parser.Settings.CommentMode != JsComment.None && (parser.Settings.CommentMode != JsComment.Important || node.IsImportant))
				return;

			node.Parent.ReplaceChild(node, null);
			node.Parent = null;
		}
	}
}