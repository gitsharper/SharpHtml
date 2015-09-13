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
using System.Text.RegularExpressions;

namespace SharpHtml {


	/////////////////////////////////////////////////////////////////////////////

	public class Stylesheet {

		//KVPList<StyleSheetBlock> Styles { get; } = new KVPList<StyleSheetBlock> { };
		List<StylesheetBlock> Styles { get; } = new List<StylesheetBlock> { };

		// get names of all classes and id's used

		// reports

		// pass in Html and walk tree to find all classes referenced

		/////////////////////////////////////////////////////////////////////////////

		public Stylesheet Add( StylesheetBlock sb )
		{
			Styles.Add( sb );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Stylesheet Add( IEnumerable<StylesheetBlock> blocks )
		{
			foreach( var block in blocks ) {
				Add( block );
			}
			return this;
		}




	}


	/////////////////////////////////////////////////////////////////////////////

	//
	// because we inherit from StylesDictionary we get all the extension methods
	// BUT if we use them they return StylesDictionary, we want StyleBlock2
	//

	public enum StyleBlockType {
		Id,
		Class,
		Other
	}


	public partial class StylesheetBlock : IStyles<StylesheetBlock> {

		//public string ClassOrId { get; set; } = "";

		public List<string> ClassesOrIds { get; } = new List<string> { };

		public string Target { get; set; } = "";


		public StyleBlockType BlockType { get; set; } = StyleBlockType.Other;

		StylesDictionary Styles = new StylesDictionary { };

		/*
			we can set a single name, multiple names

			and/or we can attach a style block to an element and pick them up there for
			inclusion in the style sheet

			if pick up from elements then need a collection with a key to allow us to look up
			style block to add to element; need that (or other) data structure to back
			reference the element so we can pick up it's id or use class name

				.class1, .class2, #id1, #id2 ... { }

				.header-container,
				.footer-container,
				.main aside {
					background: #f16529;
				}


		*/

		/////////////////////////////////////////////////////////////////////////////

		static void Test()
		{
			var tag = new Div { };
			tag.Background( "green" );

			var sb1 = new StyleBlock { };



			var sb2 = new StylesheetBlock { }
				.SetTarget( "aside" )
				.Background( "#f16529" );





		}


		/////////////////////////////////////////////////////////////////////////////
		//
		// start with: a letter, '-', or '_'
		//
		// contains and ends with: letters, digits, '-', or '_'
		//
		// css actually allows pretty much anything if you encode it, but for the
		// sake of simplicity we only allow these - anyone finds an error notify
		// the author
		//

		const string regExIdOrClassMatch = "^[a-zA-Z0-9-_]*?[a-zA-Z0-9-_]$";
		static Regex rxIdOrClassMatch;

		protected void ValidateIdentifier( string id )
		{
			// ******
			if( null == rxIdOrClassMatch ) {
				rxIdOrClassMatch = new Regex( regExIdOrClassMatch );
			}

			// ******
			if( !rxIdOrClassMatch.IsMatch( id.Trim() ) ) {
				throw new ArgumentException( $"\"{}\" is not a valid identifier" );
			}
		}

		/////////////////////////////////////////////////////////////////////////////

		public StylesheetBlock SetTarget( string target )
		{
			Target = target;
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesheetBlock AddId( string id, params string [] ids )
		{
			// ******
			ValidateIdentifier( id );
			ClassesOrIds.Add( "#" + id );

			// ******
			foreach( var item in ids ) {
				ValidateIdentifier( "#" + item );
				ClassesOrIds.Add( item );
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesheetBlock AddClass( string className, params string [] classNames )
		{
			// ******
			ValidateIdentifier( className );
			ClassesOrIds.Add( "." + className );

			// ******
			foreach( var item in classNames ) {
				ValidateIdentifier( "." + item );
				ClassesOrIds.Add( item );
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesheetBlock()
		{
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesheetBlock( string target )
		{
			this.Target = target.Trim();
		}


	}



	/////////////////////////////////////////////////////////////////////////////

	partial class StylesheetBlock {

		/////////////////////////////////////////////////////////////////////////////

		public StylesheetBlock ReplaceStyle( string key, string value )
		{
			Styles.ReplaceStyle( key, value );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesheetBlock AddStyle( string key, string value )
		{
			Styles.AddStyle( key, value );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesheetBlock AddStyles( IEnumerable<string> styles )
		{
			Styles.AddStyles( styles );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesheetBlock AddStyles( params string [] styles )
		{
			return AddStyles( (IEnumerable<string>) styles );
		}


		/////////////////////////////////////////////////////////////////////////////

		public StylesheetBlock Clone()
		{
			var sb = (StylesheetBlock) MemberwiseClone();
			foreach( var kvp in Styles ) {
				sb.Styles.Add( kvp.Key, kvp.Value );
			}
			return sb;
		}

	}

}
