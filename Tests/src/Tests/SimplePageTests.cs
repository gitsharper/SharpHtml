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
using Xunit;

using SharpHtml;

using static SharpHtml.TableTemplate;
//using SharpHtml.Simple;

namespace Tests {


	//
	// started creating tests with good intentions but they all turned out to be
	// examples of one sort or another so they were moved to the examples project
	//

	/////////////////////////////////////////////////////////////////////////////

	public class SimplePageTests {

		string srcPath;
		string src;

		///////////////////////////////////////////////////////////////////////////////
		//
		//TagList GetSource( string src, string callerName )
		//{
		//	const string REGION = "#region";
		//	const string ENDREGION = "#endregion";
		//
		//	// ******
		//	var searchStr = $"{REGION} {callerName}";
		//	var indexStart = src.IndexOf( searchStr );
		//	if( indexStart < 0 ) {
		//		return new TagList( null, new QuickTag( "div" ).SetValue( $"unable to locate method {callerName}" ) );
		//	}
		//
		//	var indexEnd = src.IndexOf( ENDREGION, indexStart );
		//	var length = (indexEnd - indexStart) + ENDREGION.Length;
		//
		//	var code = src.Substring( indexStart, length ).Replace( "\t", "  ");
		//
		//	var codeTag = new QuickTag( "code", null, "data-language = csharp" )
		//				.SetValue( code );
		//	var preTag = new QuickTag( "pre")
		//				.AppendChildren( codeTag );
		//
		//	var tagList = new TagList( null, preTag);
		//
		//	return tagList;
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//void _render( TagList tags, string pathToSource, string callerName )
		//{
		//	// ******
		//	if( null == srcPath || pathToSource != srcPath ) {
		//		srcPath = pathToSource;
		//		src = File.ReadAllText( srcPath );
		//	}
		//	var srcTags = GetSource( src, callerName );
		//
		//	// ******
		//	var html = new Html { };
		//
		//	// ******
		//	//
		//	// https://craig.is/making/rainbows
		//	// https://github.com/ccampbell/rainbow
		//	//
		//	html.Head.AddLink( "assets/css/code.css", "stylesheet" );
		//
		//	html.Head.AddScript( "assets/rainbow/rainbow.min.js" )
		//		.SetTagAlign( TagFormatOptions.Horizontal );
		//
		//	html.Head.AddScript( "assets/rainbow/language/csharp.js" )
		//		.SetTagAlign( TagFormatOptions.Horizontal );
		//
		//	// ******
		//	html.Body.AppendChildren( tags );
		//	html.Body.AppendChildren( srcTags );
		//	var result = html.Render();
		//
		//	// ******
		//	File.WriteAllText( $"..\\..\\src\\examples\\output\\{callerName}.html", result );
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//void Render( TagList tags, [CallerFilePath] string pathToSource = "", [CallerMemberName] string callerName = "" )
		//{
		//	_render( tags, pathToSource, callerName );
		//}
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//void Render( Tag tag, [CallerFilePath] string pathToSource = "", [CallerMemberName] string callerName = "" )
		//{
		//	_render( new TagList( null, tag ), pathToSource, callerName );
		//}


		/////////////////////////////////////////////////////////////////////////////

		void Render( Tag tag, [CallerMemberName] string callerName = "" )
		{
			var result = tag.Render();
			File.WriteAllText( $"..\\..\\src\\tests\\output\\{callerName}.html", result );
		}



		/////////////////////////////////////////////////////////////////////////////

		//[Fact]
		//public void CreatePage()
		//{
		//	var page = new SimplePage<SimpleHtml> { }
		//									.Initialize( "Test", "include/" );
		//	Render( page.Html );
		//}


	}

}
