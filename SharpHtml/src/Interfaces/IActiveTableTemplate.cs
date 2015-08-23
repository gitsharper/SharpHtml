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


	public interface IActiveTableTemplate {

		/// <summary>
		/// 
		/// See AddRow() in IDefineTableTemplate.cs
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="cellFunc"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		IActiveTableTemplate AddRow( TableSectionId id, CellFunc cellFunc, params object [] values );

		/// <summary>
		/// 
		/// Creates and returns a table
		/// 
		/// </summary>
		/// <param name="caption"></param>
		/// <param name="border"></param>
		/// <param name="id"></param>
		/// <param name="attrAndstyles"></param>
		/// <returns></returns>
		Table CreateTable( string caption = "", int? border = null, string id = "", IEnumerable<string> attrAndstyles = null );

	}


}
