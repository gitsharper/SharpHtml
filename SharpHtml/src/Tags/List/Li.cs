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

	public class Li : Tag {
		protected override string _TagName => "li";

		/////////////////////////////////////////////////////////////////////////////

		public Li( string text )
		{
			SetValue( text );
		}

		/////////////////////////////////////////////////////////////////////////////

		public Li()
		{		
		}
	}

}