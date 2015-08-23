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
using System.Diagnostics;
using System.Linq;


namespace SharpHtml {

	//Func<int, int, Tag, Tag> modify

	//
	// any returned tag will be wrapped by the 'tag' arguments
	//
	public delegate Tag CellFunc( int row, int column, Tag tag );

	//
	// modify the StylesDictionary or return styles to be merged; to totally replace
	// sd.Clear() then return or add styles
	//
	public delegate IEnumerable<string> StylesFunc( int column, StylesDictionary sd );


	/////////////////////////////////////////////////////////////////////////////

	public enum TableSectionId { Header, Body, Footer }


	public partial class TableTemplateBase {

		// ******
		public string TemplateClassId { get; set; } = "";
		public Action<Table> OnCreate { get; set; } = null;

		public int TemplateUseCount { get; set; } = 0;

		// ******
		public List<string> Widths { get; private set; } = new List<string> { };

		// ******
		public IEnumerable<string> TableStylesAndAttributes { get; set; }
		public string Caption { get; set; } = "";
		public string Id { get; set; } = "";
		public int Border { get; set; } = 0;

		public List<StylesDictionaryList> HeaderStyles { get; private set; }
		public List<StylesDictionaryList> BodyStyles { get; private set; }
		public List<StylesDictionaryList> FooterStyles { get; private set; }

		public List<List<Th>> HeaderRows { get; private set; }
		public List<List<Td>> BodyRows { get; private set; }
		public List<List<Td>> FooterRows { get; private set; }

		/////////////////////////////////////////////////////////////////////////////
		//
		//
		//
		/////////////////////////////////////////////////////////////////////////////

