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

	class TableWithData : TableExamplesBase {

		const string About = "Unformatted table with data";

		/////////////////////////////////////////////////////////////////////////////

		#region code
		public Tuple<string, string> Build()
		{
			// ******
			var tagList = new TagList { };

			tagList.AddChild( new QuickTag( "h4" )
				.SetValue( About )
			);
			
			// ******
			var table = new Table( caption: "Checking Account", border: 0, id: "", attrAndStyles: "" );
			tagList.AddChild( table );

			// ******
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
