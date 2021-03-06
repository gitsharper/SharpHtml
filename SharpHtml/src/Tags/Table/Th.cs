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

	public class Th : Tag {

		// ******
		protected override string _TagName { get { return "th"; } }
		protected override TagFormatOptions MultipleTagFormatAlign { get { return TagFormatOptions.Horizontal; } }


		/////////////////////////////////////////////////////////////////////////////

		public Th()
		{
		}


		/////////////////////////////////////////////////////////////////////////////

		//public Th( IRender value, string formatStr, string id = "", IEnumerable<string> styles = null )
		//	: base( value, formatStr, id, styles )
		//{
		//}

	}
}
