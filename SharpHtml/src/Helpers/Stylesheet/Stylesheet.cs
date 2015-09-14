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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////

	public enum StylesheetRuleType { RuleBlock, MediaBlock }

	public interface StylesheetRule {
		StylesheetRuleType RuleType { get; }
		string Render();
	}

	/////////////////////////////////////////////////////////////////////////////

	public class Stylesheet {

		List<StylesheetRule> Rules { get; } = new List<StylesheetRule> { };

		// Render  				
		// if( COMMENT_STR == key) {
		//		continue;
		//	}


		/////////////////////////////////////////////////////////////////////////////

		public static string Indent( string text, int nIndents )
		{
			// ******
			if( nIndents <= 0 ) {
				return text;
			}

			// ******
			return text.Indent( Tag.StdIndentStr, nIndents );
		}


		/////////////////////////////////////////////////////////////////////////////

		public string Render()
		{
			var sb = new StringBuilder { };

			foreach( var block in Rules ) {
				sb.Append( block.Render() );
			}

			return sb.ToString();
		}


		/////////////////////////////////////////////////////////////////////////////

		public Stylesheet Add( StylesheetRule sb )
		{
			Rules.Add( sb );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Stylesheet Add( IEnumerable<StylesheetRule> blocks )
		{
			foreach( var block in blocks ) {
				Add( block );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Stylesheet Add( params StylesheetRule [] blocks )
		{
			return Add( (IEnumerable<StylesheetRule>) blocks );
		}

	}

}
