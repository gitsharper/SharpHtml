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

	public class A : Tag {
		protected override string _TagName => "a";

		/////////////////////////////////////////////////////////////////////////////

		public A AsLink( string name, string uri = null )
		{
			AddAttribute( "href", (uri?.Trim() ?? string.Empty) + '#' + (name?.Trim() ?? string.Empty) );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public A AsAnchor( string name )
		{
			AddAttribute( "name", name?.Trim() ?? string.Empty );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public A()
		{		
		}


		/////////////////////////////////////////////////////////////////////////////

		public A( string uri)
		{
			AddAttribute( "href", uri?.Trim() ?? string.Empty );
		}

	}

}