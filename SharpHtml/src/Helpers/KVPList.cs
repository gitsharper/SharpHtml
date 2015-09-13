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


	/////////////////////////////////////////////////////////////////////////////

	public class KVPair<T> {
		public string Key { get; set; }
		public T Value { get; set; }
	}


	/////////////////////////////////////////////////////////////////////////////

	public class KVPList<T> : List<KVPair<T>> {

		/////////////////////////////////////////////////////////////////////////////

		public bool ContainsKey( string key )
		{
			return null != Find( kvp => kvp.Key == key );
		}


		///////////////////////////////////////////////////////////////////////////////

		public bool TryGetValue( string key, out T value )
		{
			var kvpFound = Find( kvp => kvp.Key == key );
			if( null != kvpFound ) {
				value = kvpFound.Value;
				return true;
			}

			value = default( T );
			return false;
		}


		///////////////////////////////////////////////////////////////////////////////

		public T this [ string key ]
		{
			get
			{
				var kvpFound = Find( kvp => kvp.Key == key );
				return null == kvpFound ? default( T ) : kvpFound.Value;
			}
			set
			{
				var kvpFound = Find( kvp => kvp.Key == key );
				if( null != kvpFound ) {
					kvpFound.Value = value;
				}
				else {
					Add( new KVPair<T> { Key = key, Value = value } );
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

		//public KVPList( StringComparer comparer )
		//{

		//}

	}



}