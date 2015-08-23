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
using System.Text;
using Xunit;
//using Newtonsoft.Json;
//using JpmUtilities;
using SharpHtml;

namespace Tests {


	/////////////////////////////////////////////////////////////////////////////
	
	public class TableTests {

		///////////////////////////////////////////////////////////////////////////////
		//
		//[Fact]
		//public void TestBuildTable1()
		//{
		//	// ******
		//	var table = new Table { };
		//	var tableDef = TableDefData();
		//
		//	table.Caption.AddStyles( tableDef.CaptionStyles );
		//
		//	table.AddAttributesAndStyles( tableDef.TableAttributesStyles );
		//
		//	table.THead.AddStyles( tableDef.TheadStyles );
		//	table.TBody.AddStyles( tableDef.TbodyStyles );
		//	table.TFoot.AddStyles( tableDef.TfootStyles );
		//
		//	foreach( var styles in tableDef.HeaderStyles ) {
		//		table.HeaderStyles.Add( styles.ToStylesDictionary() );
		//	}
		//
		//	foreach( var styles in tableDef.BodyStyles ) {
		//		table.BodyStyles.Add( styles.ToStylesDictionary() );
		//	}
		//
		//	foreach( var styles in tableDef.FooterStyles ) {
		//		table.FooterStyles.Add( styles.ToStylesDictionary() );
		//	}
		//
		//
		//	var tableText = table.Render();
		//	var tableStyles = table.GetStyles();
		//
		//
		//	File.WriteAllText( @"..\..\lastTestResults.html", tableStyles + tableText );
		//}
		//
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//public TableDef TableDefData()
		//{
		//	var tableDef = new TableDef { };
		//
		//	tableDef.AddStyles( "header", "font-size : 22", "font-weight : bold" );
		//	tableDef.AddStyles( "header", "font-size : 32", "font-weight : bold" );
		//	tableDef.AddStyles( "header", "font-size : 42", "font-weight : bold" );
		//
		//	tableDef.AddStyles( "body", "font-size : 22", "font-weight : bold" );
		//	tableDef.AddStyles( "body", "font-size : 32", "font-weight : bold" );
		//	tableDef.AddStyles( "body", "font-size : 42", "font-weight : bold" );
		//
		//	tableDef.AddStyles( "footer", "font-size : 22", "font-weight : bold" );
		//	tableDef.AddStyles( "footer", "font-size : 32", "font-weight : bold" );
		//	tableDef.AddStyles( "footer", "font-size : 42", "font-weight : bold" );
		//
		//	return tableDef;
		//}
		//
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//[Fact]
		//public void TestTableDef1()
		//{
		//	var tableDef = TableDefData();
		//
		//	var result = JsonConvert.SerializeObject( tableDef, Formatting.Indented );
		//	var reconstituted = JsonConvert.DeserializeObject<TableDef>( result );
		//
		//	File.WriteAllText( @"..\..\lastTableDefTest.json", result );
		//
		//}
		//
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//void AddData( Table table )
		//{
		//	//table.ColGroup.Add()
		//	//	.AddStyle( "width", "100px" )
		//	//	.AddStyle( "background", "red" );
		//
		//	//table.ColGroup.Add()
		//	//	.AddStyle( "width", "200px" )
		//	//	.AddStyle( "background", "aqua" );
		//
		//	//table.ColGroup.Add()
		//	//	.AddStyle( "width", "300px" )
		//	//	.AddStyle( "background", "blue" )
		//	//	.AddStyle( "color", "white" );
		//
		//	//table.ColGroup.Add()
		//	//	.AddStyle( "background", "red" );
		//
		//	//table.THead.AddAttributesAndStyles( "font-size : 105%", "font-weight : 800", "color : red" );
		//	table.THead.AddStyles( "font-size : 105%", "font-weight : 800", "color : red" );
		//
		//	//table.AddHeaderStyles( "color : black", "font-size : 105%", "font-weight : 900" )
		//	//	.AddHeaderStyles( "color : black", "font-weight : bold" )
		//	//	.AddHeaderStyles( "color : black", "font-weight : bold" )
		//	//	.AddHeaderStyles( "color : black", "font-weight : bold" );
		//
		//	table.AddBodyStyles( "width : 100px", "background : red" )
		//		.AddBodyStyles( "width : 200px", "background : aqua" )
		//		.AddBodyStyles( "width : 300px", "background : lightblue" )
		//		.AddBodyStyles( "background : yellow" );
		//
		//	table.THead.Add( "First Column", "", "colspan = 2" );
		//	table.AddHeadMany( "Second Column", "Third Column", "Fourth Column" );
		//
		//	//
		//	// add to body via table methods
		//	///
		//	table.AddData( "Item 1", "", "color : white" );
		//	//
		//	// add to body via Tbody methods
		//	//
		//	table.TBody
		//		.AddMany( "Item 2", "Item 3", "Item 4" );
		//	table.TBody.AddRow()
		//		.AddMany( HtmlHelpers.CreateList( "Item 1", "Item 2", "Item 3", "Item 4" ) );
		//
		//
		//	table.TFoot.Add()
		//		.AddAttribute( "colspan", "2" )
		//		.SetValue( "Footer text" );
		//	table.AddFoot( "Some more" );
		//	table.AddFoot( "Third foot" );
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//[Fact]
		//public void TestTable5()
		//{
		//	var table = new Table( "My Table", 2 );
		//	AddData( table );
		//
		//	var styleText = table.GetStyles();
		//
		//	if( Debugger.IsAttached ) {
		//		Debugger.Break();
		//	}
		//
		//	File.WriteAllText( @"..\..\lastTestResults.html", styleText );
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//[Fact]
		//public void TestTable4()
		//{
		//
		//	var html = new Html( "Test", new List<string> { "background-color: lightgray" } ) { };
		//
		//	html.Head.Children.Add( new Meta( "description", "html test" ) );
		//	html.Head.Children.Add( new Meta( "author", "-jr-" ) );
		//
		//
		//	var table = new Table( "My Table", 2 );
		//	AddData( table );
		//	html.Body.Children.Add( table );
		//
		//	var result = html.Render();
		//
		//	//if( Debugger.IsAttached ) {
		//	//	Debugger.Break();
		//	//}
		//
		//	File.WriteAllText( @"..\..\lastTestResults.html", result );
		//
		//
		//	var json = JsonConvert.SerializeObject( table, Formatting.Indented );
		//	File.WriteAllText( @"..\..\lastTableDefTest.json", json );
		//
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//[Fact]
		//public void TestTable3()
		//{
		//	var table = new Table( "My Table", 2 );
		//	AddData( table );
		//
		//	var html = new Html( "Test", new List<string> { "background-color: lightgray" } ) { };
		//	html.Body.Children.Add( table );
		//
		//	var result = html.Render();
		//
		//	if( Debugger.IsAttached ) {
		//		Debugger.Break();
		//	}
		//
		//	File.WriteAllText( @"..\..\lastTestResults.html", result );
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//[Fact]
		//public void TestTable2()
		//{
		//	var table = new Table( "My Table", 2 );
		//	AddData( table );
		//	var result = table.Render();
		//
		//	if( Debugger.IsAttached ) {
		//		Debugger.Break();
		//	}
		//
		//	File.WriteAllText( @"..\..\lastTestResults.html", result );
		//}


		/////////////////////////////////////////////////////////////////////////////

		//[Fact]
		public void TestTable1()
		{
			var table = new Table( "My Table", 2 );

			var result = table.Render();

			if( Debugger.IsAttached ) {
				Debugger.Break();
			}

			File.WriteAllText( @"..\..\lastTestResults.html", result );
		}



	}

}
