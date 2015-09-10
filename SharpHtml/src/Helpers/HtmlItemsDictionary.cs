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

namespace SharpHtml {


	public class Pair<T, T1> {
		public T Key { get; set; }
		public T1 Value { get; set; }
	}

	public class KVPList : List<Pair<string, string>> {


		/////////////////////////////////////////////////////////////////////////////

		public bool ContainsKey( string key )
		{
			return null != Find( kvp => kvp.Key == key );
		}


		///////////////////////////////////////////////////////////////////////////////

		public bool TryGetValue( string key, out string value)
		{
			var kvpFound = Find( kvp => kvp.Key == key );
			if( null != kvpFound) {
				value = kvpFound.Value;
				return true;
			}

			value = null;
			return false;
		}


		///////////////////////////////////////////////////////////////////////////////

		public string this [ string key ]
		{
			get
			{
				var kvpFound = Find( kvp => kvp.Key == key );
				return null == kvpFound ? null : kvpFound.Value;
			}
			set
			{
				var kvpFound = Find( kvp => kvp.Key == key );
				if( null != kvpFound ) {
					kvpFound.Value = value;
				}
				else {
					Add( new Pair<string, string> { Key = key, Value = value } );
				}
			}
		}


		///////////////////////////////////////////////////////////////////////////////
		//
		//public string Key( int index )
		//{
		//	// ******
		//	if( index < 0 || index >= Count ) {
		//		throw new ArgumentOutOfRangeException( "index" );
		//	}
		//
		//	return base [ index ].Key;
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//public string Value( int index )
		//{
		//	// ******
		//	if( index < 0 || index >= Count ) {
		//		throw new ArgumentOutOfRangeException( "index" );
		//	}
		//
		//	return base [ index ].Value;
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//public KVPList ParsePairs( string text )
		//{
		//	// ******
		//	var lines = new StringList( text, false );
		//
		//	// ******
		//	foreach( string line in lines ) {
		//		if( string.IsNullOrEmpty( line ) ) {
		//			continue;
		//		}
		//
		//		// ******
		//		int index = line.IndexOfAny( new char [] { '=' } );
		//
		//		string key = index < 0 ? line : line.Substring( 0, index ).Trim();
		//		string value = index < 0 ? string.Empty : line.Substring( 1 + index ).Trim();
		//
		//		Add( new KeyValuePair<string, string>( key, value ) );
		//	}
		//
		//	// ******
		//	return this;
		//}

		/////////////////////////////////////////////////////////////////////////////

		public KVPList( StringComparer comparer )
		{

		}

	}


	/////////////////////////////////////////////////////////////////////////////

	//
	// stolen from some version of MVC.Net;
	//
	// changes:
	//
	//  o  NOT sorted, want to see them in the order that I add them
	//  o  by default the caller has to say whether they want a new
	//     key to overwrite an existing one; that was 'false', its going
	//     to become 'true' - its easier to track down a last bad attribute
	//     or style than it is to figure out where the one that should be
	//     there was added incorrectly
	//
	//public class HtmlItemsDictionary : SortedDictionary<string, string> {

	// need ordered key value pair where the order is the order in which
	// they were entered, if one is eliminated then it's replacement shows up
	// at the current end

	public class HtmlItemsDictionary : KVPList { //Dictionary<string, string> {

		public const bool DefAllowOverwrite = true;

		// ******
		public bool AllowOverwrite { get; set; } = DefAllowOverwrite;

		//protected KVPList list = new KVPList( StringComparer.Ordinal );


		/////////////////////////////////////////////////////////////////////////////

		public T Clone<T>()
			where T : HtmlItemsDictionary, new()
		{
			// ******
			var dict = new T { };

			foreach( var kvp in this ) {
				dict.Add( kvp.Key, kvp.Value );
			}

			// ******
			return dict;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool Add( string keyIn, string value, bool allowOverwrite )
		{
			// ******
			var key = keyIn.ToLower().Trim();
			if( string.IsNullOrEmpty( key ) ) {
				throw new ArgumentException( "key must not be null or empty" );
			}

			// ******
			if( AllowOverwrite || !ContainsKey( key ) ) {
				base [ key ] = value.Trim();
				return true;
			}
			else {
				return false;
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool Replace( string keyIn, string value )
		{
			return Add( keyIn, value, true );
		}


		/////////////////////////////////////////////////////////////////////////////

		public new bool Add( string keyIn, string value )
		{
			return Add( keyIn, value, AllowOverwrite );
		}


		/////////////////////////////////////////////////////////////////////////////

		public new string this [ string key ]
		{
			get
			{
				return base [ key ];
			}
			set
			{
				this.Add( key, value );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public new bool ContainsKey( string key )
		{
			return base.ContainsKey( key.ToLower() );
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool Add( string key, object value )
		{
			return Add( key, value.ToString() );
		}


		/////////////////////////////////////////////////////////////////////////////

		//public void Merge( Dictionary<string, string> items, bool throwOnError )
		public void Merge( HtmlItemsDictionary items, bool throwOnError )
		{
			// ******
			if( null == items ) {
				return;
			}

			// ******
			foreach( var item in items ) {
				if( !Add( item.Key, item.Value ) ) {
					throw new ArgumentException( "duplicate key \"{0}\"", item.Key );
				}
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public HtmlItemsDictionary()
			: base( StringComparer.Ordinal )
		{
		}

		/////////////////////////////////////////////////////////////////////////////

		//public HtmlItemsDictionary( Dictionary<string, string> items = null )
		public HtmlItemsDictionary( HtmlItemsDictionary items = null )
			: base( StringComparer.Ordinal )
		{
			Merge( items, true );
		}




	}
}