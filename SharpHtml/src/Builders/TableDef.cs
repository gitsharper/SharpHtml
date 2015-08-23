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
using System.Linq;
using Newtonsoft.Json;

namespace NmpHtml {


	/////////////////////////////////////////////////////////////////////////////


	public static class TableDefExtensions {

		/////////////////////////////////////////////////////////////////////////////

		//public static IEnumerable<string> GetStyleSelectorNames( this TableDef td )
		//{
		//	return Enum.GetNames( typeof( TableDefSelectors ) );
		//}


		/////////////////////////////////////////////////////////////////////////////


	
		/////////////////////////////////////////////////////////////////////////////

		//public static bool SetStyles( this TableDef tDef, string selectorIn, params string [] items )
		//{
		//	// ******
		//	if( null == tDef ) {
		//		throw new ArgumentNullException( "tDef" );
		//	}

		//	if( string.IsNullOrWhiteSpace(selectorIn) ) {
		//		throw new ArgumentNullException( "selectorIn" );
		//	}
	
		//	// ******
		//	TableDef.ItemsList il;
		//	switch( selectorIn ) {
		//		case "caption":
		//			il = tDef.CaptionStyles;
		//			break;

		//		case "table":
		//			il = tDef.TableAttributesStyles;
		//			break;

		//		case "thead":
		//			il = tDef.TheadStyles;
		//			break;

		//		case "tbody":
		//			il = tDef.TbodyStyles;
		//			break;

		//		case "tfoot":
		//			il = tDef.TfootStyles;
		//			break;

		//		default:
		//			return false;
		//	}

		//	if( null == il ) {
		//		return false;
		//	}
		//	else {
		//		il.AddRange( items );
		//	}

		//	// ******
		//	return true;
		//}


		/////////////////////////////////////////////////////////////////////////////

		//public static bool AddStyles( this TableDef tDef, string selectorIn, params string [] items )
		//{
		//	// ******
		//	if( null == tDef ) {
		//		throw new ArgumentNullException( "tDef" );
		//	}

		//	if( string.IsNullOrWhiteSpace( selectorIn ) ) {
		//		throw new ArgumentNullException( "selectorIn" );
		//	}

		//	// ******
		//	TableDef.ListOfItemsList listOf;
		//	switch( selectorIn ) {
		//		case "header":
		//			listOf = tDef.HeaderStyles;
		//			break;

		//		case "body":
		//			listOf = tDef.BodyStyles;
		//			break;

		//		case "footer":
		//			listOf = tDef.FooterStyles;
		//			break;

		//		default:
		//			return false;
		//	}

		//	if( null == listOf ) {
		//		return false;
		//	}
		//	else {
		//		listOf.Add( items );
		//	}

		//	// ******
		//	return true;
		//}


	}


	public enum TableDefTypeofElement {
		ItemsList,
		ListOfItemsList,
	}

	public enum TableDefSetSelectors {
		Caption,	// styles for captions - single set of styles
		Table,		// attributes and styles - both extracted from single set
		Thead,		// styles for <thead> - single set of styles
		Tbody,		// styles for <tbody> - single set of styles
		Tfoot,		// styles for <tfoog> - single set of styles
	}

	public enum TableDefAddSelectors {
		Header,		// styles for <hhead> columns - one set of styles for each column
		Body,			// styles for <tbody> columns - one set of styles for each column
		Footer,		// styles for <tbody> columns - one set of styles for each column
	}

	/////////////////////////////////////////////////////////////////////////////

	[JsonObject( MemberSerialization.OptIn )]
	public class TableDef {

		/////////////////////////////////////////////////////////////////////////////

		public class ItemsList : List<string> { }

		public class ListOfItemsList : List<ItemsList> {
			public void Add( IEnumerable<string> items )
			{
				ItemsList list;
				base.Add( list = new ItemsList { } );
				list.AddRange( items );
			}
		}

		// ******
		[JsonProperty]
		public ItemsList CaptionStyles { get; set; }

		[JsonProperty]
		public ItemsList TableAttributesStyles { get; set; }

		//
		// styles for the containers: thead, tbody, tfoot; 
		// mainly used to set fonts/color/etc. for all the
		// contained th/td
		//
		[JsonProperty]
		public ItemsList TheadStyles { get; set; }

		[JsonProperty]
		public ItemsList TbodyStyles { get; set; }

