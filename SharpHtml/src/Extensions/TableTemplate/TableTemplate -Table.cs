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
		// Table.Set-Header || Body || Footer-Styles
		//
		/////////////////////////////////////////////////////////////////////////////

		public static Table SetDefaultHeaderStyles( this Table tt, StylesFunc stylesFunc, int nColumns, params string [] styles )
		{
			return tt.SetDefaultStyles( TableSectionId.Header, stylesFunc, nColumns, styles );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static Table SetDefaultHeaderStyles( this Table tt, int nColumns, params string [] styles )
		{
			return tt.SetDefaultStyles( TableSectionId.Header, null, nColumns, styles );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static Table SetDefaultBodyStyles( this Table tt, StylesFunc stylesFunc, int nColumns, params string [] styles )
		{
			return tt.SetDefaultStyles( TableSectionId.Body, stylesFunc, nColumns, styles );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static Table SetDefaultBodyStyles( this Table tt, int nColumns, params string [] styles )
		{
			return tt.SetDefaultStyles( TableSectionId.Body, null, nColumns, styles );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static Table SetDefaultFooterStyles( this Table tt, StylesFunc stylesFunc, int nColumns, params string [] styles )
		{
			return tt.SetDefaultStyles( TableSectionId.Footer, stylesFunc, nColumns, styles );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static Table SetDefaultFooterStyles( this Table tt, int nColumns, params string [] styles )
		{
			return tt.SetDefaultStyles( TableSectionId.Footer, null, nColumns, styles );
		}

		/////////////////////////////////////////////////////////////////////////////
		//
		// Table.Add-Header || Body || Footer-Styles
		//
		/////////////////////////////////////////////////////////////////////////////

		public static Table AddHeaderStyles( this Table tt, params IEnumerable<string> [] styles )
		{
			return tt.AddStyles( TableSectionId.Header, styles );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static Table AddBodyStyles( this Table tt, params IEnumerable<string> [] styles )
		{
			return tt.AddStyles( TableSectionId.Body, styles );
		}

		/////////////////////////////////////////////////////////////////////////////

		public static Table AddFooterStyles( this Table tt, params IEnumerable<string> [] styles )
		{
			return tt.AddStyles( TableSectionId.Footer, styles );
		}

		/////////////////////////////////////////////////////////////////////////////
		//
		// Table.Add-Header || Body || Footer-Row
		//
		/////////////////////////////////////////////////////////////////////////////

		public static Table AddHeaderRow( this Table tt, CellFunc cellFunc, params string [] values )
		{
			return tt.AddRow( TableSectionId.Header, cellFunc, values );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static Table AddBodyRow( this Table tt, CellFunc cellFunc, params string [] values )
		{
			return tt.AddRow( TableSectionId.Body, cellFunc, values );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static Table AddFooterRow( this Table tt, CellFunc cellFunc, params string [] values )
		{
			return tt.AddRow( TableSectionId.Footer, cellFunc, values );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static Table AddHeaderRow( this Table tt, params object [] values )
		{
			return tt.AddRow( TableSectionId.Header, null, values );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static Table AddBodyRow( this Table tt, params object [] values )
		{
			return tt.AddRow( TableSectionId.Body, null, values );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static Table AddFooterRow( this Table tt, params object [] values )
		{
			return tt.AddRow( TableSectionId.Footer, null, values );
		}

	}

}

