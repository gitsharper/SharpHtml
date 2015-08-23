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

namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////

	public static partial class LocalExtensions {

		static readonly char [] splitChars = new char [] { ':', '=' };


		/////////////////////////////////////////////////////////////////////////////
		//
		// utility
		//
		/////////////////////////////////////////////////////////////////////////////

		public static bool TrySplitString( this string str, out char splitChar, out string lhs, out string rhs )
		{
			return TrySplitString( str, splitChars, out splitChar, out lhs, out rhs );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static bool TrySplitString( this string str, char [] splitChars, out char splitChar, out string lhs, out string rhs )
		{
			// ******
			var pos = str.IndexOfAny( splitChars );
			if( pos > 0 ) {
				splitChar = str [ pos ];
				lhs = str.Substring( 0, pos ).Trim();
				rhs = str.Substring( 1 + pos );
				return true;
			}
			else {
				splitChar = '\0';
				lhs = str.Trim();
				rhs = string.Empty;
				return false;
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public static bool TrySplitString( this string str, char [] splitChars, out string lhs, out string rhs )
		{
			// ******
			var pos = str.IndexOfAny( splitChars );
			if( pos > 0 ) {
				lhs = str.Substring( 0, pos ).Trim();
				rhs = str.Substring( 1 + pos );
				return true;
			}
			else {
				lhs = str.Trim();
				rhs = string.Empty;
				return false;
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public static T ToDictionary<T>( this IEnumerable<string> items, bool throwOnError = true )
			where T : HtmlItemsDictionary, new()
		{
			// ******
			var newDict = new T { };
			if( null == items ) {
				return newDict;
			}

			// ******
			foreach( var item in items ) {
				if( string.IsNullOrWhiteSpace( item ) ) {
					continue;
				}

				string key;
				string value;
				char splitChar;

				if( item.TrySplitString( splitChars, out splitChar, out key, out value ) ) {
					newDict.Add( key, value );
				}
				else if( throwOnError ) {
					throw new ArgumentException( string.Format( "bad dictionary item definition, requires a  \"name = value\", found: \"{0}\"", item ) );
				}
			}

			// ******
			return newDict;
		}


		/////////////////////////////////////////////////////////////////////////////

		public static AttributesDictionary ToAttributesDictionary( this IEnumerable<string> items, bool throwOnError = true )
		{
			return ToDictionary<AttributesDictionary>( items );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static StylesDictionary ToStylesDictionary( this IEnumerable<string> items, bool throwOnError = true )
		{
			return ToDictionary<StylesDictionary>( items );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static void AddAttributesAndStyles( this IEnumerable<string> items, AttributesDictionary ad, StylesDictionary sd, bool throwOnError = true )
		{
			// ******
			if( null == items ) {
				throw new ArgumentNullException( "items" );
			}

			if( null == ad ) {
				throw new ArgumentNullException( "attributesDictionary" );
			}

			if( null == sd ) {
				throw new ArgumentNullException( "stylesDictionary" );
			}

			// ******
			foreach( var item in items ) {
				string key, value;
				char splitChar;

				if( item.TrySplitString( splitChars, out splitChar, out key, out value ) ) {
					if( SC.COLON == splitChar ) {
						sd.Add( key, value );
					}
					else {
						ad.Add( key, value );
					}
				}
				else if( throwOnError ) {
					throw new ArgumentException( "bad dictionary item definition, requires \"name = value\", found: \"{0}\"", item );
				}
			}
		}

		/////////////////////////////////////////////////////////////////////////////

		public static void AddToDictionary( this IEnumerable<string> items, HtmlItemsDictionary dict, bool throwOnError )
		{
			// ******
			if( null == items ) {
				throw new ArgumentNullException( "items" );
			}

			if( null == dict ) {
				throw new ArgumentNullException( "attributesDictionary" );
			}

			// ******
			foreach( var item in items ) {
				string key, value; char splitChar;

				if( item.TrySplitString( splitChars, out splitChar, out key, out value ) ) {
					dict.Add( key, value );
				}
				else if( throwOnError ) {
					throw new ArgumentException( "bad dictionary item definition, requires \"name = value\", found: \"{0}\"", item );
				}
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public static List<StylesDictionaryList> Clone( this List<StylesDictionaryList> outerList )
		{
			// ******
			var outerListClone = new List<StylesDictionaryList> { };

			foreach( var innerList in outerList ) {
				var newInnerList = new StylesDictionaryList { };
				outerListClone.Add( newInnerList );

				foreach( var styles in innerList ) {
					StylesDictionary innerStyles;
					newInnerList.Add( innerStyles = new StylesDictionary { } );
					styles.Each( item => innerStyles.Add( item.Key, item.Value ) );
				}
			}

			// ******
			return outerListClone;
		}


		/////////////////////////////////////////////////////////////////////////////

		public static List<List<T>> Clone<T>( this List<List<T>> outerList )
			where T : Tag, new()
		{
			// ******
			var clone = new List<List<T>> { };

			foreach( List<T> listOfT in outerList ) {
				var newList = new List<T> { };
				listOfT.Each( item => newList.Add( (T) item.Clone() ) );
				clone.Add( newList );
			}

			// ******
			return clone;
		}

	}

}
