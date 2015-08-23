#region License
// 
// Author: Joe McLain <nmp.developer@outlook.com>
// Copyright (c) 2015, Joe McLain and Digital Writing
// 
// Licensed under Eclipse Public License, Version 1.0 (EPL-1.0)
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Linq;
using System.Text;


namespace SharpHtml {


	/////////////////////////////////////////////////////////////////////////////

	public class AttributesDictionary : HtmlItemsDictionary {

		/////////////////////////////////////////////////////////////////////////////

		public int Render( StringBuilder sb )
		{
			// ******
			int nAttributes = 0;

			if( base.ContainsKey( "id" ) ) {
				var value = base [ "id" ];
				sb.AppendFormat( " id=\"{0}\"", value );
				nAttributes += 1;
			}

			foreach( var attribute in this ) {
				var key = attribute.Key;

				// ******
				if( string.Equals( key, "id", StringComparison.Ordinal ) ) {
					//
					// already taken care of
					//
					continue; // DevDiv Bugs #227595: don't output empty IDs
				}

				// ******
				//string value = HttpUtility.HtmlAttributeEncode( attribute.Value );
				//
				// HtmlEncode() is almost certainly WRONG
				//
				// found this, not looked at yet: http://www.html-5.com/tutorials/url-encoding-tutorial.html
				//
				//string value = attribute.Value.HtmlEncode( false );
				string value = SC.HtmlEncode( attribute.Value, false );

				// ******
				sb.Append( ' ' )
					.Append( key )
					.Append( "=\"" )
					.Append( value )
					.Append( '"' );

				nAttributes += 1;
			}

			// ******
			return nAttributes;
		}


		/////////////////////////////////////////////////////////////////////////////

		public AttributesDictionary()
		{
		}

	}
}