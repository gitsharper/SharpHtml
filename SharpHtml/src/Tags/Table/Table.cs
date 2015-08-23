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
using System.Threading.Tasks;
//using Newtonsoft.Json;

namespace SharpHtml {

	// table model
	// 
	// <table>
	// 	
	// 	<caption>
	// 	</caption>
	// 
	// 	<colgroup>
	// 		<col>
	// 		<col>
	// 	</colgroup>
	// 
	// 	<thead>
	// 		<tr>
	// 			<th> caption </th>
	// 		</tr>
	// 	</thead>
	// 
	// 	<tfoot>
	// 		<tr>
	// 			<td> compiled by xxx </td>
	// 		</tr>
	// 	</tfoot>
	// 
	// 	<tbody>
	// 		<tr>
	// 			<td> item </td> <td> item 2 </td>
	// 		</tr>
	// 	</tbody>
	// 
	// 
	// </table>

	/*
		this is not to expose my ignorance of html 5 but I note to following for my own
		into

						table.AddStyleBlock( "tr", StyleBlockAddAs.Id, "border-bottom : 1px solid black" );

							#someId tr {
								border-bottom : 1px solid black;
							}

		will not work unless 

						"border-collapse: collapse",

		if you want spacing between cells then:

						"border-collapse: separate",
							
							and

						"border-spacing : 55px",




	*/


	/////////////////////////////////////////////////////////////////////////////

	public partial class Table : Tag {	//, Table {
		protected override string _TagName { get { return "table"; } }
		public override StyleMode StyleMode { get { return StyleMode.Ignore; } }

		// ******
		//bool wrapData = true;

		// ******
		public TableTemplateBase AssociatedTemplate { get; private set; }

		// ******
		public Action<Table> OnCreate { get; set; } = null;

		// ******
		//
		// styles for each column in header, body, footer; they
		// are rendered into "#table thead|tbody|tfoot tr th|td:nth-child(index)"
		// where index is the column (1 based - first column is '1')
		//
		// table templates allow adding of multiple rows of styles, but we
		// only use the first one - when you add multiple line of data to
		// the header/body/footer using template routines the styles added
		// are inline
		//
		public StylesDictionaryList HeaderStyles { get; private set; }
		public StylesDictionaryList BodyStyles { get; private set; }
		public StylesDictionaryList FooterStyles { get; private set; }

		// ******
		public Caption Caption { get; private set; }
		public Colgroup ColGroup { get; private set; }
		public Thead THead { get; private set; }
		public Tbody TBody { get; private set; }
		public Tfoot TFoot { get; private set; }

		///////////////////////////////////////////////////////////////////////////////
		//
		//public Table SetCaption( string caption, string formatStr = "" )
		//{
		//	Caption.SetValue( caption, formatStr );
		//	return this;
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//public Table SetBorder( int border )
		//{
		//	AddAttribute( "border", border < 0 ? 0 : border, true );
		//	return this;
		//}
		//
		//
		///////////////////////////////////////////////////////////////////////////////
		//
		//public Table SetStyleClass( string className )
		//{
		//	StyleClassName = className;
		//	return this;
		//}
		//

		/////////////////////////////////////////////////////////////////////////////