		[JsonProperty]
		public ItemsList TfootStyles { get; set; }

		// ******
		//
		// for head|body|foot [ ["", "",""], ["", "", ""] ]
		//
		// an array of string lists where each list represents styles for
		// a single column of the table, instances for: head, body, and footer
		//
		[JsonProperty]
		public ListOfItemsList HeaderStyles { get; set; }

		[JsonProperty]
		public ListOfItemsList BodyStyles { get; set; }

		[JsonProperty]
		public ListOfItemsList FooterStyles { get; set; }

		//
		// where each ItemList represents the text data and any possible
		// attributes/styles
		//
		// generally use these to set override styles on a header/body/footer
		// entry - can also be used for static text like headers and footers
		//
		// the first entry of each ItemList is the text value that will be
		// set on entry, the remainder are attribute/style entries that are
		// set on the individual data entries
		//
		public ListOfItemsList HeadData { get; set; }
		public ListOfItemsList BodyData { get; set; }
		public ListOfItemsList FooterData { get; set; }


		/////////////////////////////////////////////////////////////////////////////

		public bool AddData( string selector, params string[] items )
		{
			// ******
			if( string.IsNullOrWhiteSpace( selector ) ) {
				throw new ArgumentNullException( "selector" );
			}

			// ******
			ListOfItemsList listOf;
			switch( selector.ToLower() ) {
				case "header":
					listOf = HeadData;
					break;

				case "body":
					listOf = BodyData;
					break;

				case "footer":
					listOf = FooterData;
					break;

				default:
					return false;
			}

			if( null == listOf ) {
				return false;
			}

			// ******
			listOf.Add( items );
			return true;
		}
		
		/////////////////////////////////////////////////////////////////////////////

		public bool SetStyles( string selectorIn, params string [] items )
		{
			// ******
			if( string.IsNullOrWhiteSpace( selectorIn ) ) {
				throw new ArgumentNullException( "selectorIn" );
			}

			if( selectorIn.Length < 2 ) {
				return false;
			}

			TableDefSetSelectors selector;
			var name = char.ToUpper( selectorIn.First() ) + selectorIn.Substring( 1 ).ToLower();
			if( !Enum.TryParse( name, out selector ) ) {
				return false;
			}

			// ******
			ItemsList il;
			switch( selector ) {
				case TableDefSetSelectors.Caption:
					il = CaptionStyles;
					break;

				case TableDefSetSelectors.Table:
					il = TableAttributesStyles;
					break;

				case TableDefSetSelectors.Thead:
					il = TheadStyles;
					break;

				case TableDefSetSelectors.Tbody:
					il = TbodyStyles;
					break;

				case TableDefSetSelectors.Tfoot:
					il = TfootStyles;
					break;

				default:
					return false;
			}

			// ******
			if( null == il ) {
				return false;
			}

			// ******
			il.AddRange( items );
			return true;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool AddStyles( string selectorIn, params string [] items )
		{
			// ******
			if( string.IsNullOrWhiteSpace( selectorIn ) ) {
				throw new ArgumentNullException( "selectorIn" );
			}

			if( selectorIn.Length < 2 ) {
				return false;
			}

			TableDefAddSelectors selector;
			var name = char.ToUpper( selectorIn.First() ) + selectorIn.Substring( 1 ).ToLower();
			if( !Enum.TryParse( name, out selector ) ) {
				return false;
			}

			// ******
			ListOfItemsList listOf;
			switch( selector ) {
				case TableDefAddSelectors.Header:
					listOf = HeaderStyles;
					break;

				case TableDefAddSelectors.Body:
					listOf = BodyStyles;
					break;

				case TableDefAddSelectors.Footer:
					listOf = FooterStyles;
					break;

				default:
					return false;
			}

			if( null == listOf ) {
				return false;
			}

			// ******
			listOf.Add( items );
			return true;
		}

	
		/////////////////////////////////////////////////////////////////////////////

		public TableDef()
		{
			CaptionStyles = new ItemsList { };
			TableAttributesStyles = new ItemsList { };
			TheadStyles = new ItemsList { };
			TbodyStyles = new ItemsList { };
			TfootStyles = new ItemsList { };

			HeaderStyles = new ListOfItemsList { };
			BodyStyles = new ListOfItemsList { };
			FooterStyles = new ListOfItemsList { };
		}

	}
}