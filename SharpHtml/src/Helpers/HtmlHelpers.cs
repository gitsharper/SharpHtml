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
using System.Text;

namespace SharpHtml {


	/////////////////////////////////////////////////////////////////////////////

	public static class HtmlHelpers {

		/////////////////////////////////////////////////////////////////////////////

		public static List<string> CreateList( params string [] styles )
		{
			return new List<string>( styles );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static string CreateSanitizedId( string originalId, string invalidCharReplacement = null )
		{
			if( String.IsNullOrWhiteSpace( originalId ) ) {
				return null;
			}

			if( null == invalidCharReplacement ) {
				//throw new ArgumentNullException( "invalidCharReplacement" );
				invalidCharReplacement = RestrictedCharChecks.IdAttributeDotReplacement;
			}

			char firstChar = originalId [ 0 ];
			if( !RestrictedCharChecks.IsLetter( firstChar ) ) {
				// the first character must be a letter
				return null;
			}

			StringBuilder sb = new StringBuilder( originalId.Length );
			sb.Append( firstChar );

			for( int i = 1; i < originalId.Length; i++ ) {
				char thisChar = originalId [ i ];
				if( RestrictedCharChecks.IsValidIdCharacter( thisChar ) ) {
					sb.Append( thisChar );
				}
				else {
					sb.Append( invalidCharReplacement );
				}
			}

			return sb.ToString();
		}


		/////////////////////////////////////////////////////////////////////////////

		private static class RestrictedCharChecks {

			/////////////////////////////////////////////////////////////////////////////

			const string DEF_DOT_REPLACEMENT = "_";
			private static string _idAttributeDotReplacement;

			public static string IdAttributeDotReplacement
			{
				get
				{
					if( String.IsNullOrEmpty( _idAttributeDotReplacement ) ) {
						_idAttributeDotReplacement = DEF_DOT_REPLACEMENT;
					}
					return _idAttributeDotReplacement;
				}
				set
				{
					_idAttributeDotReplacement = value;
				}
			}


			private static bool IsAllowableSpecialCharacter( char c )
			{
				switch( c ) {
					case '-':
					case '_':
					case ':':
						// note that we're specifically excluding the '.' character
						return true;

					default:
						return false;
				}
			}

			private static bool IsDigit( char c )
			{
				return ('0' <= c && c <= '9');
			}

			public static bool IsLetter( char c )
			{
				return (('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z'));
			}

			public static bool IsValidIdCharacter( char c )
			{
				return (IsLetter( c ) || IsDigit( c ) || IsAllowableSpecialCharacter( c ));
			}
		}
	}
}