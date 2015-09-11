using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SharpHtml.Pages {
	using Layouts;

	/////////////////////////////////////////////////////////////////////////////

	public static partial class PageAndLayoutExtensions {


		/////////////////////////////////////////////////////////////////////////////

		public static Tag _AddLayout( this Tag toTag, ILayout layout )
		{
			//var properties = layout.GetType().GetRuntimeProperties().Where( pi => pi.PropertyType.;
			var type = layout.GetType();
			var properties = type.GetProperties( BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly );

			//
			// need to insert stuff in tag, but could be one or many
			//

			//toTag.AddChild()
			//   layout.Initialize( toTag );

			return toTag;
		}


	}
}
