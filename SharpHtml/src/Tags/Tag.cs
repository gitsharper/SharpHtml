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
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////

	public enum TagRenderMode {
		Normal,           // <tag> </tag>
		NormalNoOpenTag,  // ...</tag>
		VoidTag,          // <tag>
		StartTag,         // <tag>
		EndTag,           // </tag>
		SelfClosing       // <tag />
	}

	public enum TagFormatOptions {
		Horizontal = 0,
		HorizontalWithReturn,
		Vertical,
	}

	public enum StyleBlockAddAs {
		Id,
		Class,
		Standalone
	}

	public enum StyleMode {
		//
		// note: a tags StyleBlocks are always included in the <style>'s at the top of the page
		//
		Ignore,           // ignore StylesDictionary for Tag, use (like table) where styles are placed in StyleBlocks
		IncludeInTag,     // a tags styles are included inline
		IncludeInStyles,  // a tags stylew are added to the <style>'s at the top of the page
	}


	/////////////////////////////////////////////////////////////////////////////

	[DebuggerDisplay( "tag: {_TagName}, id: {Id}" )]

	public abstract class Tag : IRender, IStyles<Tag> {
		protected abstract string _TagName { get; }
		protected virtual TagRenderMode RenderMode { get { return TagRenderMode.Normal; } }
		protected virtual TagFormatOptions MultipleTagFormatAlign { get { return _multipleTagFormatAlign; } }

		// ******
		//
		// we act as a storage point for styles that have been associated with a tags instance,
		// how those styles are rendered depends upon StyleMode and the default is IncludeInTag
		// which will place the styles in style="" as part of the open tag
		//
		// tags or user code can override the default by choosing IncludeInStyles which will cause
		// the code in the "Html->Head->Style" tag to place styles in a block in <style>'s using
		// the tag's id (#id, an id will be generated if the tag does not have one)
		//
		// finally, any style blocks in StyleBocks will be added to <style>'s using the name
		// associated with the block
		//
		// you can add arbitrary style block to the <style>'s on any tag by using one of these methods:
		//
		//   public StyleBlock AddStyleBlock( string styleName, IEnumerable<string> styles )
		//   public StyleBlock AddStyleBlock( string styleName, params string [] styles )
		//
		// by using one of the other AddStyleBlock() methods that take a StyleBlockAddAs argument you
		// can a style block for the current tag using it's Id, a class name (read the code for how its
		// generated), or (redundantly) as the supplied name
		//
		public virtual StyleMode StyleMode { get { return _styleMode; } }

		// ******
		StyleMode _styleMode = StyleMode.IncludeInTag;
		TagFormatOptions _multipleTagFormatAlign = TagFormatOptions.Vertical;

		// ******
		//public const string StdIndentStr = "\t";
		public const string StdIndentStr = "  ";

		// ******
		public string TagName { get { return _TagName; } }

		// ******
		public AttributesDictionary Attributes { get; private set; } = new AttributesDictionary { };
		public StylesDictionary Styles { get; private set; } = new StylesDictionary { };
		public StyleBlockList StyleBlocks { get; private set; } = new StyleBlockList { };

		// ******
		public Tag Parent { get; set; }
		public TagList Children { get; private set; }
		public bool FromTemplate { get; set; } = false;

		// ******
		//
		// Value is used for tag content when there are no Children, FormatStr
		// can be used to format value (by calling it's "ToString( string fmt )"
		// member (if there is one)
		//
		public IRender Value { get; set; }
		public string FormatStr { get; set; } = string.Empty;


		/////////////////////////////////////////////////////////////////////////////

		public virtual Tag Clone()
		//where T : Tag, new()
		{
			// ******
			var tag = (Tag) this.MemberwiseClone();

			tag.Attributes = Attributes.Clone<AttributesDictionary>();
			tag.Styles = Styles.Clone<StylesDictionary>();
			tag.StyleBlocks = StyleBlocks.Clone();

			tag.Parent = null;

			tag.Children = Children.Clone( tag );
			//tag.Children.Clear();
			//foreach( var child in Children ) {
			//	tag.Children.Add( child.Clone() );
			//}

			// ******
			return tag;
		}




		/////////////////////////////////////////////////////////////////////////////

		static int _generatedTagIndex = 0;

		string _id = string.Empty;

		public string Id
		{
			get
			{
				// ******
				var name = _id;
				if( string.IsNullOrEmpty( name ) ) {
					name = string.Format( "{0}{1}", TagName, ++_generatedTagIndex );
					name = SetId( name ).Id;
				}

				// ******
				return name;
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool IdIsEmpty
		{
			get
			{
				return string.IsNullOrWhiteSpace( _id );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		const string CLASS_PREFIX = "cls";

		string _styleClassName = string.Empty;

		public string StyleClassName
		{
			get
			{
				// ******
				if( string.IsNullOrWhiteSpace( _styleClassName ) ) {
					//
					// Id has already been santizec
					//
					_styleClassName = CLASS_PREFIX + Id;
					AddCssClass( _styleClassName );
				}

				// ******
				return _styleClassName;
			}
			set
			{
				if( string.IsNullOrWhiteSpace( _styleClassName ) ) {
					var sanatizedId = HtmlHelpers.CreateSanitizedId( value );
					if( !string.IsNullOrEmpty( sanatizedId ) ) {
						_styleClassName = CLASS_PREFIX + sanatizedId;
						AddCssClass( _styleClassName );
					}
				}
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		public T GetAs<T>()
			where T : Tag
		{
			return this as T;
		}


		/////////////////////////////////////////////////////////////////////////////

		public T GetParentAs<T>()
			where T : Tag
		{
			return Parent as T;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag SetId( string name )
		{
			// ******
			const string ID = "id";

			// ******

			//
			// set only once
			//

			string sanatizedId;
			if( !Attributes.TryGetValue( ID, out sanatizedId ) ) {
				sanatizedId = HtmlHelpers.CreateSanitizedId( name );
				if( !string.IsNullOrEmpty( sanatizedId ) ) {
					_id = Attributes [ ID ] = sanatizedId;
				}
			}

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag SetValue( IRender data )
		{
			Value = data;
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag SetValue( string data, string formatStr = "" )
		{
			Value = new RenderString( data );
			FormatStr = formatStr;
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag SetTagAlign( TagFormatOptions tfo )
		{
			_multipleTagFormatAlign = tfo;
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag SetStyleMode( StyleMode sm )
		{
			_styleMode = sm;
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddAttributesAndStyles( IEnumerable<string> attrAndStyles )
		{
			if( null != attrAndStyles ) {
				attrAndStyles.AddAttributesAndStyles( Attributes, Styles, false );
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddAttributesAndStyles( params string [] attributes )
		{
			return AddAttributesAndStyles( (IEnumerable<string>) attributes );
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		// children
		//
		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Adds a tag supplied by the user, return the Tag that was called
		/// </summary>
		/// <param name="item"></param>
		/// <returns>returns the tag the item was added to</returns>

		public Tag AddChild( Tag item )
		{
			Children.AddChild( item );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag InsertChild( int index, Tag item )
		{
			Children.InsertChild( index, item );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Inserts the tag supplied by the user, returns the Tag that was called
		/// </summary>
		/// <param name="itemAfter"></param>
		/// <param name="item"></param>
		/// <returns>returns the tag the item was added to</returns>

		public Tag InsertChildBefore( Tag itemAfter, Tag item )
		{
			Children.InsertChildBefore( itemAfter, item );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Inserts the tag supplied by the user, returns the Tag that was called
		/// </summary>
		/// <param name="itemBefore"></param>
		/// <param name="item"></param>
		/// <returns>returns the tag the item was added to</returns>

		public Tag InsertChildAfter( Tag itemBefore, Tag item )
		{
			Children.InsertChildAfter( itemBefore, item );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates, adds and return the new QuickTag
		/// </summary>
		/// <param name="name"></param>
		/// <param name="id"></param>
		/// <param name="attrAndStyles"></param>
		/// <returns> returns the tag that was created NOT the tag of the item it was added to</returns>

		public QuickTag AddNewQuickTag( string name, string id = "", IEnumerable<string> attrAndStyles = null )
		{
			return Children.AddNewQuickTag( name, id, attrAndStyles );
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		// Children, create and add using T where T : Tag, new()
		//
		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates, adds and returns new child tag
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="text"></param>
		/// <returns> returns the tag that was created NOT the tag of the item it was added to</returns>

		public T AddNewChild<T>( string text = "" )
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
		/// <summary>
		/// Creates, adds and returns the new child
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="text"></param>
		/// <param name="attributesAndStyles"></param>
		/// <returns> returns the tag that was created NOT the tag of the item it was added to</returns>

		public T AddNewChild<T>( string text, IEnumerable<string> attributesAndStyles )
			where T : Tag, new()
		{
			var item = AddNewChild<T>( text );
			item.AddAttributesAndStyles( attributesAndStyles );
			return item;
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates, adds and return the new child tag
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="text"></param>
		/// <param name="attributesAndStyles"></param>
		/// <returns> returns the tag that was created NOT the tag of the item it was added to</returns>

		public T AddNewChild<T>( string text, params string [] attributesAndStyles )
			where T : Tag, new()
		{
			return AddNewChild<T>( text, (IEnumerable<string>) attributesAndStyles );
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates and adds multiple children of caller supplied T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumString"></param>

		void _addChildItem<T>( IEnumerable<string> enumString )
			where T : Tag, new()
		{
			T item;
			AddChild( item = new T { } );

			if( null == enumString || 0 == enumString.Count() ) {
				return;
			}

			// ******
			item.SetValue( enumString.First().Trim() );
			item.AddAttributesAndStyles( enumString.Skip( 1 ) );
		}

		public void AddNewChild<T>( IEnumerable<string> firstItem, params IEnumerable<string> [] arrayOfIEnumerableString )
			where T : Tag, new()
		{
			// ******
			if( null == firstItem ) {
				return;
			}
			_addChildItem<T>( firstItem );

			// ******
			if( null == arrayOfIEnumerableString ) {
				return;
			}

			foreach( var enumString in arrayOfIEnumerableString ) {
				_addChildItem<T>( enumString );
			}
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates, adds and returns the new child tag
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="text"></param>
		/// <param name="formatStr"></param>
		/// <param name="id"></param>
		/// <param name="attrAndStyles"></param>
		/// <returns> returns the tag that was created NOT the tag of the item it was added to</returns>

		public T AddNewChild<T>( string text, string formatStr, string id, IEnumerable<string> attrAndStyles )
			where T : Tag, new()
		{
			// ******
			var item = new T { };
			AddChild( item );

			// ******
			item.Initialize( text, formatStr, id, attrAndStyles );
			return item;
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates, adds and return the new child tag
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="text"></param>
		/// <param name="formatStr"></param>
		/// <param name="id"></param>
		/// <param name="attrAndStyles"></param>
		/// <returns> returns the tag that was created NOT the tag of the item it was added to</returns>

		public T AddNewChild<T>( string text, string formatStr, string id, params string [] attrAndStyles )
			where T : Tag, new()
		{
			return AddNewChild<T>( text, formatStr, id, (IEnumerable<string>) attrAndStyles );
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Append a series of child and returns the Tag that was called
		/// </summary>
		/// <param name="tl"></param>

		public Tag AppendChildren( IEnumerable<Tag> tl )
		{
			Children.AppendChildren( tl );
			return this;
		}

		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Append a series of child and returns the Tag that was called
		/// </summary>
		/// <param name="tags"></param>

		public Tag AppendChildren( params Tag [] tags )
		{
			return AppendChildren( (IEnumerable<Tag>) tags );
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		// attributes
		//
		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Adds the name of a css class to the tags "class" attribute and returns the
		/// tag that was called
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>

		public Tag AddCssClass( string value )
		{
			string currentValue;

			if( Attributes.TryGetValue( "class", out currentValue ) ) {
				var classesStr = Attributes [ "class" ];
				if( !classesStr.Contains( value ) ) {
					Attributes [ "class" ] = currentValue + " " + value;
				}
			}
			else {
				Attributes [ "class" ] = value;
			}
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddAttribute( string key, object value, bool allowOverwrite = false )
		{
			this.AddAttribute( key, value.ToString(), allowOverwrite );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddAttribute( string key, string value, bool allowOverwrite = false )
		{
			Attributes.Add( key, value );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddAttributes( IEnumerable<string> attributes )
		{
			Attributes.Merge( attributes.ToDictionary<AttributesDictionary>( true ), throwOnError: true );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddAttributes( params string [] attributes )
		{
			return AddAttributes( (IEnumerable<string>) attributes );
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		// IStyles<Tag>
		//
		/////////////////////////////////////////////////////////////////////////////

		public Tag AddComment( string comment )
		{
			Styles.AddComment( comment );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag ReplaceStyle( string key, string value )
		{
			Styles.Replace( key, value );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddStyle( string key, string value )
		{
			Styles.Add( key, value );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddStyles( IEnumerable<string> styles )
		{
			Styles.Merge( styles.ToDictionary<StylesDictionary>( true ), throwOnError: false );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddStyles( params string [] styles )
		{
			return AddStyles( (IEnumerable<string>) styles );
		}


		/////////////////////////////////////////////////////////////////////////////

		//public enum StyleBlockAddAs {
		//	Id,
		//	Class,
		//	Bare
		//}

		string StyleBlockName( StyleBlockAddAs addAs, string name )
		{
			switch( addAs ) {
				case StyleBlockAddAs.Class:
					return string.Format( ".{0} {1}", StyleClassName, name );

				case StyleBlockAddAs.Id:
					return string.Format( "#{0} {1}", Id, name );

				case StyleBlockAddAs.Standalone:
				default:
					return name;
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		//
		// if called externally this allows the adding of arbitrary style blocks
		// to the current tag which will eventually end up in the <style> section of
		// the header - the name of the style block is whatever is provided and need
		// have nothing to do with the current tag
		//
		// *** this can become a debugging nightmare when you don't know where a style
		// comes from !
		//

		public StyleBlock AddStyleBlock( StyleBlock styleBlock )
		{
			StyleBlocks.Add( styleBlock );
			return styleBlock;
		}


		/////////////////////////////////////////////////////////////////////////////

		public StyleBlock AddStyleBlock( string styleName, IEnumerable<string> styles )
		{
			return AddStyleBlock( new StyleBlock( styleName, styles ) );
		}


		/////////////////////////////////////////////////////////////////////////////

		public StyleBlock AddStyleBlock( string styleName, params string [] styles )
		{
			return AddStyleBlock( styleName, (IEnumerable<string>) styles );
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddStyleBlock( StyleBlockAddAs addAs, string name, IEnumerable<string> styles )
		{
			var styleName = StyleBlockName( addAs, name );
			//StyleBlocks.Add( new StyleBlock( styleName, styles ) );
			//AddStyleBlock( new StyleBlock( styleName, styles ) );
			AddStyleBlock( styleName, styles );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddStyleBlock( StyleBlockAddAs addAs, string name, params string [] styles )
		{
			return AddStyleBlock( addAs, name, (IEnumerable<string>) styles );
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag AddStyleBlock( StyleBlockAddAs addAs, string name, StylesDictionary styles )
		{
			var styleName = StyleBlockName( addAs, name );
			StyleBlocks.Add( new StyleBlock( styleName, styles ) );
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////
		//
		// rendering tag and contents
		//
		/////////////////////////////////////////////////////////////////////////////

		public string Indent( string text, int nIndents )
		{
			// ******
			if( nIndents <= 0 ) {
				return text;
			}

			// ******
			return text.Indent( StdIndentStr, nIndents );
		}


		/////////////////////////////////////////////////////////////////////////////

		public string Format( object value, string format )
		{
			// ******
			if( string.IsNullOrWhiteSpace( FormatStr ) ) {
				return value.ToString();
			}
			else {
				return value.ApplyFormat( format );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		protected virtual string RenderContent()
		{
			// ******
			if( Children.Count > 0 ) {
				var sb = new StringBuilder { };

				foreach( var child in Children ) {
					sb.Append( child.Render() );
				}

				// ******
				return sb.ToString();
			}
			else {
				if( null == Value ) {
					return string.Empty;
				}
				else if( !string.IsNullOrWhiteSpace( FormatStr ) ) {
					return Format( Value.Render(), FormatStr );
				}
				else {
					return Value.Render();
				}
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		//
		// render from script, takes a string for TagRenderMode
		//

		public string Render( string selectorIn )
		{
			// ******
			if( string.IsNullOrWhiteSpace( selectorIn ) ) {
				throw new ArgumentNullException( "selectorIn" );
			}

			// ******
			if( selectorIn.Length >= 2 ) {
				TagRenderMode selector;
				var name = char.ToUpper( selectorIn.First() ) + selectorIn.Substring( 1 ).ToLower();
				if( Enum.TryParse( name, out selector ) ) {
					return Render( selector );
				}
			}

			// ******
			throw new Exception( string.Format( "unknown TagRenderMode \"{0}\"", selectorIn ) );
		}


		/////////////////////////////////////////////////////////////////////////////

		public virtual string Render()
		{
			return Render( RenderMode );
		}


		/////////////////////////////////////////////////////////////////////////////

		public string Render( TagRenderMode renderMode )
		{

			// ******
			StringBuilder sb = new StringBuilder();

			switch( renderMode ) {
				case TagRenderMode.VoidTag:
				case TagRenderMode.StartTag:
					StartTag( sb );
					break;

				case TagRenderMode.EndTag:
					EndTag( sb );
					break;

				case TagRenderMode.SelfClosing:
					SelfClosingTag( sb );
					break;

				case TagRenderMode.NormalNoOpenTag:
					RenderNormal( sb, false );
					break;

				case TagRenderMode.Normal:
				default:
					RenderNormal( sb, true );
					break;
			}

			// ******
			//if( FromTemplate && null != BumpTemplateUseCount ) {
			//	BumpTemplateUseCount( this );
			//}

			// ******
			return sb.ToString();
		}


		/////////////////////////////////////////////////////////////////////////////

		void RenderInlineStyles( StylesDictionary styles, StringBuilder sb )
		{
			// ******
			if( null != styles && styles.Count > 0 ) {
				sb.Append( " style=\"" );
				Styles.Render( sb, false );
				sb.Append( "\"" );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		void SelfClosingTag( StringBuilder sb )
		{
			sb.Append( '<' ).Append( TagName );

			var nAttr = Attributes.Render( sb );

			if( StyleMode.IncludeInTag == StyleMode ) {
				if( 0 == nAttr ) {
					sb.Append( ' ' );
				}
				RenderInlineStyles( Styles, sb );
			}

			sb.Append( nAttr > 0 ? " />" : "/>" );

			//
			// after close tag
			//
			if( TagFormatOptions.Vertical == MultipleTagFormatAlign ) {
				sb.AppendLine();  // at end of tag
				sb.AppendLine();  // blank line
			}
			else if( TagFormatOptions.HorizontalWithReturn == MultipleTagFormatAlign ) {
				sb.AppendLine();  // line after closing tag
			}
			else {
				sb.Append( " " );
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		void StartTag( StringBuilder sb )
		{
			// ******
			sb.Append( '<' ).Append( TagName );

			Attributes.Render( sb );

			if( StyleMode.IncludeInTag == StyleMode ) {
				RenderInlineStyles( Styles, sb );
			}

			sb.Append( '>' );

			//
			// after open tag, before content
			//
			if( TagFormatOptions.Vertical == MultipleTagFormatAlign ) {
				sb.AppendLine();
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		void EndTag( StringBuilder sb )
		{
			sb.Append( "</" ).Append( TagName ).Append( '>' );
		}


		/////////////////////////////////////////////////////////////////////////////

		void RenderNormal( StringBuilder sb, bool renderOpenTag )
		{
			if( renderOpenTag ) {
				sb.Append( '<' )
					.Append( TagName );

				Attributes.Render( sb );

				if( StyleMode.IncludeInTag == StyleMode ) {
					RenderInlineStyles( Styles, sb );
				}

				sb.Append( '>' );
			}

			//
			// after open tag, before content
			//
			if( TagFormatOptions.Vertical == MultipleTagFormatAlign ) {
				sb.AppendLine();
			}
			else {
			}


			if( TagFormatOptions.Vertical == MultipleTagFormatAlign ) {
				//
				// only indent for vertial formatting
				//
				var content = Indent( RenderContent(), 1 );

				sb.Append( content );

				if( content.Length > 0 ) {
					var lastCh = content.Last();
					if( SC.NEWLINE != lastCh && SC.CR != lastCh ) {
						sb.AppendLine();
					}
				}
			}
			else {
				sb.Append( RenderContent() );
			}

			sb.Append( "</" )
							.Append( TagName )
							.Append( '>' );

			if( !IdIsEmpty ) {
				sb.Append( $" <!-- {Id} -->" );
			}
			//
			// after close tag
			//
			if( TagFormatOptions.Vertical == MultipleTagFormatAlign ) {
				sb.AppendLine();  // at end of close tag
				sb.AppendLine();  // blank line following
			}
			else if( TagFormatOptions.HorizontalWithReturn == MultipleTagFormatAlign ) {
				sb.AppendLine();  // line after closing tag
			}
			else {
				sb.Append( " " );
			}

		}


		/////////////////////////////////////////////////////////////////////////////

		//
		// override to add style to StyleBlocks, or perform any other action
		// to setup the tags (or associated tags) styles
		//
		// <table> uses this to generate it's various nth-child() styles
		//

		public virtual void BuildUpStyles()
		{
			foreach( var child in Children ) {
				child.BuildUpStyles();
			}
		}



		/////////////////////////////////////////////////////////////////////////////

		public override string ToString()
		{
			//return Render( TagRenderMode.Normal );
			return string.Format( "Tag: {0}", TagName );
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag Initialize( string text, string formatStr, string id, IEnumerable<string> attrAndStyles )
		{
			return Initialize( new RenderString( text ), formatStr, id, attrAndStyles );
		}


		/////////////////////////////////////////////////////////////////////////////

		public Tag Initialize( IRender content, string formatStr, string id, IEnumerable<string> attrAndStyles )
		{
			Value = content ?? new RenderString { };
			FormatStr = string.IsNullOrWhiteSpace( formatStr ) ? string.Empty : formatStr;

			SetId( id );

			this.AddAttributesAndStyles( attrAndStyles );

			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		protected Tag()
		{
			Children = new TagList( this );
		}

	}

}
