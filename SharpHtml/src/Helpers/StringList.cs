#region License
// 
// Author: Joe McLain <nmp.developer@outlook.com>
// Copyright (c) 2013, Joe McLain and Digital Writing
// 
// Licensed under Eclipse Public License, Version 1.0 (EPL-1.0)
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Soap;
//using System.Runtime.Serialization.Formatters.Binary;

namespace SharpHtml {

	////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// 	Collection of stringd using List&lt;T&gt;
	/// </summary>
	///
	/// <remarks>
	/// 	Jpm, 3/26/2011.
	/// </remarks>
	////////////////////////////////////////////////////////////////////////////

	[Serializable()]
	public class StringList : List<string> {

		bool unique = false;

		/////////////////////////////////////////////////////////////////////////////

		public new StringList Add( string s )
		{
			if( !unique || !this.Contains( s ) ) {
				base.Add( s );
			}

			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StringList Add( IEnumerable<string> collection )
		{
			// ******
			foreach( string s in collection ) {
				Add( s );
			}

			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StringList Add( string [] strs )
		{
			return Add( strs as IEnumerable<string> );
		}


		/////////////////////////////////////////////////////////////////////////////

		public string FirstAndSkip()
		{
			// ******
			if( 0 == Count ) {
				return null;
			}

			// ******
			string str = this [ 0 ];
			RemoveAt( 0 );

			// ******
			return str;
		}


		/////////////////////////////////////////////////////////////////////////////

		//public string PeekNextArg()
		//{
		//	// ******
		//	if( 0 == Count ) {
		//		return null;
		//	}

		//	// ******
		//	return this [ 0 ];
		//}


		/////////////////////////////////////////////////////////////////////////////

		//public string NextArg()
		//{
		//	//
		//	// treating StringList like a stack and "popping" from the head
		//	// of the list
		//	//

		//	// ******
		//	if( 0 == Count ) {
		//		return null;
		//	}

		//	// ******
		//	string arg = this [ 0 ];
		//	RemoveAt( 0 );

		//	// ******
		//	return arg;
		//}


		///////////////////////////////////////////////////////////////////////////////

		//public string NextArg( string defValue )
		//{
		//	string result = NextArg();
		//	return null == result ? defValue : result;
		//}


		/////////////////////////////////////////////////////////////////////////////

		public StringList SplitLines( string text, bool trim )
		{
			// ******
			if( string.IsNullOrEmpty( text ) ) {
				return this;
			}

			// ******
			var sb = new StringBuilder();

			int len = text.Length;
			int lastIndex = len - 1;

			for( int i = 0; i < len; i++ ) {
				char ch = text [ i ];
				if( SC.CR == ch || SC.NEWLINE == ch ) {
					string lineOfText = sb.ToString();
					sb.Length = 0;
					Add( trim ? lineOfText.Trim() : lineOfText );

					//
					// if we broke on a CR and a newline follows then we
					// eat the newline
					//
					if( i < lastIndex ) {
						char ch2 = text [ 1 + i ];
						if( SC.CR == ch && SC.NEWLINE == ch2 ) {
							++i;
						}
					}
				}
				else {
					sb.Append( ch );
				}
			}

			string str = sb.ToString();
			if( sb.Length > 0 ) {
				Add( trim ? str.Trim() : str );
			}

			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StringList SplitAndAdd( char splitChar, string str, Func<string, string> action )
		{
			//Add( str.Split(new char [] {splitChar}, StringSplitOptions.RemoveEmptyEntries) );
			string [] array = str.Split( new char [] { splitChar }, StringSplitOptions.RemoveEmptyEntries );
			if( null != action ) {
				foreach( string s in array ) {
					Add( action( s ) );
				}
			}
			else {
				Add( array );
			}

			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		//public void SplitAndAddOnComma( string str, Func<string, string> action )
		//{
		//	SplitAndAdd( ',', str, action );
		//}


		/////////////////////////////////////////////////////////////////////////////

		public string Join( string joinStr )
		{
			return Join( joinStr, null );
		}


		/////////////////////////////////////////////////////////////////////////////

		public string Join( string joinStr, Func<string, string> action = null )
		{
			// ******
			StringBuilder sb = new StringBuilder();

			// ******
			int count = 0;

			foreach( string s in this ) {
				if( count++ > 0 ) {
					sb.Append( joinStr );
				}

				sb.Append( null == action ? s : action( s ) );
			}

			// ******
			return sb.ToString();
		}


		/////////////////////////////////////////////////////////////////////////////

		public string Join( char chJoin, Func<string, string> action = null )
		{
			// ******
			StringBuilder sb = new StringBuilder();

			// ******
			int count = 0;

			foreach( string s in this ) {
				if( count++ > 0 ) {
					sb.Append( chJoin );
				}

				sb.Append( null == action ? s : action( s ) );
			}

			// ******
			return sb.ToString();
		}


		/////////////////////////////////////////////////////////////////////////////

		//public string JoinForCodeInjection()
		//{
		//	// ******
		//	StringBuilder sb = new StringBuilder();

		//	// ******
		//	int count = 0;

		//	foreach( string s in this ) {
		//		if( count++ > 0 ) {
		//			sb.Append( ", " );
		//		}

		//		sb.AppendFormat( "\"{0}\"", s.EscapeEscapes() );
		//	}

		//	// ******
		//	return sb.ToString();
		//}


		/////////////////////////////////////////////////////////////////////////////

		//public NmpStringArray ToStringArray( bool trimValue = false, char [] splitChars = null )
		//{
		//	return new NmpStringArray( this, trimValue, splitChars );
		//}


		/////////////////////////////////////////////////////////////////////////////

		public override string ToString()
		{

			//
			// since this is for use in the macro processor we only append
			// a newline
			//

			// ******
			StringBuilder sb = new StringBuilder();
			foreach( string s in this ) {
				sb.Append( s );
				sb.Append( '\n' );
			}

			// ******
			return sb.ToString();
		}

		/////////////////////////////////////////////////////////////////////////////

		[DebuggerStepThrough]
		public StringList( string text, bool trim )
		{
			SplitLines( text, trim );
		}


		/////////////////////////////////////////////////////////////////////////////

		[DebuggerStepThrough]
		public StringList( bool unique = false )
		{
			this.unique = unique;
		}


		/////////////////////////////////////////////////////////////////////////////

		[DebuggerStepThrough]
		public StringList( char ch, string str, bool unique = false, Func<string, string> action = null )
		{
			this.unique = unique;
			SplitAndAdd( ch, str, action );
		}


		///////////////////////////////////////////////////////////////////////////////

		//[DebuggerStepThrough]
		//public StringList( IEnumerable<string> collection, bool unique = false )
		//{
		//	this.unique = unique;
		//	Add( collection );
		//}


		/////////////////////////////////////////////////////////////////////////////

		[DebuggerStepThrough]
		public StringList( IEnumerable<object> collection, bool unique = false )
		{
			this.unique = unique;
			foreach( object o in collection ) {
				Add( o.ToString() );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		[DebuggerStepThrough]
		public StringList( params object [] values )
			: this( values, false )
		{
		}


		/////////////////////////////////////////////////////////////////////////////

		[DebuggerStepThrough]
		public StringList( IList<string> values, bool unique = false )
		{
			this.unique = unique;
			foreach( var value in values ) {
				Add( value );
			}
		}


	}
}
