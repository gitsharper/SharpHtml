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

	class TableWithFormatting3 : TableExamplesBase {

		const string About = "Same as TableWithSomeFormatting1 with some headers and some color. The styles are mostly in the style block, the red values in the last column are inline.";

		/////////////////////////////////////////////////////////////////////////////

		#region code
		public Tuple<string, string> Build()
		{
			// ******
			var tagList = new TagList { };
			tagList.AddChild( new QuickTag( "h4" )
					.SetValue( About ) );

			// ******
			var table = new Table();
			tagList.AddChild( table );

			table.AddAttributesAndStyles(
				"table-layout: fixed",
				"border-collapse: collapse",
				"border: 1px solid black",
				"margin: 0px",
				"padding : 0",
				"background : ivory"
				);

			table.AddHeaderRow( Content( "", "colspan = 4" ), Content( $"4 month moving average", "colspan = 3" ) )
				.AddHeaderRow( "Ending Date", "In", "Out", "Diff", "Moving Avg In", "Moving Avg Out", "Moving Avg Diff" );

			table.AddStyleBlock( StyleBlockAddAs.Class, "td, th", "border : 1px solid black" );

			table.AddBodyStyles();

			//
			// yet another way to set body styles; a StylesFunc allows the interception of columns as they are
			// added, parameters are the column number being added and the StylesDictionary that represents that
			// column, return an IEnumerable<string> of styles to merge, or manipulate the styles dictionary
			// itself
			//
			// breaking out the StyleFunction on it's own makes this look a lot less messy and more readable than
			// inserting the lambda directly into SetDefaultBodyStyles() - at least for the sake of an example
			//
			StylesFunc sf = ( col, sd ) => col > 3 ? Styles( "font-style : italic " ) : null;
			table.SetDefaultBodyStyles( sf, 7, "width : 100px", "text-align : right" );

			// ******
			foreach( var item in AccountSummaryData.Create() ) {
				table.AddBodyRow(
						item.Col1.ToString( "dd MMM yyyy" ),
						item.Col2.ToString(),
						item.Col3.ToString(),
						item.Col4.ToString(),
						item.Col5.ToString(),
						item.Col6.ToString(),
						Content( item.Col7.ToString(), item.Col7 < 0 ? "color : red" : null )
				);
			}

			// ******
			return new Tuple<string, string>( Render( tagList ), About );
		}
		#endregion

	}
}
