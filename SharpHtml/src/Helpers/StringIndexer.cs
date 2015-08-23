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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.Globalization;
using System.IO;
using System.Reflection;

namespace SharpHtml {


	/////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Wraps a string that we can index as we move from character to character,
	/// used as input for simple string parsing
	/// </summary>

	[DebuggerDisplay( "remainder: {Remainder}" )]

	public class StringIndexer {

		// ******
		public const char	EOS = SC.NO_CHAR;

		// ******
		readonly string theStr = string.Empty;
		int length = 0;
		int index = 0;


		/////////////////////////////////////////////////////////////////////////////

		public string OriginalString { get { return theStr; } }

		int Length { get { return length; } }
		public bool Empty { get { return index >= Length; } }
		public int Index { get { return index; } }

		public int RemainderCount { get { return Length - index; } }
		public string Remainder { get { return theStr.Substring( index, length - index ); } }

		public bool AtEnd { get { return Empty; } }

		/////////////////////////////////////////////////////////////////////////////

		int FixLenth( int start, int length )
		{
			if( length < 1 || start >= Length ) {
				//
				// zero or fewer characters for length, or the value of start will
				// result in no characters in the string
				//
				return 0;
			}
			return length > Length - start ? Length - start : length;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool IndexInRange( int i )
		{
			return i >= 0 && i < Length;
		}


		/////////////////////////////////////////////////////////////////////////////

		public char this [ int reqIndex ]
		{
			get
			{
				//if( reqIndex < 0 || reqIndex >= theStr.Length ) {
				//	throw new ArgumentOutOfRangeException( "reqIndex" );
				//}
				//return theStr [ reqIndex ];
				return Peek( reqIndex );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool Contains( char ch )
		{
			return IndexOf( ch ) >= 0;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool Contains( char ch, bool ignoreCase )
		{
			return IndexOf( ch, ignoreCase ) >= 0;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool Contains( string str )
		{
			return IndexOf( str, false ) >= 0;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool Contains( string str, bool ignoreCase )
		{
			return IndexOf( str, ignoreCase ) >= 0;
		}


		/////////////////////////////////////////////////////////////////////////////

		public int IndexOf( char ch )
		{
			return theStr.IndexOf( ch, index, length - index ) - index;
		}


		/////////////////////////////////////////////////////////////////////////////

		public int IndexOf( char chIn, bool ignoreCase )
		{
			// ******
			if( !ignoreCase ) {
				return IndexOf( chIn );
			}

			// ******
			var ch = char.ToLower( chIn );
			for( int i = index; i < length; i++ ) {
				if( ch == char.ToLower( theStr [ i ] ) ) {
					return i - index;
				}
			}

			// ******
			return -1;
		}


		/////////////////////////////////////////////////////////////////////////////

		//
		// should return index be based on start+index or index ?,
		// indexof with start/len arguments returns index from the
		// start of the string
		//

		public int IndexOf( char ch, int start, int lenChars )
		{
			// ******
			var iStart = index + start;
			if( !IndexInRange( iStart ) ) {
				return -1;
			}

			// ******
			var lenToCheck = FixLenth( iStart, lenChars );
			if( lenToCheck < 1 ) {
				return -1;
			}

			// ******
			return theStr.IndexOf( ch, iStart, lenChars ) - index;
		}


		/////////////////////////////////////////////////////////////////////////////

		//
		// should return index be based on start+index or index ?,
		// indexof with start/len arguments returns index from the
		// start of the string
		//

		public int IndexOf( char chIn, int start, int lenChars, bool ignoreCase )
		{
			// ******
			if( !ignoreCase ) {
				return IndexOf( chIn, start, lenChars );
			}

			// ******
			var iStart = index + start;
			if( !IndexInRange( iStart ) ) {
				return -1;
			}

			// ******
			var lenToCheck = FixLenth( iStart, lenChars );
			if( lenToCheck < 1 ) {
				return -1;
			}

			// ******
			var end = iStart + lenToCheck;
			
			var ch = char.ToLower( chIn );
			for( int i = iStart; i < end; i++ ) {
				if( ch == char.ToLower( theStr [ i ] ) ) {
					return i - index;
				}
			}

			// ******
			return -1;
		}


		/////////////////////////////////////////////////////////////////////////////

		//
		// should return index be based on start+index or index ?,
		// indexof with start/len arguments returns index from the
		// start of the string
		//

		public int IndexOf( string str, int start, int lenChars, bool ignoreCase )
		{
			// ******
			var iStart = index + start;
			if( !IndexInRange( iStart ) ) {
				return -1;
			}

			// ******
			var lenToCheck = FixLenth( iStart, lenChars );
			if( lenToCheck < 1 ) {
				return -1;
			}

			// ******
			return theStr.IndexOf( str, iStart, lenToCheck, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal ) - index;
		}


		/////////////////////////////////////////////////////////////////////////////

		public int IndexOf( string str, bool ignoreCase )
		{
			return theStr.IndexOf( str, index, length - index, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal ) - index;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool StartsWith( string cmpStr )
		{
			// ******
			int cmpStrLen = cmpStr.Length;
			if( cmpStrLen > RemainderCount ) {
				return false;
			}

			// ******
			for( int i = 0; i < cmpStrLen; i++ ) {
				if( cmpStr [ i ] != theStr [ index + i ] ) {
					return false;
				}
			}

			// ******
			return true;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool StartsWith( string cmpStr, bool ignoreCase )
		{
			// ******
			if( !ignoreCase ) {
				return StartsWith( cmpStr );
			}

			// ******
			int cmpStrLen = cmpStr.Length;
			if( cmpStrLen > RemainderCount ) {
				return false;
			}

			// ******
			for( int i = 0; i < cmpStrLen; i++ ) {
				if( char.ToUpperInvariant( cmpStr [ i ] ) != char.ToUpperInvariant( theStr [ index + i ] ) ) {
					return false;
				}
			}

			// ******
			return true;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool EndsWith( string cmpStr, bool ignoreCase )
		{
			// ******
			if( RemainderCount < cmpStr.Length ) {
				return false;
			}

			// ******
			return theStr.EndsWith( cmpStr, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal );
		}


		/////////////////////////////////////////////////////////////////////////////

		public char TrimStart()
		{
			return Skip( c => char.IsWhiteSpace( c ) );
		}


		/////////////////////////////////////////////////////////////////////////////

		public void TrimEnd()
		{
			if( RemainderCount > 0 ) {
				var str = theStr.TrimEnd();
				length = str.Length;
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public char LastChar()
		{
			// ******
			if( 0 == RemainderCount ) {
				return EOS;
			}

			// ******
			return theStr [ length - 1 ];
		}


		/////////////////////////////////////////////////////////////////////////////

		public char SkipBackwards( Predicate<char> cmp )
		{
			// ******
			char ch;
			while( EOS != (ch = LastChar()) && cmp( ch ) ) {
				length -= 1;
			}

			// ******
			return ch;
		}



		/////////////////////////////////////////////////////////////////////////////

		public void SkipAll()
		{
			index = length;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool Skip( int nChars = 1 )
		{
			// ******
			index += nChars;

			if( index >= Length ) {
				index = Length;
				//
				// is empty
				//
				return true;
			}
			else {
				//
				// NOT empty;
				//
				return false;
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public char SkipAndNext( int nChars = 1 )
		{
			Skip( nChars );
			return Peek();
		}


		/////////////////////////////////////////////////////////////////////////////

		public char Skip( Predicate<char> cmp )
		{
			while( EOS != Peek() && cmp( Peek() ) ) {
				NextChar();
			}
			return Peek();
		}


		/////////////////////////////////////////////////////////////////////////////

		// Peek(0) is the same as Peek()

		public char Peek( int indexAhead )
		{
			var peekIndex = index + indexAhead;
			return peekIndex < 0 || peekIndex >= length ? EOS : theStr [ peekIndex ];
		}


		/////////////////////////////////////////////////////////////////////////////

		public char Peek()
		{
			return index < 0 || index >= length ? EOS : theStr [ index ];
		}


		/////////////////////////////////////////////////////////////////////////////

		public char NextChar()
		{
			char ch = Peek();
			if( ch != EOS ) {
				++index;
			}
			return ch;
		}


		///////////////////////////////////////////////////////////////////////////

		public string NextChars( int count )
		{
			// ******
			if( 0 == count ) {
				return string.Empty;
			}

			// ******
			var sb = new StringBuilder();
			while( count-- > 0 ) {
				sb.Append( NextChar() );
			}

			// ******
			return sb.ToString();
		}


		/////////////////////////////////////////////////////////////////////////////

		public void Reset()
		{
			index = 0;
			length = theStr.Length;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StringIndexer( string s )
		{
			theStr = string.IsNullOrEmpty( s ) ? string.Empty : s;
			Reset();
		}
	}



}