		public Table SetCaption( string caption )
		{
			Caption.SetValue( caption );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public new Table SetId( string id )
		{
			base.SetId( id );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Table SetBorder( string border )
		{
			int value;
			if( int.TryParse( border, out value ) ) {
				AddAttribute( "border", value < 0 ? 0 : value, true );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Table SetOnCreate( Action<Table> action )
		{
			OnCreate = action;
			return this;
		}


		///////////////////////////////////////////////////////////////////////////////
		//
		//List<string> Widths = new List<string> { };
		//
		//public Table SetColumnWidths( params string [] widths )
		//{
		//	// ******
		//	Widths.Clear();
		//
		//	for( int index = 0; index < widths.Length; index += 1 ) {
		//		var value = widths[ index ];
		//		Widths.Add( value ?? "0" );
		//	}
		//
		//	// ******
		//	return this;
		//}


		/////////////////////////////////////////////////////////////////////////////

		protected StylesDictionaryList GetStylesList( TableSectionId id )
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

		public Table SetDefaultStyles( TableSectionId id, StylesFunc stylesFunc, int nColumns, params string [] styles )
		{
			// ******
			var stylesList = GetStylesList( id );
			//
			// single row of styles (unlike templates)
			//
			if( stylesList.Count > 0 ) {
				throw new Exception( $"styles list for {id.ToString()} already has style columns" );
			}
			//stylesList.Clear();

			// ******
			if( nColumns < 1 ) {
				throw new ArgumentOutOfRangeException( nameof( nColumns ) );
			}

			// ******
			var templateDict = styles.ToStylesDictionary( false );

			for( int index = 0; index < nColumns; index += 1 ) {
				var newDict = templateDict.Clone<StylesDictionary>();
				stylesList.Add( newDict );

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

		public Table AddStyles( TableSectionId id, params IEnumerable<string> [] styles )
		{
			// ******
			var stylesList = GetStylesList( id);
			//
			// single row of styles (unlike templates)
			//
			if( stylesList.Count > 0 ) {
				throw new Exception( $"styles list for {id.ToString()} already has style columns" );
			}
			//stylesList.Clear();

			// ******
			if( null == styles || 0 == styles.Length ) {
				return this;
			}

			// ******
			//
			// each IEnumerable<string> in defs is added as a style dictionary
			//
			foreach( var items in styles ) {
				if( null == items || 0 == items.Count() ) {
					stylesList.Add( new StylesDictionary { } );
				}
				else {
					stylesList.Add( items.ToDictionary<StylesDictionary>() );
				}
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Table AddRow( TableSectionId id, CellFunc cellFunc, params object [] values )
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
					_addDataRow( THead, cellFunc, items );
					break;

				case TableSectionId.Body:
					_addDataRow( TBody, cellFunc, items );
					break;

				case TableSectionId.Footer:
					_addDataRow( TFoot, cellFunc, items );
					break;

				default:
					throw new Exception( "unknow TableSectionId" );
			}

			// ******
			return this;
		}

		protected void _addDataRow<T>( TableHelperT<T> rows, CellFunc cellFunc, List<Tuple<Type, object>> items )
		where T : Tag, new()
		{
			// ******
			rows.AddRow();

			foreach( var item in items ) {
				Type type = item.Item1;
				object value = item.Item2;

				// ******
				var tag = rows.AddEmptyElement();

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
					var wrapThisTag = valueCellFunc( rows.Children.Count - 1, rows.CurrentRow.Children.Count - 1, tag );
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

				// ******
				if( null != cellFunc ) {
					var wrapThisTag = cellFunc( rows.Children.Count - 1, rows.CurrentRow.Children.Count - 1, tag );
					if( null != wrapThisTag ) {
						tag.AddChild( wrapThisTag );
					}
				}

			}
		}

		
		/////////////////////////////////////////////////////////////////////////////

		//
		// use this to get our entries in the <style></style> block, used when the
		// table is being used by Nmp (or other environment) where we build up a
		// table but are not useing the Html, or Style class
		//

		public string GetStyles()
		{
			var style = new Style { };
			style.RenderStyles( this );
			return style.GetSylesWithoutTag();
		}



		/////////////////////////////////////////////////////////////////////////////

		protected override string RenderContent()
		{
			// ******
			//
			// if there are header style defined (include widths) then
			// make sure there are enough enties to match - add empty ones
			// if required
			//
			if( HeaderStyles.Count > 0 ) {
				THead.AssureRowAndColumns( HeaderStyles.Count );
			}

			if( !THead.RowsHaveChildren() ) {
				Children.Remove( THead );
			}

			//if( !TBody.RowsHaveChildren() ) {
			//	Children.Remove( TBody );
			//}

			if( !TFoot.RowsHaveChildren() ) {
				Children.Remove( TFoot );
			}

			// ******
			return base.RenderContent();
		}


		/////////////////////////////////////////////////////////////////////////////

		enum TableStyle { Header, Body, Footer }

		void HeadBodyFooterStyles( TableStyle styleType )
		{
			// ******
			string fmtStr;
			StylesDictionaryList styles;

			switch( styleType ) {
				case TableStyle.Header:
					styles = HeaderStyles;
					fmtStr = "thead tr th:nth-child({0})";
					break;
				case TableStyle.Footer:
					styles = FooterStyles;
					fmtStr = "tfoot tr td:nth-child({0})";
					break;
				default:
				case TableStyle.Body:
					styles = BodyStyles;
					fmtStr = "tbody tr td:nth-child({0})";
					break;
			}
			// ******
			var count = styles.Count;
			for( int index = 0; index < count; index += 1 ) {
				if( styles [ index ].Count > 0 ) {
					//
					// sometimes an empty style block will be added so that a column 
					// will be represented (without styles) to keep the nth-child() order
					// correct - there is no reason to actually add that empty block of
					// styles as long as we assign the correct index to nth-child()
					//
					AddStyleBlock( StyleBlockAddAs.Class, string.Format( fmtStr, 1 + index ), styles [ index ] );
				}
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public override void BuildUpStyles()
		{
			// ******
			//
			// only do this once for any given template
			//
			if( !FromTemplate || 0 == AssociatedTemplate.TemplateUseCount ) {
				//var styleName = StyleClassName;

				AddStyleBlock( StyleBlockAddAs.Class, "", this.Styles );
				//this.Styles.Clear();

				HeadBodyFooterStyles( TableStyle.Header );
				HeadBodyFooterStyles( TableStyle.Body );
				HeadBodyFooterStyles( TableStyle.Footer );

				if( FromTemplate ) {
					AssociatedTemplate.TemplateUseCount += 1;
				}
			}

			// ******
			//
			// iterate children
			//
			base.BuildUpStyles();
		}


		/////////////////////////////////////////////////////////////////////////////

		public override string ToString()
		{
			return string.Format( "table: caption = \"{0}\"", Caption.Value.Render() );
		}


		/////////////////////////////////////////////////////////////////////////////

		void DefaultInitialize( string caption )
		{
			// ******
			HeaderStyles = new StylesDictionaryList { };
			BodyStyles = new StylesDictionaryList { };
			FooterStyles = new StylesDictionaryList { };

			// ******
			Children.AddChild( Caption = new Caption( caption ) );
			Children.AddChild( ColGroup = new Colgroup { } );
			Children.AddChild( THead = new Thead { } );
			Children.AddChild( TBody = new Tbody { } );
			Children.AddChild( TFoot = new Tfoot { } );
		}


		/////////////////////////////////////////////////////////////////////////////

		public Table( TableTemplateBase tt )
		{
			// ******
			DefaultInitialize( tt.Caption );

			// ******
			FromTemplate = true;
			AssociatedTemplate = tt;
			//
			// must use this to have a unique style name
			//
			StyleClassName = tt.TemplateClassId;
			//BumpTemplateUseCount = BumpTemplateUse;

			// ******
			SetId( tt.Id );
			AddAttribute( "border", tt.Border < 0 ? 0 : tt.Border, true );
			AddAttributesAndStyles( tt.TableStylesAndAttributes );

			// ******
			//
			// user can define multiple lines of styles but we're only using the first
			//
			if( tt.HeaderStyles.Count > 0 ) {
				HeaderStyles = tt.HeaderStyles.First();
			}

			if( tt.BodyStyles.Count > 0 ) {
				BodyStyles = tt.BodyStyles.First();
			}

			if( tt.FooterStyles.Count > 0 ) {
				FooterStyles = tt.FooterStyles.First();
			}

			// ******
			THead.Children.Clear();
			foreach( var row in tt.HeaderRows ) {
				THead.AddRow( row );
			}

			TBody.Children.Clear();
			foreach( var row in tt.BodyRows ) {
				TBody.AddRow( row );
			}

			TFoot.Children.Clear();
			foreach( var row in tt.FooterRows ) {
				TFoot.AddRow( row );
			}

			if( null != tt.OnCreate ) {
				tt.OnCreate( this );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public Table()
			: this( string.Empty )
		{
		}


		/////////////////////////////////////////////////////////////////////////////

		public Table( string caption, int border = 0, string id = "", params string [] attrAndStyles )
			: this( caption, border, id, (IEnumerable<string>) attrAndStyles )
		{
		}


		/////////////////////////////////////////////////////////////////////////////

		public Table( string caption, int border = 0, string id = "", IEnumerable<string> attrAndStyles = null )
		{
			// ******
			DefaultInitialize( caption );

			// ******
			SetId( id );
			AddAttributesAndStyles( attrAndStyles );
			AddAttribute( "border", border < 0 ? 0 : border );

			if( null != OnCreate ) {
				OnCreate( this );
			}
		}

	}




}