		protected List<StylesDictionaryList> GetStylesList( TableSectionId id )
		{
			switch( id ) {
				case TableSectionId.Header:
					return HeaderStyles;

				case TableSectionId.Body:
					return BodyStyles;

				case TableSectionId.Footer:
					return FooterStyles;

				default:
					throw new Exception( "unknow TableSectionId" );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		protected TableTemplateBase SetDefStyles( TableSectionId id, int nColumns, StylesFunc stylesFunc, IEnumerable<string> styles )
		{
			// ******
			var stylesList = GetStylesList( id);

			// ******
			if( nColumns < 1 ) {
				throw new ArgumentOutOfRangeException( nameof( nColumns ) );
			}

			// ******
			StylesDictionaryList newDictList;
			stylesList.Add( newDictList = new StylesDictionaryList { } );

			var templateDict = styles.ToStylesDictionary( false );
			// add single style string

			for( int index = 0; index < nColumns; index += 1 ) {
				var newDict = templateDict.Clone<StylesDictionary>();
				newDictList.Add( newDict );

				if( null != stylesFunc ) {
					var result = stylesFunc( index, newDict );
					if( null != result && result.Count() > 0 ) {
						newDict.Merge( result.ToStylesDictionary( false ), false );
					}
				}
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		protected TableTemplateBase AddStyles( TableSectionId id, IEnumerable<string> [] defs )
		{
			var stylesList = GetStylesList( id);

			// ******
			//
			// new row of styles
			//
			StylesDictionaryList newStyles;
			//
			// stored here
			//
			stylesList.Add( newStyles = new StylesDictionaryList { } );

			// ******
			if( null == defs || 0 == defs.Length ) {
				return this;
			}

			// ******
			//
			// each IEnumerable<string> in defs is added as a style dictionary
			//
			foreach( var items in defs ) {
				if( null == items || 0 == items.Count() ) {
					newStyles.Add( new StylesDictionary { } );
				}
				else {
					newStyles.Add( items.ToDictionary<StylesDictionary>() );
				}
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		//
		//
		/////////////////////////////////////////////////////////////////////////////

		public TableTemplateBase AddDataRow( TableSectionId id, CellFunc cellFunc, params object [] values )
		{
			// ******
			var items = new List<Tuple<Type, object>> { };

			foreach( var value in values ) {
				if( value is string ) {
					items.Add( new Tuple<Type, object>( typeof( string ), value ) );
				}
				else if( value is IEnumerable<string> ) {
					items.Add( new Tuple<Type, object>( typeof( IEnumerable<string> ), value ) );
				}
				else if( value is CellFunc ) {
					items.Add( new Tuple<Type, object>( typeof( CellFunc ), value ) );
				}
				else {
					throw new Exception( $"AddRow( params object [] values ) expected either strings and/or IEnumerable<string>s, found: {value.GetType().Name}" );
				}
			}

			// ******
			switch( id ) {
				case TableSectionId.Header:
					_addDataRow( HeaderRows, cellFunc, items );
					break;

				case TableSectionId.Body:
					_addDataRow( BodyRows, cellFunc, items );
					break;

				case TableSectionId.Footer:
					_addDataRow( FooterRows, cellFunc, items );
					break;

				default:
					throw new Exception( "unknow TableSectionId" );
			}

			// ******
			return this;
		}


		///////////////////////////////////////////////////////////////////////////////

		protected void _addDataRow<T>( List<List<T>> rows, CellFunc cellFunc, List<Tuple<Type, object>> items )
			where T : Tag, new()
		{
			// ******
			var row = new List<T> { };
			rows.Add( row );

			// ******
			foreach( var item in items ) {
				Type type = item.Item1;
				object value = item.Item2;

				// ******
				T tag;
				row.Add( tag = new T { } );

				if( null == value ) {
					continue;
				}

				// ******
				if( typeof( string ) == type ) {
					tag.SetValue( value.ToString() );
				}
				else if( typeof( CellFunc ) == type ) {
					var valueCellFunc = value as CellFunc;
					//
					// note: cellFunc() must set value
					//
					var wrapThisTag = valueCellFunc( rows.Count - 1, row.Count - 1, tag );
					if( null != wrapThisTag ) {
						tag.AddChild( wrapThisTag );
					}
				}
				else {
					var strs = value as IEnumerable<string>;
					var strValue = strs.First();
					var attrAndStyles = strs.Skip( 1 );
					tag.Initialize( strValue, string.Empty, string.Empty, attrAndStyles );
				}

				if( null != cellFunc ) {
					var wrapThisTag = cellFunc( rows.Count - 1, row.Count - 1, tag );
					if( null != wrapThisTag ) {
						tag.AddChild( wrapThisTag );
					}
				}
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public TableTemplateBase Clone()
		{
			return new TableTemplateBase( this );
		}


		/////////////////////////////////////////////////////////////////////////////

		public IActiveTableTemplate CreateTemplateInstance()
		{
			//
			// ActiveTableTemplate(this) calls TableTemplateBase( TableTemblateBase ) below
			//
			return new ActiveTableTemplate( this );
		}


		/////////////////////////////////////////////////////////////////////////////

		protected TableTemplateBase( TableTemplateBase ttb )
		{
			// ******
			TemplateClassId = ttb.TemplateClassId;
			OnCreate = ttb.OnCreate;

			// ******
			TableStylesAndAttributes = new List<string>( ttb.TableStylesAndAttributes );
			Caption = ttb.Caption;
			Id = ttb.Id;
			Border = ttb.Border;

			// ******
			Widths = ttb.Widths;
			HeaderStyles = ttb.HeaderStyles.Clone();
			BodyStyles = ttb.BodyStyles.Clone();
			FooterStyles = ttb.FooterStyles.Clone();

			// ******
			HeaderRows = ttb.HeaderRows.Clone();
			BodyRows = ttb.BodyRows.Clone();
			FooterRows = ttb.FooterRows.Clone();

			// ******
			if( Widths.Count > 0 ) {
				//
				// must have a row of data of at least Widths.Count
				//
				//if( 0 == HeaderRows.Count ) {
				//	HeaderRows.Add( new List<Th> { } );
				//}

				//var headerRowData = HeaderRows[ 0 ];
				//while( headerRowData.Count < Widths.Count ) {
				//	headerRowData.Add( new Th { } );
				//}

				// ******
				//
				// also require header styles of at least Widths.Count
				//
				if( 0 == HeaderStyles.Count ) {
					HeaderStyles.Add( new StylesDictionaryList { } );
				}

				var styleRow = HeaderStyles[ 0 ];
				while( styleRow.Count < Widths.Count ) {
					styleRow.Add( new StylesDictionary { } );
				}

				// ******
				for( int i = 0; i < Widths.Count; i += 1 ) {
					var widthStr = Widths[i];
					if( !string.IsNullOrWhiteSpace( widthStr ) ) {
						styleRow [ i ].Add( "width", widthStr );
					}
				}
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public TableTemplateBase( params string [] tableStylesAndAttributes )
		{
			// ******
			TableStylesAndAttributes = tableStylesAndAttributes;

			// ******
			HeaderStyles = new List<StylesDictionaryList> { };
			BodyStyles = new List<StylesDictionaryList> { };
			FooterStyles = new List<StylesDictionaryList> { };

			//HeaderContents = new List<List<string>> { };
			//BodyContents = new List<List<string>> { };
			//FooterContents = new List<List<string>> { };

			HeaderRows = new List<List<Th>> { };
			BodyRows = new List<List<Td>> { };
			FooterRows = new List<List<Td>> { };
		}
	}
}
