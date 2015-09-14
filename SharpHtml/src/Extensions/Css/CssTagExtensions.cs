using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////

	public static partial class CssTagExtensions {


		/////////////////////////////////////////////////////////////////////////////

		public static string Rgb24ToCss( byte red, byte green, byte blue )
		{
			var result = $"#{red:x2}{green:x2}{blue:x2}";
			return result;
		}


		/////////////////////////////////////////////////////////////////////////////

		public static T AddComment<T>( this IStyles<T> styles, string comment )
			where T : class
		{
			styles.AddComment( comment );
			return styles as T;
		}


		/////////////////////////////////////////////////////////////////////////////

		public static T ArbitraryStyle<T>( this IStyles<T> styles, string name, string values )
			where T : class
		{
			styles.AddStyle( name, values );
			return styles as T;
		}


		/////////////////////////////////////////////////////////////////////////////

		public static T Zoom<T>( this IStyles<T> styles, string values )
			where T : class
		{

			// *zoom ??

			styles.AddStyle( "zoom", values );
			return styles as T;
		}


		/////////////////////////////////////////////////////////////////////////////

		public static T Resize<T>( this IStyles<T> styles, string values )
			where T : class
		{
			styles.AddStyle( "resize", values );
			return styles as T;
		}


		/////////////////////////////////////////////////////////////////////////////

		//public static T Styles<T>( this IStyles<T> styles, IEnumerable<string> values )
		//	where T : class
		//{
		//	foreach( var value in values ) {
		//		//styles.AddStyle( ", values );
		//	}
		//	return styles as T;
		//}

	}
}
