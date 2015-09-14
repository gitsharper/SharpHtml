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

	public class HtmlItemsDictionary : KVPList<string> { //Dictionary<string, string> {

		public const string COMMENT_STR = "//";
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
		
		public void AddComment( string comment )
		{
			base.Add( new KVPair<string> { Key = COMMENT_STR, Value = comment } );
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

		public bool Add( string keyIn, string value )
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
			//: base( StringComparer.Ordinal )
		{
		}

		/////////////////////////////////////////////////////////////////////////////

		//public HtmlItemsDictionary( Dictionary<string, string> items = null )
		public HtmlItemsDictionary( HtmlItemsDictionary items = null )
			//: base( StringComparer.Ordinal )
		{
			Merge( items, true );
		}




	}
}