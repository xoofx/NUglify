namespace NUglify.JavaScript
{
	public enum JsComment
	{
		/// <summary>
		/// Don't output any comments
		/// </summary>
		None = 0,

		/// <summary>
		/// Only output important comments (//! or /*!)
		/// </summary>
		Important = 1,

		/// <summary>
		/// Output all comments
		/// </summary>
		All = 2
	}
}