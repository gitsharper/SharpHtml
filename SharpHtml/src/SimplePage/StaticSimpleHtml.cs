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

using SharpHtml;

namespace SharpHtml.Simple {

	public static class StaticSimpleHtml {

		/////////////////////////////////////////////////////////////////////////////

		public static SimpleHtml AddToHeader( this SimpleHtml html, params Tag [] tags )
		{
			foreach( var tag in tags ) {
				if( null != tag ) {
					html.BodyHeader.AddChild( tag );
				}
			}
			return html;
		}


		/////////////////////////////////////////////////////////////////////////////

		public static SimpleHtml AddToBody( this SimpleHtml html, params Tag [] tags )
		{
			foreach( var tag in tags ) {
				if( null != tag ) {
					html.BodyMain.AddChild( tag );
				}
			}
			return html;
		}


		/////////////////////////////////////////////////////////////////////////////

		public static SimpleHtml AddToFooter( this SimpleHtml html, params Tag [] tags )
		{
			foreach( var tag in tags ) {
				if( null != tag ) {
					html.BodyFooter.AddChild( tag );
				}
			}
			return html;
		}


	}
}
