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

	class TableWithFormatting1 : TableExamplesBase {

		const string About = "Some formatting: adding styles to the body columns and then loading data separately. The styles are placed in the style block in the head section.";

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
			table.AddBodyStyles(
				Styles( "width : 100px", "text-align : right" ),
				Styles( "width : 100px", "text-align : right" ),
				Styles( "width : 100px", "text-align : right" ),
				Styles( "width : 100px", "text-align : right" ),
				Styles( "width : 100px", "text-align : right", "font-Style : italic" ),
				Styles( "width : 100px", "text-align : right", "font-Style : italic" ),
				Styles( "width : 100px", "text-align : right", "font-Style : italic" )
			);

			foreach( var item in AccountSummaryData.Create() ) {
				table.AddBodyRow(
						item.Col1.ToString( "dd MMM yyyy" ),
						item.Col2.ToString(),
						item.Col3.ToString(),
						item.Col4.ToString(),
						item.Col5.ToString(),
						item.Col6.ToString(),
						item.Col7.ToString()
				);
			}

			// ******
			return new Tuple<string, string>( Render( tagList ), About );
		}
		#endregion
	}
}
