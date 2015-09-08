using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpHtml;
using static SharpHtml.TableTemplate;

namespace Examples {
	
	public static class Templates {

		//
		// define as static, can use many times
		//
		public static IDefineTableTemplate MoneyFlowTemplate =
			DefineTableTemplate.Create(
						"moneyFlow", "table-layout: fixed", "border-collapse: collapse", "margin: 0px", "padding : 0", "background : ivory" )
				.SetColumnWidths( "125px", "100px", "100px", "100px", "100px", "100px", "100px" )
				.SetDefaultHeaderStyles( 7, "text-align : center" )
				.AddHeaderRow( Content( "", "colspan = 4" ), Content( $"4 month moving average", "colspan = 3" ) )
				.AddHeaderRow( "Ending Date", "In", "Out", "Diff", "Moving Avg In", "Moving Avg Out", "Moving Avg Diff" )
				.SetDefaultBodyStyles( 7, "text-align : right", "padding-right : 5px" )
				.SetDefaultFooterStyles( 7 )
				.AddFooterRow( Content( "strikethrough values (<s>814.22</s>) indicate incomplete data for the months moving average", "colspan = 7", "color : gray", "padding-left : 10px" ) )
				.SetOnCreate( table => {
					table.AddStyleBlock( StyleBlockAddAs.Class, "td, th", "border : 1px solid black" );
				} )
			;
	
			
		}


}
