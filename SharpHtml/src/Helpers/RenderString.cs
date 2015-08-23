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
using System.Text;

namespace SharpHtml {


	/////////////////////////////////////////////////////////////////////////////

	public class RenderString : IRender {

		public string Value { get; private set; }

		/////////////////////////////////////////////////////////////////////////////

		public string Render()
		{
			return Value;
		}

		/////////////////////////////////////////////////////////////////////////////

		public RenderString( string str = "" )
		{
			Value = String.IsNullOrEmpty( str ) ? string.Empty : str;
		}
	}

}