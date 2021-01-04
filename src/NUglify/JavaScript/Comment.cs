// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license.
// See the license.txt file in the project root for more information.

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