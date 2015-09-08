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

	class TableWithTemplate2 : TableExamplesBase {

		const string About = "Same as TableWithSomeFormatting3() only using a template, a little more verbose but usable many times";

		/////////////////////////////////////////////////////////////////////////////

		#region code
		Tag ModifyTags( AccountSummaryData.Data data, int row, int column, Tag tag )
		{
			// ******
			Tag returnTag = null;

			// ******
			if( 0 == row && column > 3 ) {
				returnTag = new QuickTag( "s" );
				returnTag.Value = tag.Value;
				tag.Value = null;
			}

			// ******
			if( 6 == column && data.Col7 < 0 ) {
				tag.AddStyles( "color : red" );
			}

			// ******
			return returnTag;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tuple<string, string> Build()
		{
			// ******
			var tagList = new TagList { };

			tagList.AddChild( new QuickTag( "h4" )
				.SetValue( About )
			);

			// ******
			var instance = Templates.MoneyFlowTemplate.CreateTemplateInstance();
			foreach( var item in AccountSummaryData.Create() ) {
				instance.AddBodyRow(
					//
					// we can modify cells as they are added
					//
					( row, column, tag ) => ModifyTags( item, row, column, tag ),

					item.Col1.ToString( "dd MMM yyyy" ),
					item.Col2.ToString( "n" ),
					item.Col3.ToString( "n" ),

					//
					// we don't have to monitor each tag as it's created, we can 
					// modify the tag "inline" by passing a CellFunc as a callback,
					// note: we do have to add the data ourself
					//
					(CellFunc) (( row, col, tag ) => {
						tag.SetValue( item.Col4.ToString( "n" ) );
						if( item.Col4 < 0 ) {
							tag.AddStyles( "color : red" );
						}
						return null;
					}),

					item.Col5.ToString( "n" ),
					item.Col6.ToString( "n" ),

					//
					// yet another way to customize the adding of data, if we pass an IEnumerable<string>
					// the first string will be used for the content of the tag and the remaining
					// items are parsed as attributes and styles ("style : value", "attribute = value")
					//
					Content( item.Col7.ToString( "n" ), item.Col7 < 0 ? "color : red" : "" )
				);
			}

			// ******
			tagList.AddChild( instance.CreateTable() );
			return new Tuple<string, string>( Render( tagList ), About );
    }
		#endregion

	}
}
