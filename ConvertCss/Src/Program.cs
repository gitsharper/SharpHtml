using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using JpmUtilities;

namespace ConvertCss {

	/////////////////////////////////////////////////////////////////////////////

	class Program {

		/////////////////////////////////////////////////////////////////////////////

		public static void Write( string str )
		{
			Console.Write( str );
		}


		/////////////////////////////////////////////////////////////////////////////

		static void Convert( string text )
		{
			// ******
			var reader = new StringIndexer( text );

			while( !reader.AtEnd) {

			// first need comment cutter






			}

		}


		/////////////////////////////////////////////////////////////////////////////

		static void Main( string [] args )
		{
			if( args.Length < 1 ) {
				throw new ArgumentException( "not enought arguments" );
			}

			var path = args [ 1 ];

			if( !File.Exists( path ) ) {
				throw new ArgumentException( $"\"{path}\" does not exists" );
			}

			try {
				var text = File.ReadAllText( path );
				Convert( text );
			}
			catch( Exception ex ) {
				throw;
			}
		}

	}
}
