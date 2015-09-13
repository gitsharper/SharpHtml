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
