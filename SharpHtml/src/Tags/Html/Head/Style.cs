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

namespace SharpHtml {

	/*
	 * 
	 * <style type="text/css" >
	 * 
	 *	#someTag {
	 *		attribute : value;
	 *	}
	 * 
	 * </style>
	 * 
	 */

	/////////////////////////////////////////////////////////////////////////////

	public class Style : Tag {
		protected override string _TagName { get { return "style"; } }

		//enum TableStyle { Header, Body, Footer }

		// ******
		protected Dictionary<string, StylesDictionary> blockStyles { get; private set; }


		/////////////////////////////////////////////////////////////////////////////

		public void Add( string name, StylesDictionary styles )
		{
			// ******
			if( string.IsNullOrWhiteSpace( name ) ) {
				throw new ArgumentNullException( "name" );
			}

			if( null == styles ) {
				throw new ArgumentNullException( "styles" );
			}

			// ******
			var key = name;
			blockStyles [ key ] = styles;
		}


		/////////////////////////////////////////////////////////////////////////////

		public void Add( string name, IEnumerable<string> styles )
		{
			Add( name, styles.ToStylesDictionary() );
		}


		/////////////////////////////////////////////////////////////////////////////

		string GenerateStyleBlockId( Tag tag )
		{
			return SC.HASH + tag.Id;
		}


		/////////////////////////////////////////////////////////////////////////////

		protected void IterateTagStyles( Tag tag )
		{
			// ******
			foreach( var child in tag.Children ) {
				if( typeof( Style ) == child.GetType() ) {
					//
					// skip ourself
					//
					continue;
				}

				// ******
				if( StyleMode.IncludeInStyles == child.StyleMode && child.Styles.Count > 0 ) { 
					var styleName = GenerateStyleBlockId( child );
					Add( styleName, child.Styles );
				}

				if( child.StyleBlocks.Count > 0 ) {
					//
					// we use to generate the first part of a rule name
					// (#id) and prepend it to the block name
					//
					// it's now up to the tag.BuildUpStyles() to generate
					// the full name
					//
					foreach( var block in child.StyleBlocks ) {
						Add( block.Name, block );
					}
				}

				if( child.Children.Count > 0 ) {
					IterateTagStyles( child );
				}
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public void RenderStyles( Tag tag )
		{
			// ******
			//
			// allows an tag to generate it's own styles, or styles for it's children,
			// Tag.Render( TagRenderMode ) will add inline styles to the tag, Style.IterateTagStyles()
			// (above) will add styles that need to be added to the in-page <style> block
			//
			tag.BuildUpStyles();

			// ******
			blockStyles.Clear();
			IterateTagStyles( tag );
		}


		/////////////////////////////////////////////////////////////////////////////

		public string GetSylesWithoutTag()
		{
			return RenderContent();
		}

		/////////////////////////////////////////////////////////////////////////////

		protected override string RenderContent()
		{
			var sb = new StringBuilder { };

			foreach( var blockStyle in blockStyles ) {
				sb.AppendFormat( "{0} {{", blockStyle.Key );
				sb.AppendLine();

				var sbInner = new StringBuilder { };
				foreach( var style in blockStyle.Value ) {
					sbInner.AppendFormat( "{0} : {1};", style.Key, style.Value );
					sbInner.AppendLine();
				}

				sb.Append( Indent( sbInner.ToString(), 1 ) );

				sb.AppendFormat( "}}" );
				sb.AppendLine();
			}

			Styles.Render( sb, true );

			return sb.ToString();
		}


		/////////////////////////////////////////////////////////////////////////////

		public Style()
		{
			AddAttribute( "type", "text/css" );
			blockStyles = new Dictionary<string, StylesDictionary> { };
		}


		/////////////////////////////////////////////////////////////////////////////

		//public Td( IRender value, string formatStr, string id = "", IEnumerable<string> styles = null )
		//	: base( value, formatStr, id, styles )
		//{
		//}

	}
}
