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

	public static partial class TableTemplate {


		/////////////////////////////////////////////////////////////////////////////
		//
		// using static SharpHtml.TableTemplate;
		//
		/////////////////////////////////////////////////////////////////////////////

		static IEnumerable<string> Values( string [] values )
		{
			var list = new List<string> { };

			foreach( var value in values ) {
				if( string.IsNullOrEmpty( value ) ) {
					list.Add( string.Empty );
				}
				else if( value.Contains( '&' ) ) {
					var split = value.Split( new char [] { '&' }, StringSplitOptions.RemoveEmptyEntries );
					list.AddRange( split );
				}
				else {
					list.Add( value );
				}
			}

			return list;
		}


		/////////////////////////////////////////////////////////////////////////////

		public static IEnumerable<string> Content( params string [] values )
		{
			return Values( values );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static IEnumerable<string> SA( params string [] values )
		{
			return Values( values );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static IEnumerable<string> ContentSA( params string [] values )
		{
			return Values( values );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static IEnumerable<string> Styles( params string [] values )
		{
			return Values( values );
		}


		/////////////////////////////////////////////////////////////////////////////

		public static IEnumerable<string> EmptySA()
		{
			return new string [ 0 ];
		}



		//		/////////////////////////////////////////////////////////////////////////////
		//		//
		//		// Table.Set-Header || Body || Footer-Styles
		//		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table SetDefaultHeaderStyles( this Table tt, StylesFunc stylesFunc, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Header, stylesFunc, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table SetDefaultHeaderStyles( this Table tt, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Header, null, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table SetDefaultBodyStyles( this Table tt, StylesFunc stylesFunc, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Body, stylesFunc, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table SetDefaultBodyStyles( this Table tt, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Body, null, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table SetDefaultFooterStyles( this Table tt, StylesFunc stylesFunc, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Footer, stylesFunc, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table SetDefaultFooterStyles( this Table tt, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Footer, null, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//		//
		//		// Table.Add-Header || Body || Footer-Styles
		//		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table AddHeaderStyles( this Table tt, params IEnumerable<string> [] styles )
		//		{
		//			return tt.AddStyles( TableSectionId.Header, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table AddBodyStyles( this Table tt, params IEnumerable<string> [] styles )
		//		{
		//			return tt.AddStyles( TableSectionId.Body, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table AddFooterStyles( this Table tt, params IEnumerable<string> [] styles )
		//		{
		//			return tt.AddStyles( TableSectionId.Footer, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//		//
		//		// Table.Add-Header || Body || Footer-Row
		//		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table AddHeaderRow( this Table tt, CellFunc cellFunc, params string [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Header, cellFunc, values );
		//		}
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table AddBodyRow( this Table tt, CellFunc cellFunc, params string [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Body, cellFunc, values );
		//		}
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table AddFooterRow( this Table tt, CellFunc cellFunc, params string [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Footer, cellFunc, values );
		//		}
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table AddHeaderRow( this Table tt, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Header, null, values );
		//		}
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table AddBodyRow( this Table tt, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Body, null, values );
		//		}
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static Table AddFooterRow( this Table tt, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Footer, null, values );
		//		}
		//
		//
		//
		//
		//
		//
		//
		//
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//		//
		//		// IDefineTableTemplate.Set-Header || Body || Footer-Styles
		//		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate SetDefaultHeaderStyles( this IDefineTableTemplate tt, StylesFunc stylesFunc, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Header, stylesFunc, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate SetDefaultHeaderStyles( this IDefineTableTemplate tt, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Header, null, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate SetDefaultBodyStyles( this IDefineTableTemplate tt, StylesFunc stylesFunc, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Body, stylesFunc, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate SetDefaultBodyStyles( this IDefineTableTemplate tt, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Body, null, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate SetDefaultFooterStyles( this IDefineTableTemplate tt, StylesFunc stylesFunc, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Footer, stylesFunc, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate SetDefaultFooterStyles( this IDefineTableTemplate tt, int nColumns, params string [] styles )
		//		{
		//			return tt.SetDefaultStyles( TableSectionId.Footer, null, nColumns, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//		//
		//		// IDefineTableTemplate.Add-Header || Body || Footer-Styles
		//		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate AddHeaderStyles( this IDefineTableTemplate tt, params IEnumerable<string> [] styles )
		//		{
		//			return tt.AddStyles( TableSectionId.Header, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate AddBodyStyles( this IDefineTableTemplate tt, params IEnumerable<string> [] styles )
		//		{
		//			return tt.AddStyles( TableSectionId.Body, styles );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate AddFooterStyles( this IDefineTableTemplate tt, params IEnumerable<string> [] styles )
		//		{
		//			return tt.AddStyles( TableSectionId.Footer, styles );
		//		}
		//
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//		//
		//		// IDefineTableTemplate.Add-Header || Body || Footer-Row
		//		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate AddHeaderRow( this IDefineTableTemplate tt, CellFunc cellFunc, params string [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Header, cellFunc, values );
		//		}
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate AddBodyRow( this IDefineTableTemplate tt, CellFunc cellFunc, params string [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Body, cellFunc, values );
		//		}
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate AddFooterRow( this IDefineTableTemplate tt, CellFunc cellFunc, params string [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Footer, cellFunc, values );
		//		}
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate AddHeaderRow( this IDefineTableTemplate tt, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Header, null, values );
		//		}
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate AddBodyRow( this IDefineTableTemplate tt, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Body, null, values );
		//		}
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IDefineTableTemplate AddFooterRow( this IDefineTableTemplate tt, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Footer, null, values );
		//		}
		//
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//		//
		//		//  IActiveTableTemplate
		//		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IActiveTableTemplate AddHeaderRow( this IActiveTableTemplate tt, CellFunc cellFunc, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Header, cellFunc, values );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IActiveTableTemplate AddBodyRow( this IActiveTableTemplate tt, CellFunc cellFunc, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Body, cellFunc, values );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IActiveTableTemplate AddFooterRow( this IActiveTableTemplate tt, CellFunc cellFunc, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Footer, cellFunc, values );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IActiveTableTemplate AddHeaderRow( this IActiveTableTemplate tt, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Header, null, values );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IActiveTableTemplate AddBodyRow( this IActiveTableTemplate tt, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Body, null, values );
		//		}
		//
		//		/////////////////////////////////////////////////////////////////////////////
		//
		//		public static IActiveTableTemplate AddFooterRow( this IActiveTableTemplate tt, params object [] values )
		//		{
		//			return tt.AddRow( TableSectionId.Footer, null, values );
		//		}


	}

}

