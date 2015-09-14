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

namespace SharpHtml {


	/////////////////////////////////////////////////////////////////////////////

	//
	// because we inherit from StylesDictionary we get all the extension methods
	// BUT if we use them they return StylesDictionary, we want StyleBlock
	//

	public class StyleBlock : StylesDictionary, IStyles<StyleBlock> {

		public string Name { get; protected set; } = "";

		//
		// the extension method AddToDictionary() should find us
		//

		/////////////////////////////////////////////////////////////////////////////

		public new StyleBlock AddComment( string comment )
		{
			base.AddComment( comment );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public new StyleBlock ReplaceStyle( string key, string value )
		{
			base.ReplaceStyle( key, value );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public new StyleBlock AddStyle( string key, string value )
		{
			base.AddStyle( key, value );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public new StyleBlock AddStyles( IEnumerable<string> styles )
		{
			base.AddStyles( styles );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public new StyleBlock AddStyles( params string [] styles )
		{
			return AddStyles( (IEnumerable<string>) styles );
		}


		/////////////////////////////////////////////////////////////////////////////

		public StyleBlock Clone()
		{
			var sb = (StyleBlock) MemberwiseClone();
			foreach( var kvp in this ) {
				sb.Add( kvp.Key, kvp.Value );
			}
			return sb;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StyleBlock()
		{
			Name = string.Empty;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StyleBlock( string name, IEnumerable<string> styles )
		{
			// ******
			if( string.IsNullOrWhiteSpace( name ) ) {
				throw new ArgumentNullException( nameof( name ) );
			}
			// ******
			Name = name;
			if( null != styles ) {
				styles.AddToDictionary( this, false );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public StyleBlock( string name, params string [] styles )
			: this( name, (IEnumerable<string>) styles )
		{
		}


		/////////////////////////////////////////////////////////////////////////////

		public StyleBlock( string name, StylesDictionary styles )
		{
			// ******
			if( string.IsNullOrWhiteSpace( name ) ) {
				throw new ArgumentNullException( nameof( name ) );
			}

			// ******
			Name = name;
			if( null != styles ) {
				Merge( styles, false );
			}
		}

	}
}
