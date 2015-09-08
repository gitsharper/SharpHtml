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

	class TableWithTemplate1 : TableExamplesBase {

		const string About = "Same as TableWithSomeFormatting3() only using a template, a little more verbose but usable many times";

		/////////////////////////////////////////////////////////////////////////////

		#region code
		public Tuple<string, string> Build()
		{
			// ******
			var tagList = new TagList { };
			tagList.AddChild( new QuickTag( "h4" )
					.SetValue( About ) );

			// ******
			var instance = Templates.MoneyFlowTemplate.CreateTemplateInstance();
			foreach( var item in AccountSummaryData.Create() ) {
				instance.AddBodyRow(
					item.Col1.ToString( "dd MMM yyyy" ),
					item.Col2.ToString( "n" ),
					item.Col3.ToString( "n" ),
					item.Col4.ToString( "n" ),
					item.Col5.ToString( "n" ),
					item.Col6.ToString( "n" ),
					item.Col7.ToString( "n" )
				);
			}

			// ******
			tagList.AddChild( instance.CreateTable() );
			return new Tuple<string, string>( Render( tagList ), About );
		}
		#endregion
	}
}
