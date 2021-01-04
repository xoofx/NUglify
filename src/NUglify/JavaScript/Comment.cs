using System.Diagnostics;

namespace NUglify.JavaScript
{
	[DebuggerDisplay("{Context}")]
	public class Comment
	{
		public SourceContext Context { get; set; }
		public bool IsImportant { get; set; }
		public bool IsMultiLine { get; set; }
	}
}