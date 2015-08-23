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
using System.Threading.Tasks;
using System.Reflection;

namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////

	public static class Utilities {


		/////////////////////////////////////////////////////////////////////////////

		public static void Each<T>( this IEnumerable<T> items, Action<T> action )
		{
			if( null == items ) { return; }
			foreach( var item in items ) {
				action( item );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		//
		// you can also use Concat<T>(), and Union<T>() if you don't want duplicated;
		// this, however just appends the second to the first using lists
		//

		public static List<T> Append<T>( this IEnumerable<T> items, IEnumerable<T> moreItems )
		{
			if( null == items ) {
				throw new ArgumentNullException( nameof( items ) );
			}

			if( null == moreItems ) {
				throw new ArgumentNullException( nameof( moreItems ) );
			}

			var list = items.ToList();
			list.AddRange( moreItems );
			return list;
		}


		/////////////////////////////////////////////////////////////////////////////

		static Type [] singleStringArg =  new Type [] { typeof( string ) };

		public static string ApplyFormat( this object value, string fmtStr )
		{
			// ******
			if( null == value ) {
				return string.Empty;
			}
			else if( string.IsNullOrWhiteSpace( fmtStr ) ) {
				return value.ToString();
			}

			// ******
			try {
				if( value is string ) {
					return string.Format( fmtStr, value );
				}
				else {
					MethodInfo mi = value.GetType().GetMethod( "ToString", singleStringArg );
					if( null != mi ) {
						return mi.Invoke( value, new object [] { fmtStr } ) as string;
					}
					return value.ToString();
				}
			}
			catch( Exception ex ) {
				throw new Exception( "while formatting object", ex );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public static IEnumerable<object> ApplyFormat( IEnumerable<object> items, params string [] fmtArgs )
		{
			// ******
			if( 0 == fmtArgs.Length ) {
				return items;
			}

			// ******
			var list = new List<object> { };

			int fmtArgsLen = fmtArgs.Length;
			int index = 0;
			var parameters = new Type [] { typeof( string ) };

			foreach( var item in items ) {
				object result = null;

				if( index < fmtArgsLen && !string.IsNullOrWhiteSpace( fmtArgs [ index ] ) ) {
					MethodInfo mi = item.GetType().GetMethod( "ToString", parameters );
					if( null != mi ) {
						result = mi.Invoke( item, new object [] { fmtArgs [ index ] } );
					}
				}

				// ******
				list.Add( result ?? item.ToString() );
				index += 1;
			}

			// ******
			return list;
		}

		/////////////////////////////////////////////////////////////////////////////

		public static string Indent( this string str, string prependStr, int count, char notFirstChar = '\0' )
		{
			//const int MAX_INDENT = 256;

			// ******
			if( string.IsNullOrEmpty( str ) ) {
				return string.Empty;
			}

			// ******
			if( string.IsNullOrEmpty( str ) ) {
				return string.Empty;
			}

			// ******
			var sb = new StringBuilder { };

			// ******
			//
			// insert AFTER newline and at char pos 0 if char 0 is NOT a newline
			//
			// do not insert if first char of line is notFirstChar (which we need for calls
			// from Nmp where SpecialChars.EOL_COMMENT_PLACEHOLDER must be handled specially - we
			// need to leave it there so the whole line will be removed when Nmp finalizes the output)
			//
			var si = new StringIndexer( str );

			if( SC.NEWLINE != si.Peek() ) {
				sb.Append( prependStr );
			}

			while( !si.AtEnd ) {
				var ch = si.NextChar();

				// 
				// newline
				// more than just a single newline (don't indent a last empty line)
				// and not a placeholder that needs to be deleted during finalization
				//
				if( SC.NEWLINE == ch && si.RemainderCount > 1 && notFirstChar != si.Peek() ) {
					sb.Append( SC.NEWLINE );
					sb.Append( prependStr );
				}
				else {
					sb.Append( ch );
				}
			}

			// ******
			var result = sb.ToString();
			return result;
		}



	}


}
