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

	public class Link : Tag {
		protected override string _TagName { get { return "link"; } }
		protected override TagRenderMode RenderMode { get { return TagRenderMode.VoidTag; } }


		///////////////////////////////////////////////////////////////////////////

		public Tag Initialize( string href, string rel )
		{
			// ******
			if( string.IsNullOrWhiteSpace( rel ) ) {
				throw new ArgumentNullException( "rel" );
			}
			Attributes.Add( "rel", rel.Trim() );

			//if( !string.IsNullOrWhiteSpace( type ) ) {
			//	Attributes.Add( "type", type.Trim() );
			//}

			if( !string.IsNullOrWhiteSpace( href ) ) {
				Attributes.Add( "href", href.Trim() );
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Link()
		{
		}

		/////////////////////////////////////////////////////////////////////////////

		public Link( string href, string rel )
		{
			Initialize( href, rel );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static Link NewStylesheetRef( string href )
		{
			return (Link) new Link().AddAttribute( "rel", "stylesheet" ).AddAttribute( "href", href?.Trim() ?? string.Empty );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static Link NewReference( string rel, string href )
		{
			// ******
			if( string.IsNullOrWhiteSpace( rel ) ) {
				throw new ArgumentNullException( nameof( rel ) );
			}

			// ******
			return (Link) new Link().AddAttribute( "rel", rel?.Trim() ?? string.Empty ).AddAttribute( "href", href?.Trim() ?? string.Empty );
		}



	}
}
