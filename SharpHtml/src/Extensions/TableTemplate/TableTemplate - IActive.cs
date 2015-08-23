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
using System.Threading.Tasks;

namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////

	 partial class TableTemplate {


		/////////////////////////////////////////////////////////////////////////////
		//
		//  IActiveTableTemplate
		//
		/////////////////////////////////////////////////////////////////////////////

		public static IActiveTableTemplate AddHeaderRow( this IActiveTableTemplate tt, CellFunc cellFunc, params object [] values )
		{
			return tt.AddRow( TableSectionId.Header, cellFunc, values );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static IActiveTableTemplate AddBodyRow( this IActiveTableTemplate tt, CellFunc cellFunc, params object [] values )
		{
			return tt.AddRow( TableSectionId.Body, cellFunc, values );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static IActiveTableTemplate AddFooterRow( this IActiveTableTemplate tt, CellFunc cellFunc, params object [] values )
		{
			return tt.AddRow( TableSectionId.Footer, cellFunc, values );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static IActiveTableTemplate AddHeaderRow( this IActiveTableTemplate tt, params object [] values )
		{
			return tt.AddRow( TableSectionId.Header, null, values );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static IActiveTableTemplate AddBodyRow( this IActiveTableTemplate tt, params object [] values )
		{
			return tt.AddRow( TableSectionId.Body, null, values );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static IActiveTableTemplate AddFooterRow( this IActiveTableTemplate tt, params object [] values )
		{
			return tt.AddRow( TableSectionId.Footer, null, values );
		}
	}

}

