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

	public class StyleBlock : StylesDictionary {

		public string Name { get; protected set; } = "";

		//
		// the extension method AddToDictionary() should find us
		//

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
