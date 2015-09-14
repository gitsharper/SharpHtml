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


	/////////////////////////////////////////////////////////////////////////////

	public class MediaBlock : StylesheetRule {

		public StylesheetRuleType RuleType => StylesheetRuleType.MediaBlock;
		public string Condition { get; private set; } = "*** uninitialized ***";

		List<RuleBlock> Rules = new List<RuleBlock> { };


		/////////////////////////////////////////////////////////////////////////////

		public MediaBlock Add( RuleBlock rb )
		{
			Rules.Add( rb );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public MediaBlock Add( IEnumerable<RuleBlock> blocks )
		{
			foreach( var block in blocks ) {
				Add( block );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public MediaBlock Add( params RuleBlock [] blocks )
		{
			return Add( (IEnumerable<RuleBlock>) blocks );
		}


		/////////////////////////////////////////////////////////////////////////////
		
		//
		// problem is we're going to want to add more than one rule block and we'll
		// have lost the MediaBlock instance for chaining
		//

		public RuleBlock AddRuleBlock( string target )
		{
			var rb = new RuleBlock( target );
			Add( rb );
			return rb;
		}
		
		
		/////////////////////////////////////////////////////////////////////////////

		public string Render()
		{
			// ******
			var sb = new StringBuilder { };

			// ******
			sb.AppendLine( $"@media {Condition} {{" );

			// ******
			foreach( var rule in Rules ) {
				sb.Append( Stylesheet.Indent( rule.Render(), 1 ) );
			}

			// ******
			sb.AppendLine( "}" );
			sb.AppendLine();

			// ******
			return sb.ToString();
		}


		/////////////////////////////////////////////////////////////////////////////


		public MediaBlock( string condition )
		{
			Condition = string.IsNullOrWhiteSpace( condition ) ? "*** bad name ***" : condition.Trim();
		}

	}


	/////////////////////////////////////////////////////////////////////////////

	public class RuleBlock : IStyles<RuleBlock>, StylesheetRule {

		public StylesheetRuleType RuleType => StylesheetRuleType.RuleBlock;

		// ******
		public string Target { get; set; } = "";
		public List<string> ClassesOrIds { get; } = new List<string> { };

		// ******
		StylesDictionary Styles = new StylesDictionary { };


		/////////////////////////////////////////////////////////////////////////////

		public StringBuilder Render( StringBuilder sb )
		{
			// ******
			int counter = 0;
			foreach( var item in ClassesOrIds ) {
				if( counter > 0 ) {
					sb.Append( ", " );
				}
				sb.Append( item );
				counter += 1;
			}

			sb.Append( (counter > 0 ? " " : "") + Target );

			// ******
			sb.AppendLine( " {" );

			// ******
			foreach( var style in Styles ) {
				if( HtmlItemsDictionary.COMMENT_STR == style.Key ) {
					sb.AppendLine( $"/* {style.Value} */" );
				}
				else {
					sb.AppendLine( $"  {style.Key} : {style.Value};" );
				}
			}

			// ******
			sb.AppendLine( "}" );
			sb.AppendLine();

			// ******
			return sb;
		}


		/////////////////////////////////////////////////////////////////////////////

		public string Render()
		{
			// ******
			if( string.IsNullOrWhiteSpace( Target ) && 0 == ClassesOrIds.Count ) {
				return string.Empty;
			}

			// ******
			var sb = new StringBuilder { };
			return Render( sb ).ToString();
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		// start with: a letter, '-', or '_'
		//
		// or start with a '.' or '#' since were bound to forget and leave it in
		// for class, or id
		//
		// contains and ends with: letters, digits, '-', or '_'
		//
		// css actually allows pretty much anything if you encode it, but for the
		// sake of simplicity we only allow these - anyone finds an error please notify
		//

		const string regExIdOrClassMatch = "^[a-zA-Z0-9-_.#]*?[a-zA-Z0-9-_]$";
		static Regex rxIdOrClassMatch;

		protected void ValidateIdentifier( string id )
		{
			// ******
			if( null == rxIdOrClassMatch ) {
				rxIdOrClassMatch = new Regex( regExIdOrClassMatch );
			}

			// ******
			//if( !rxIdOrClassMatch.IsMatch( id.Trim() ) ) {
			//	throw new ArgumentException( $"\"{id}\" is not a valid identifier" );
			//}
		}

		/////////////////////////////////////////////////////////////////////////////

		public RuleBlock SetTarget( string target )
		{
			Target = target;
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		string PrependStr( string ppStr, string str )
		{
			if( str.StartsWith( ppStr ) ) {
				return str;
			}
			return ppStr + str;
		}


		/////////////////////////////////////////////////////////////////////////////

		public RuleBlock AddId( string id, params string [] ids )
		{
			// ******
			ValidateIdentifier( id );
			ClassesOrIds.Add( PrependStr( "#", id ) );

			// ******
			foreach( var item in ids ) {
				ValidateIdentifier( item );
				ClassesOrIds.Add( PrependStr( "#", item ) );
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public RuleBlock AddClass( string className, params string [] classNames )
		{
			// ******
			ValidateIdentifier( className );
			ClassesOrIds.Add( PrependStr( ".", className ) );

			// ******
			foreach( var item in classNames ) {
				ValidateIdentifier( item );
				ClassesOrIds.Add( PrependStr( ".", item ) );
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public RuleBlock()
		{
		}


		/////////////////////////////////////////////////////////////////////////////

		public RuleBlock( string target )
		{
			this.Target = target.Trim();
		}






		/////////////////////////////////////////////////////////////////////////////

		public RuleBlock AddComment( string comment )
		{
			Styles.AddComment( comment );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public RuleBlock ReplaceStyle( string key, string value )
		{
			Styles.ReplaceStyle( key, value );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public RuleBlock AddStyle( string key, string value )
		{
			Styles.AddStyle( key, value );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public RuleBlock AddStyles( IEnumerable<string> styles )
		{
			Styles.AddStyles( styles );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public RuleBlock AddStyles( params string [] styles )
		{
			return AddStyles( (IEnumerable<string>) styles );
		}


		/////////////////////////////////////////////////////////////////////////////

		public RuleBlock Clone()
		{
			var sb = (RuleBlock) MemberwiseClone();
			foreach( var kvp in Styles ) {
				sb.Styles.Add( kvp.Key, kvp.Value );
			}
			return sb;
		}

	}

}
