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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using static SharpHtml.DefineTableTemplate;

namespace SharpHtml {


	/////////////////////////////////////////////////////////////////////////////

	public partial class DefineTableTemplate : TableTemplateBase, IDefineTableTemplate {


		/////////////////////////////////////////////////////////////////////////////

		private static int _nextTableTemplateId;

		protected static string NextTableTemplateClassId
		{
			get
			{
				return $"tblTemplate{_nextTableTemplateId++}";
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public IDefineTableTemplate SetCaption( string caption )
		{
			Caption = caption;
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public IDefineTableTemplate SetId( string id )
		{
			Id = id;
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public IDefineTableTemplate SetBorder( string border )
		{
			int value;
			if( int.TryParse( border, out value ) ) {
				Border = value;
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public IDefineTableTemplate SetOnCreate( Action<Table> action )
		{
			OnCreate = action;
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public IDefineTableTemplate SetColumnWidths( params string [] widths )
		{
			// ******
			Widths.Clear();

			for( int index = 0; index < widths.Length; index += 1 ) {
				var value = widths[ index ];
				Widths.Add( value ?? "0" );
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		//
		//
		/////////////////////////////////////////////////////////////////////////////

		public IDefineTableTemplate SetDefaultStyles( TableSectionId section, StylesFunc stylesFunc, int nColumns, params string [] styles )
		{
			base.SetDefStyles( section, nColumns, stylesFunc, styles );
			return this;
		}

		/////////////////////////////////////////////////////////////////////////////

		public new IDefineTableTemplate AddStyles( TableSectionId id, params IEnumerable<string> [] styles )
		{
			base.AddStyles( id, styles );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public IDefineTableTemplate AddRow( TableSectionId id, CellFunc cellFunc, params object [] values )
		{
			base.AddDataRow( id, cellFunc, values );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		protected DefineTableTemplate( string templateId, params string [] tableStylesAndAttributes )
			: base( tableStylesAndAttributes )
		{
			TemplateClassId = string.IsNullOrWhiteSpace( templateId ) ? NextTableTemplateClassId : templateId;
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		// DefineTableTemplate.Create()
		//
		/////////////////////////////////////////////////////////////////////////////

		public static IDefineTableTemplate Create( string templateId = "", params string [] tableStylesAndAttributes )
		{
			return new DefineTableTemplate( templateId, tableStylesAndAttributes );
		}

	}

}

