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

namespace SharpHtml {


	/////////////////////////////////////////////////////////////////////////////

	//public interface ITableHelper {
	//
	//	// ******
	//	//Tr CurrentRow { get; }
	//
	//	// ******
	//	//Tag AddEmptyElement();
	//	//Tag AddElement( string text, params string [] attrAndStyles );
	//	//ITableHelper AddElements( IEnumerable<string> firstItem, params IEnumerable<string> [] attrAndStyles );
	//	//ITableHelper AddElementsRow( IEnumerable<string> firstItem, params IEnumerable<string> [] attrAndStyles );
	//	//ITableHelper AddElementsRow( string firstItem, params string [] items );
	//
	//	// ******
	//	//ITableHelper AddRow( string id = "", params string [] attrAndStyles );
	//
	//}


	/////////////////////////////////////////////////////////////////////////////

	// Thead / Tbody / Tfoot

	public abstract class TableHelperT<T> : Tag //, ITableHelper
		where T : Tag, new() {

		// ******
		public Tr CurrentRow
		{ get; private set; }


		/////////////////////////////////////////////////////////////////////////////

		public bool RowsHaveChildren()
		{
			foreach( var child in Children ) {
				if( child.Children.Count > 0 ) {
					return true;
				}
			}

			return false;
		}


		/////////////////////////////////////////////////////////////////////////////

		void AssureRow()
		{
			if( null == CurrentRow ) {
				AddRow();
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public void AssureRowAndColumns( int columns )
		{
			AssureRow();
			var countItemsToAdd = columns - CurrentRow.Children.Count;

			//
			// if there are no header columns defined they we add them as "hidden"
			//
			T tag = new T { };
			if( countItemsToAdd == columns ) {
				//
				// no header content defined at all
				//
				//tag.AddAttributesAndStyles( "border : none", "backround : none" );
				CurrentRow.AddAttributesAndStyles( "visibility : collapse" );
			}

			while( countItemsToAdd-- > 0 ) {
				CurrentRow.AddChild( tag );
			}
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		//
		/////////////////////////////////////////////////////////////////////////////

		public Tag AddEmptyElement()
		{
			AssureRow();
			return CurrentRow.AddNewChild<T>();
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddElement( string text, params string [] attrAndStyles )
		{
			AssureRow();
			return CurrentRow.AddNewChild<T>( text, attrAndStyles );
		}


		/////////////////////////////////////////////////////////////////////////////

		public TableHelperT<T> AddElements( IEnumerable<string> firstItem, params IEnumerable<string> [] attrAndStyles )
		{
			AssureRow();
			CurrentRow.AddNewChild<T>( firstItem, attrAndStyles );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		//
		// adds a row and append elements
		//

		public TableHelperT<T> AddElementsRow( IEnumerable<string> firstItem, params IEnumerable<string> [] attrAndStyles )
		{
			AddRow( string.Empty );
			return AddElements( firstItem, attrAndStyles );
		}


		/////////////////////////////////////////////////////////////////////////////

		public TableHelperT<T> AddElementsRow( string firstItem, params string [] items )
		{
			AddRow();
			AddElement( firstItem );
			foreach( var item in items ) {
				AddElement( item );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		//
		/////////////////////////////////////////////////////////////////////////////

		public TableHelperT<T> AddRow()
		{
			return AddRow( string.Empty );
		}


		/////////////////////////////////////////////////////////////////////////////

		//
		// adds a new row to Thead, Tbody or Tfoot
		//

		public TableHelperT<T> AddRow( string id, params string [] attrAndStyles )
		{
			CurrentRow = AddNewChild<Tr>( string.Empty, string.Empty, id, attrAndStyles );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		//
		// adds a new row and appends a bunch of elements
		//

		public TableHelperT<T> AddRow( List<T> items )
		{
			AddRow();
			CurrentRow.AppendChildren( items );// Children.AddRange( items );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		protected TableHelperT()
		{
			//
			// note: ctor Table( TableTemplateBase tt ) clears head/body/footer
			// and then (possbibly) create a new row and adds data
			//
			//AddRow();
		}
	}
}