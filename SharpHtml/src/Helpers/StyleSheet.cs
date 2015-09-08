using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpHtml {

	/////////////////////////////////////////////////////////////////////////////


	public class StyleSheet : StyleBlockList {

		public const char VARIABLE_DEF_CHAR = '@';
		public const string VARIABLE_DEF_STR = "@";

		// ******
		protected Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string> { };

		/////////////////////////////////////////////////////////////////////////////

		public string NormalizeVarName( string name )
		{
			return name?.Trim().ToLower() ?? null;
		}


		/////////////////////////////////////////////////////////////////////////////

		public bool RemoveVariable( string name )
		{
			// ******
			var varName = NormalizeVarName(name);
			if( null != varName && Variables.ContainsKey( NormalizeVarName( varName ) ) ) {
				Variables.Remove( varName );
				return true;
			}

			// ******
			return false;
		}


		/////////////////////////////////////////////////////////////////////////////

		// pushVar
		// popVar

		public StyleSheet DefineVariable( string name, string value )
		{
			// ******
			var varName = NormalizeVarName(name);
			if( string.IsNullOrWhiteSpace( varName ) ) {
				throw new ArgumentNullException( nameof( name ) );
			}

			// ******
			Variables [ varName ] = value;

			// ******
			return this;
		}


		/////////////////////////////////////////////////////////////////////////////

		static void _test()
		{

			/*

			<div class="main-container"
			<div id="content" class="main wrapper clearfix"
				<nav id="sidebar"
				<div id="mainContent"

			<div id="footer-container"
				<footer class"wrapper"
	*/

			// variables
			// save as .css file

			// have node and lessc.cmd in path


			var sb = new StyleBlock { };








		}

	}


}