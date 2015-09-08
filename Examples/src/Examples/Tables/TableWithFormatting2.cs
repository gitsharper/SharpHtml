using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SharpHtml;

using static SharpHtml.TableTemplate;

namespace Examples {

	class TableWithFormatting2 : TableExamplesBase {

		const string About = "Same as TableWithSomeFormatting1() except we've combined the styles with the data and added a bit of color for negative values in the last column. Unlike TableWithSomeFormatting1 the sytles are defined inline.";

		/////////////////////////////////////////////////////////////////////////////

		#region code
		public Tuple<string, string> Build()
		{
			// ******
			var tagList = new TagList { };
			tagList.AddChild( new QuickTag( "h4" )
					.SetValue( About ) );

			// ******
			var table = new Table( caption: "Money Flow", border: 0, id: "", attrAndStyles: "background : ivory" );
			tagList.AddChild( table );

			// ******
			foreach( var item in AccountSummaryData.Create() ) {
				table.AddBodyRow(
						Content( item.Col1.ToString( "dd MMM yyyy" ), "width : 100px", "text-align : right" ),
						Content( item.Col2.ToString(), "width : 100px", "text-align : right" ),
						Content( item.Col3.ToString(), "width : 100px", "text-align : right" ),
						Content( item.Col4.ToString(), "width : 100px", "text-align : right" ),
						Content( item.Col5.ToString(), "width : 100px", "text-align : right" ),
						Content( item.Col6.ToString(), "width : 100px", "text-align : right" ),
						Content( item.Col7.ToString(), "width : 100px", "text-align : right", item.Col7 < 0 ? "color : red" : "" )
				);
			}

			// ******
			return new Tuple<string, string>( Render( tagList ), About );
		}
		#endregion
	}
}
