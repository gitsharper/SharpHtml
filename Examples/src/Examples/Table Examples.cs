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


	/////////////////////////////////////////////////////////////////////////////

	public class TableExamples {

		/////////////////////////////////////////////////////////////////////////////


		TagList GetSource( string src, string callerName )
		{
			const string REGION = "#region";
			const string ENDREGION = "#endregion";

			// ******
			var searchStr = $"{REGION} {callerName}";
			var indexStart = src.IndexOf( searchStr );
			if( indexStart < 0 ) {
				return new TagList( null, new QuickTag( "div" ).SetValue( $"unable to locate method {callerName}" ) );
			}

			var indexEnd = src.IndexOf( ENDREGION, indexStart );
			var length = (indexEnd - indexStart) + ENDREGION.Length;

			var code = src.Substring( indexStart, length ).Replace( "\t", "  ");

			var codeTag = new QuickTag( "code", null, "data-language = csharp" )
						.SetValue( code );

			var preTag = new QuickTag( "pre");
			preTag.AppendChildren( codeTag );

			var tagList = new TagList( null, preTag);

			return tagList;
		}


		/////////////////////////////////////////////////////////////////////////////

		string srcPath;
		string src;

		void _render( TagList tags, string pathToSource, string callerName )
		{
			// ******
			if( null == srcPath || pathToSource != srcPath ) {
				srcPath = pathToSource;
				src = File.ReadAllText( srcPath );
			}
			var srcTags = GetSource( src, callerName );

			// ******
			var html = new Html { };

			// ******
			//
			// https://craig.is/making/rainbows
			// https://github.com/ccampbell/rainbow
			//
			html.Head.AddLink( "assets/css/code.css", "stylesheet" );

			html.Head.AddScript( "assets/rainbow/rainbow.min.js" )
				.SetTagAlign( TagFormatOptions.Horizontal );

			html.Head.AddScript( "assets/rainbow/language/csharp.js" )
				.SetTagAlign( TagFormatOptions.Horizontal );

			// ******
			html.Body.AppendChildren( tags );
			html.Body.AppendChildren( srcTags );
			var result = html.Render();

			// ******
			File.WriteAllText( $"..\\..\\src\\examples\\output\\{callerName}.html", result );
		}


		/////////////////////////////////////////////////////////////////////////////

		void Render( TagList tags, [CallerFilePath] string pathToSource = "", [CallerMemberName] string callerName = "" )
		{
			_render( tags, pathToSource, callerName );
		}

		/////////////////////////////////////////////////////////////////////////////

		void Render( Tag tag, [CallerFilePath] string pathToSource = "", [CallerMemberName] string callerName = "" )
		{
			_render( new TagList( null, tag ), pathToSource, callerName );
		}


		/////////////////////////////////////////////////////////////////////////////

		#region TransactionsTable
		public void TransactionsTable()
		{
			// ******
			var tagList = new TagList { };
			tagList.AddChild( new QuickTag( "h3" ).SetValue( "Bunch of Identically Ugly Tables" ) );

			// ******
			var categories = TransactionData.Create();
			foreach( var category in categories ) {
				var table = new Table( caption : category.Key, border : 1, id : "", attrAndStyles : "" );
				tagList.AddChild( table );

				foreach( var entry in category ) {
					table.AddBodyRow( entry.Date.ToString(), entry.Amount.ToString(), entry.Detail );
				}

			}

			// ******
			Render( tagList );
		}
		#endregion




		/////////////////////////////////////////////////////////////////////////////

		#region TableWithTemplate2

		//
		// this is more complex than TableWithTemplate1() because we're making use
		// of callbacks to modify the display of certain cells
		//

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


		public void TableWithTemplate2()
		{
			// ******
			var tagList = new TagList { };

			tagList.AddChild( new QuickTag( "h4" )
				.SetValue( "Using the same template as TableWithTemplate1() but we're going to play around with the data as we load it" )
			);
			//tagList.AddChild( new UserTag( "pre" )
			//	.SetValue( "<code> ( row, index, tag ) => null </code>" )
			//);

			// ******
			var instance = _moneyFlow.CreateTemplateInstance();
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
					(CellFunc) ( ( row, col, tag ) => {
						tag.SetValue( item.Col4.ToString( "n" ) );
						if( item.Col4 < 0 ) {
							tag.AddStyles( "color : red" );
						}
						return null;
					} ),

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
			Render( tagList );
		}
		#endregion


		/////////////////////////////////////////////////////////////////////////////

		#region TableWithTemplate1
		//
		// define as static, can use many times
		//
		static IDefineTableTemplate _moneyFlow =
			DefineTableTemplate.Create(
						"moneyFlow", "table-layout: fixed", "border-collapse: collapse", "margin: 0px", "padding : 0", "background : ivory" )
				.SetColumnWidths( "125px", "100px", "100px", "100px", "100px", "100px", "100px" )
				.SetDefaultHeaderStyles( 7, "text-align : center" )
				.AddHeaderRow( Content( "", "colspan = 4" ), Content( $"4 month moving average", "colspan = 3" ) )
				.AddHeaderRow( "Ending Date", "In", "Out", "Diff", "Moving Avg In", "Moving Avg Out", "Moving Avg Diff" )
				.SetDefaultBodyStyles( 7, "text-align : right", "padding-right : 5px" )
				.SetDefaultFooterStyles( 7 )
				.AddFooterRow( Content( "strikethrough values (<s>814.22</s>) indicate incomplete data for the months moving average", "colspan = 7", "color : gray", "padding-left : 10px" ) )
				.SetOnCreate( table =>
				{
					table.AddStyleBlock( StyleBlockAddAs.Class, "td, th", "border : 1px solid black" );
				})
			;

		public void TableWithTemplate1()
		{
			// ******
			var tagList = new TagList { };
			tagList.AddChild( new QuickTag( "h4" )
					.SetValue( "Same as TableWithSomeFormatting3() only using a template, a little more verbose but usable many times" ) );

			// ******
			var instance = _moneyFlow.CreateTemplateInstance();
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
			Render( tagList );
		}
		#endregion


		/////////////////////////////////////////////////////////////////////////////

		#region TableWithSomeFormatting3
		public void TableWithSomeFormatting3()
		{
			// ******
			var tagList = new TagList { };
			tagList.AddChild( new QuickTag( "h4" )
					.SetValue( "Same as TableWithSomeFormatting2() with some headers added" ) );

			// ******
			var table = new Table( );
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
			Render( tagList );
		}
		#endregion


		/////////////////////////////////////////////////////////////////////////////

		#region TableWithSomeFormatting2
		public void TableWithSomeFormatting2()
		{
			// ******
			var tagList = new TagList { };
			tagList.AddChild( new QuickTag( "h4" )
					.SetValue( "Same as TableWithSomeFormatting1() except we've combined the styles with the data and added a bit of color for negative values in the last column." ) );

			// ******
			var table = new Table( caption : "Money Flow", border : 0, id : "", attrAndStyles : "background : ivory" );
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
			Render( tagList );
		}
		#endregion


		/////////////////////////////////////////////////////////////////////////////

		#region TableWithSomeFormatting1
		public void TableWithSomeFormatting1()
		{
			// ******
			var tagList = new TagList { };
			tagList.AddChild( new QuickTag( "h4" )
					.SetValue( "Some formatting: adding styles to the body columns and then loading data separately" ) );

			// ******
			var table = new Table( caption : "Money Flow", border : 0, id : "", attrAndStyles : "background : ivory" );
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
			Render( tagList );
		}
		#endregion


		/////////////////////////////////////////////////////////////////////////////

		#region TableWithData
		public void TableWithData()
		{
			// ******
			var table = new Table( caption : "Checking Account", border : 0, id : "", attrAndStyles : "" );

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
			Render( table );
		}
		#endregion


		/////////////////////////////////////////////////////////////////////////////

		#region CreateTable
		public void CreateTable()
		{
			var table = new Table( caption : "Checking Account", border : 0, id : "", attrAndStyles : "width : 90%" );
			Render( table );
		}
		#endregion


	}

}
