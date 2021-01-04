namespace NUglify.JavaScript
{
	public enum JsComment
	{
		/// <summary>
		/// Don't output any comments
		/// </summary>
		PreserveNone = 0,

		/// <summary>
		/// Only output important comments (//! or /*!)
		/// </summary>
		PreserveImportant = 1,

		/// <summary>
		/// Output all comments
		/// </summary>
		PreserveAll = 2
	}
}