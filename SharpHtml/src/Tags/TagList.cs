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

namespace SharpHtml {


	/////////////////////////////////////////////////////////////////////////////

	public class TagList : IEnumerable<Tag> {

		// ******
		public IEnumerator<Tag> GetEnumerator() { return tagsList.GetEnumerator(); }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return tagsList.GetEnumerator(); }

		// ******
		List<Tag> tagsList = new List<Tag> { };
		readonly Tag owner;

		// ******
		public bool HasOwner { get { return null != owner; } }
		public int Count { get { return tagsList.Count; } }


		/////////////////////////////////////////////////////////////////////////////

		public TagList Clone( Tag newParent )
		{
			// ******
			var tl = new TagList( newParent );

			foreach( var tag in tagsList ) {
				tl.AddChild( tag.Clone() );
			}

			// ******
			return tl;
		}


		/////////////////////////////////////////////////////////////////////////////
		
		public Tag FindFirst( string tagName )
		{
			return tagsList.Find( tag => tagName == tag.TagName );
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag FindFirst<T>( ) 
			where T : Tag
		{
			var type = typeof( T );
			return tagsList.Find( tag => type == tag.GetType() );
		}


		/////////////////////////////////////////////////////////////////////////////

		public void Clear()
		{
			tagsList.Clear();
		}


		/////////////////////////////////////////////////////////////////////////////

		public void Remove( Tag item )
		{
			tagsList.Remove( item );
		}


		/////////////////////////////////////////////////////////////////////////////

		protected void AddTagParent( Tag tag )
		{
			if( HasOwner ) {
				tag.Parent = owner;
			}
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		//
		//
		/////////////////////////////////////////////////////////////////////////////

		public void AddChild( Tag tag )
		{
			tagsList.Add( tag );
			AddTagParent( tag );
		}


		/////////////////////////////////////////////////////////////////////////////

		public void InsertChild( int index, Tag tag )
		{
			// ******
			if( null == tag ) {
				throw new ArgumentNullException( nameof( tag ) );
			}

			// ******
			if( index < 0 ) {
				index = 0;
			}
			else if( index >= tagsList.Count ) {
				index = tagsList.Count;
			}
			tagsList.Insert( index, tag );
		}


		/////////////////////////////////////////////////////////////////////////////

		public void InsertChildBefore( Tag tagAfter, Tag tag )
		{
			// ******
			if( null == tag ) {
				throw new ArgumentNullException( nameof( tag ) );
			}

			// ******
			var index = null == tagAfter ? -1 : tagsList.FindIndex( t => t == tagAfter );
			if( index < 0 ) {
				//
				// head
				//
				tagsList.Insert( 0, tag );
			}
			else {
				tagsList.Insert( index, tag );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public void InsertChildAfter( Tag tagBefore, Tag tag )
		{
			// ******
			if( null == tag ) {
				throw new ArgumentNullException( nameof( tag ) );
			}

			// ******
			var index = null == tagBefore ? -1 : tagsList.FindIndex( t => t == tagBefore );
			if( index < 0 ) {
				//
				// end
				//
				tagsList.Add( tag );
			}
			else {
				tagsList.Insert( 1 + index, tag );
			}

		}


		/////////////////////////////////////////////////////////////////////////////

		public T AddChild<T>( string text = "" )
			where T : Tag, new()
		{
			// ******
			var item = new T { };
			AddChild( item );

			// ******
			item.SetValue( text );
			return item;
		}


		/////////////////////////////////////////////////////////////////////////////

		public TagList AppendChildren( IEnumerable<Tag> tags )
		{
			foreach( var tag in tags ) {
				AddChild( tag );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public TagList AppendChildren( TagList tags )
		{
			//AddRange( tags );
			foreach( var tag in tags.tagsList ) {
				AddChild( tag );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public QuickTag AddNewQuickTag( string name, string id = "", IEnumerable<string> attrAndStyles = null )
		{
			var ut = new QuickTag( name, id, attrAndStyles );
			AddChild( ut );
			return ut;
		}


		/////////////////////////////////////////////////////////////////////////////

		public TagList()
			: this( null )
		{
		}


		/////////////////////////////////////////////////////////////////////////////

		public TagList( Tag owner, params Tag [] tags )
		{
			this.owner = owner;
			AppendChildren( tags );
		}

	}
}
