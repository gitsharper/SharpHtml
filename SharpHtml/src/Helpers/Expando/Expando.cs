#region License
/*
 **************************************************************
 *  Author: Rick Strahl 
 *          © West Wind Technologies, 2012
 *          http://www.west-wind.com/
 * 
 * Created: Feb 2, 2012
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 **************************************************************  

 Joe McLain, September 2015
	
	o removed serialization

*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Reflection;

namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Class that provides extensible properties and methods to an
	/// existing object when cast to dynamic. This
	/// dynamic object stores 'extra' properties in a dictionary or
	/// checks the actual properties of the instance passed via 
	/// constructor.
	/// 
	/// This class can be subclassed to extend an existing type or 
	/// you can pass in an instance to extend. Properties (both
	/// dynamic and strongly typed) can be accessed through an 
	/// indexer.
	/// 
	/// This type allows you three ways to access its properties:
	/// 
	/// Directly: any explicitly declared properties are accessible
	/// Dynamic: dynamic cast allows access to dictionary and native properties/methods
	/// Dictionary: Any of the extended properties are accessible via IDictionary interface
	/// </summary>

	public class Expando : DynamicObject, IDynamicMetaObjectProvider {
		/// <summary>
		/// Instance of object passed in
		/// </summary>
		object Instance;

		/// <summary>
		/// Cached type of the instance
		/// </summary>
		Type InstanceType;

		PropertyInfo [] InstancePropertyInfo
		{
			get
			{
				if( _InstancePropertyInfo == null && Instance != null )
					_InstancePropertyInfo = Instance.GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly );
				return _InstancePropertyInfo;
			}
		}
		PropertyInfo [] _InstancePropertyInfo;


		/// <summary>
		/// String Dictionary that contains the extra dynamic values
		/// stored on this object/instance
		/// </summary>        
		/// <remarks>Using PropertyBag to support XML Serialization of the dictionary</remarks>
		public PropertyBag Properties = new PropertyBag();

		//public Dictionary<string,object> Properties = new Dictionary<string, object>();


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// This constructor just works off the internal dictionary and any 
		/// public properties of this object.
		/// 
		/// Note you can subclass Expando.
		/// </summary>

		public Expando()
		{
			Initialize( this );
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Allows passing in an existing instance variable to 'extend'.        
		/// </summary>
		/// <remarks>
		/// You can pass in null here if you don't want to 
		/// check native properties and only check the Dictionary!
		/// </remarks>
		/// <param name="instance"></param>

		public Expando( object instance )
		{
			Initialize( instance );
		}


		/////////////////////////////////////////////////////////////////////////////

		protected virtual void Initialize( object instance )
		{
			Instance = instance;
			if( instance != null )
				InstanceType = instance.GetType();
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Return both instance and dynamic names.
		/// 
		/// Important to return both so JSON serialization with 
		/// Json.NET works.
		/// </summary>
		/// <returns></returns>

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			foreach( var prop in GetProperties( true ) )
				yield return prop.Key;
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Try to retrieve a member by name first from instance properties
		/// followed by the collection entries.
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="result"></param>
		/// <returns></returns>

		public override bool TryGetMember( GetMemberBinder binder, out object result )
		{
			return TryGetProperty( binder.Name, out result );
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Property setter implementation tries to retrieve value from instance 
		/// first then into this object
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="value"></param>
		/// <returns></returns>

		public override bool TrySetMember( SetMemberBinder binder, object value )
		{
			return TrySetProperty( binder.Name, value );
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Dynamic invocation method. Currently allows only for Reflection based
		/// operation (no ability to add methods dynamically).
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="args"></param>
		/// <param name="result"></param>
		/// <returns></returns>

		public override bool TryInvokeMember( InvokeMemberBinder binder, object [] args, out object result )
		{
			if( Instance != null ) {
				try {
					// check instance passed in for methods to invoke
					if( InvokeMethod( Instance, binder.Name, args, out result ) )
						return true;
				}
				catch { }
			}

			result = null;
			return false;
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Reflection Helper method to retrieve a property
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="name"></param>
		/// <param name="result"></param>
		/// <returns></returns>

		protected bool GetProperty( object instance, string name, out object result )
		{
			if( instance == null )
				instance = this;

			var miArray = InstanceType.GetMember( name, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance );
			if( miArray != null && miArray.Length > 0 ) {
				var mi = miArray [ 0 ];
				if( mi.MemberType == MemberTypes.Property ) {
					result = ((PropertyInfo) mi).GetValue( instance, null );
					return true;
				}
			}

			result = null;
			return false;
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Reflection helper method to set a property value
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>

		protected bool SetProperty( object instance, string name, object value )
		{
			if( instance == null )
				instance = this;

			var miArray = InstanceType.GetMember( name, BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance );
			if( miArray != null && miArray.Length > 0 ) {
				var mi = miArray [ 0 ];
				if( mi.MemberType == MemberTypes.Property ) {
					((PropertyInfo) mi).SetValue( Instance, value, null );
					return true;
				}
			}
			return false;
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Reflection helper method to invoke a method
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="name"></param>
		/// <param name="args"></param>
		/// <param name="result"></param>
		/// <returns></returns>

		protected bool InvokeMethod( object instance, string name, object [] args, out object result )
		{
			if( instance == null )
				instance = this;

			// Look at the instanceType
			var miArray = InstanceType.GetMember( name,
															BindingFlags.InvokeMethod |
															BindingFlags.Public | BindingFlags.Instance );

			if( miArray != null && miArray.Length > 0 ) {
				var mi = miArray [ 0 ] as MethodInfo;
				result = mi.Invoke( Instance, args );
				return true;
			}

			result = null;
			return false;
		}


		/////////////////////////////////////////////////////////////////////////////

		public delegate object GetReferencedObject();

		public GetReferencedObject MakeGetReferencedObject( PropertyInfo info, object instance )
		{
			return () => info.GetValue( instance );
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Add a property that reads another property and returns it's value. Use this
		/// when you want to always return the current value on some object where it's
		/// value may change.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="info"></param>
		/// <param name="instance"></param>

		public void AddPropertyReference( string name, PropertyInfo info, object instance )
		{
			Properties [ name ] = MakeGetReferencedObject( info, instance );
		}


		/////////////////////////////////////////////////////////////////////////////

		public void AddProperty( string name, object value )
		{
			Properties [ name ] = value;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool TryGetProperty( string name, out object result )
		{
			// ******
			result = null;
			var success = false;

			// ******
			if( Properties.Keys.Contains( name ) ) {
				result = Properties [ name ];
				success = true;
			}
			else {
				//
				// try reflection on instanceType
				//
				if( GetProperty( Instance, name, out result ) ) {
					success = true;
				}
			}

			if( success ) {
				if( null != result ) {
					//
					// see if we need to retreive the value from a property
					// located somewhere else, this allows us to always return
					// the current value of a property that may have been changed
					//
					var gro = result as GetReferencedObject;
					if( null != gro ) {
						result = gro();
					}
				}

				// ******
				return true;
			}

			// ******
			return false;
		}


		/////////////////////////////////////////////////////////////////////////////

		protected bool TrySetProperty( string name, object value )
		{
			// ******

			//
			// always try to set value in Properties first if it already exists there, if
			// it does not exist there then try instance properties, if not found there then
			// add to Properties
			//
			// question, how can override instance properties if a Property can ONLY be set
			// once an instance property is not found ??
			//
			//  do we care?
			//
			//  if we decide to care, add explicit AddPropert() for Properties
			//

			if( Properties.ContainsKey( name ) ) {
				Properties [ name ] = value;
				return true;
			}

			// ******
			//
			// try instance property first because dynamic property will always succeed
			// with a set
			//
			if( Instance != null ) {
				try {
					bool result = SetProperty( Instance, name, value );
					if( result ) {
						return true;
					}
				}
				catch { }
			}

			// ******
			//
			// no match - set or add to dictionary
			//
			Properties [ name ] = value;
			return true;
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Convenience method that provides a string Indexer 
		/// to the Properties collection AND the strongly typed
		/// properties of the object by name.
		/// 
		/// // dynamic
		/// exp["Address"] = "112 nowhere lane"; 
		/// // strong
		/// var name = exp["StronglyTypedProperty"] as string; 
		/// </summary>
		/// <remarks>
		/// The getter checks the Properties dictionary first
		/// then looks in PropertyInfo for properties.
		/// The setter checks the instance properties before
		/// checking the Properties dictionary.
		/// </remarks>
		/// <param name="key"></param>
		/// 
		/// <returns></returns>
		public object this [ string key ]
		{
			get
			{
				object value;
				if( TryGetProperty( key, out value ) ) {
					return value;
				}
				throw new KeyNotFoundException { };
			}
			set
			{
				TrySetProperty( key, value );
			}
		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns and the properties of 
		/// </summary>
		/// <param name="includeProperties"></param>
		/// <returns></returns>

		public IEnumerable<KeyValuePair<string, object>> GetProperties( bool includeInstanceProperties = false )
		{
			if( includeInstanceProperties && Instance != null ) {
				foreach( var prop in this.InstancePropertyInfo )
					yield return new KeyValuePair<string, object>( prop.Name, prop.GetValue( Instance, null ) );
			}

			foreach( var key in this.Properties.Keys )
				yield return new KeyValuePair<string, object>( key, this.Properties [ key ] );

		}


		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Checks whether a property exists in the Property collection
		/// or as a property on the instance
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>

		public bool Contains( KeyValuePair<string, object> item, bool includeInstanceProperties = false )
		{
			bool res = Properties.ContainsKey( item.Key );
			if( res )
				return true;

			if( includeInstanceProperties && Instance != null ) {
				foreach( var prop in this.InstancePropertyInfo ) {
					if( prop.Name == item.Key )
						return true;
				}
			}

			return false;
		}

	}
}
