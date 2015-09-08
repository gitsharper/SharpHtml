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

	public class TableExamplesBase {

		/////////////////////////////////////////////////////////////////////////////


		TagList GetSource( string src, string callerName )
		{
			const string REGION = "#region code";
			const string ENDREGION = "#endregion";

			// ******
			//var searchStr = $"{REGION} {callerName}";
			var indexStart = src.IndexOf( REGION );
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

		string _render( TagList tags, string pathToSource, string saveFileName )
		{
			// ******
			if( null == srcPath || pathToSource != srcPath ) {
				srcPath = pathToSource;
				src = File.ReadAllText( srcPath );
			}
			var srcTags = GetSource( src, saveFileName );

			// ******
			var html = new Html { };

			// ******
			//
			// https://craig.is/making/rainbows
			// https://github.com/ccampbell/rainbow
			//
			html.Head.AddLink( "../Output/assets/css/code.css", "stylesheet" );

			html.Head.AddScript( "../Output/assets/rainbow/rainbow.min.js" )
				.SetTagAlign( TagFormatOptions.Horizontal );

			html.Head.AddScript( "../Output/assets/rainbow/language/csharp.js" )
				.SetTagAlign( TagFormatOptions.Horizontal );

			// ******
			html.Body.AppendChildren( tags );
			html.Body.AppendChildren( srcTags );
			var result = html.Render();

			// ******
			var outputFilePath = $"{Path.GetDirectoryName(pathToSource)}\\{saveFileName}.html";
			File.WriteAllText( outputFilePath, result );
			return outputFilePath;
    }


		/////////////////////////////////////////////////////////////////////////////

		public string Render( TagList tags, [CallerFilePath] string pathToSource = "", [CallerMemberName] string callerName = "" )
		{
			var frame = new StackTrace().GetFrame( 1 );
			return _render( tags, pathToSource, $"{frame.GetMethod().DeclaringType.Name}.{callerName}" );
		}

		/////////////////////////////////////////////////////////////////////////////

		public string Render( Tag tag, [CallerFilePath] string pathToSource = "", [CallerMemberName] string callerName = "" )
		{
			var frame = new StackTrace().GetFrame( 1 );
			return _render( new TagList( null, tag ), pathToSource, $"{frame.GetMethod().DeclaringType.Name}.{callerName}"  );
		}

	}

}
