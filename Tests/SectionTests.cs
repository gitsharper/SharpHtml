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
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;
using JpmUtilities;
using NmpHtml;

namespace HtmlHelpersTests {

	public class SectionTests {

		/////////////////////////////////////////////////////////////////////////////

		void AddDataToSection<T>( Section<T> section ) where T : Tag, new()
		{
			section.Add( "col 1" );
			section.Add( "col 2" );
			section.Add( "col 3" );
			section.Add( "col 4" );
			section.Add( new FuncRenderer( () => "via func" ) );

			section.AddRow();

			section.Add( "col 1" );
			section.Add( "col 2" );
			section.Add( "col 3" );
			section.Add( "col 4" );

			section.AddRow();

			//section.AddMany( (object) "a string", "length" );
		}


		/////////////////////////////////////////////////////////////////////////////

		[Fact]
		public void TestSection1()
		{
			// ******
			var section = new Section<Td>( new Tbody { } );

			AddDataToSection( section );

			var result = section.RenderSection();


			Assert.NotNull( result );

			File.WriteAllText( @"..\..\lastTestResults.html", result );


			//Assert.NotNull( section );
		}


		/////////////////////////////////////////////////////////////////////////////

		[Fact]
		public void NewSection()
		{
			var section = new Section<Td>( new Tbody { } );
			Assert.NotNull( section );
		}

	}

}
