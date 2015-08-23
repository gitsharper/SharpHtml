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

	public class ActiveTableTemplate : TableTemplateBase, IActiveTableTemplate {

		/////////////////////////////////////////////////////////////////////////////

		public IActiveTableTemplate AddRow( TableSectionId id, CellFunc cellFunc, params object [] values )
		{
			base.AddDataRow( id, cellFunc, values );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Table CreateTable( string caption = "", int? border = null, string id = "", IEnumerable<string> attrAndstyles = null )
		{
			// ******
			if( !string.IsNullOrWhiteSpace( caption ) ) {
				Caption = caption;
			}

			if( border.HasValue ) {
				Border = border.Value;
			}

			if( !string.IsNullOrWhiteSpace( id ) ) {
				Id = id;
			}

			if( null != attrAndstyles ) {
				TableStylesAndAttributes = TableStylesAndAttributes.Append( attrAndstyles );
			}

			// ******
			return new Table( this );
		}


		/////////////////////////////////////////////////////////////////////////////

		public ActiveTableTemplate( TableTemplateBase template )
			: base( template )
		{
		}


	}



}

