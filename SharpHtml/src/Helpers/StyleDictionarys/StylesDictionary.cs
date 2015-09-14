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
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SharpHtml {


	public class StylesDictionary : HtmlItemsDictionary, IStyles<StylesDictionary> {


		/////////////////////////////////////////////////////////////////////////////

		public new StylesDictionary AddComment( string comment )
		{
			base.AddComment( comment );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesDictionary ReplaceStyle( string key, string value )
		{
			Add( key, value );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesDictionary AddStyle( string key, string value )
		{
			Add( key, value );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesDictionary AddStyles( IEnumerable<string> styles )
		{
			Merge( styles.ToDictionary<StylesDictionary>( true ), throwOnError: false );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesDictionary AddStyles( params string [] styles )
		{
			AddStyles( (IEnumerable<string>) styles );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public int Render( StringBuilder sb, bool appendNewline )
		{
			// ******
			int nStyles = 0;

			foreach( var attribute in this ) {
				string key = attribute.Key;

				if( COMMENT_STR == key ) {
					continue;
				}

				// ******
				if( String.Equals( key, "id", StringComparison.Ordinal /* case-sensitive */) && String.IsNullOrEmpty( attribute.Value ) ) {
					continue;
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
				//sb.Append( key );
				//sb.Append( " : " );
				//sb.Append( value );
				//sb.Append( ';' );

				sb.Append( string.Format( "{0} : {1};", key, value ) );

				if( appendNewline ) {
					sb.AppendLine();
				}
				else {
					sb.Append( ' ' );
				}

				nStyles += 1;
			}

			// ******
			return nStyles;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesDictionary()
		{
		}

	}

}