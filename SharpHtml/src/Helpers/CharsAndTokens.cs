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
using System.Text;
using System.Text.RegularExpressions;


	///////////////////////////////////////////////////////////////////////////
	//
	// StaticCharsAndTokens
	//
	///////////////////////////////////////////////////////////////////////////

	public static class SC {

		public const string	MACRO_TRUE		= "1";
		public const string	MACRO_FALSE		= "0";
		public const string	MACRO_NULL		= "(null)";
	
	
		public const int		END_OF_BUFFER			= -1;
		
		public const char		NO_CHAR						= '\0';
		public const char		EOS								= '\0';

		public const char		NEWLINE						= '\n';
		public const string	NEWLINE_STR				= "\n";
		public const char		CR								= '\r';
		public const char		TAB								= '\t';
		public const string	TAB_STR						= "\t";
		
		public const char		SPACE							= ' ';
		public const char		WHITE_SPACE				= ' ';		// represents all white-space

		public const char		OPEN_BRACE				= '{';
		public const char		CLOSE_BRACE				= '}';
	
		public const char		OPEN_OBJECT				= '{';
		public const char		CLOSE_OBJECT			= '}';
	
		public const string	OPEN_OBJECT_STR		= "{";
		public const string	CLOSE_OBJECT_STR	= "}";
	
		public const char		OPEN_BRACKET			= '[';
		public const char		CLOSE_BRACKET			= ']';
		
		public const string	OPEN_BRACKET_STR	= "[";
		public const string	CLOSE_BRACKET_STR	= "]";
		
		public const char		OPEN_ARRAY				= '[';
		public const char		CLOSE_ARRAY				= ']';
		
		//public const string	OPEN_ARRAY_STR		= "[";
		//public const string	CLOSE_ARRAY_STR		= "]";
		
		public const char		OPEN_PAREN				= '(';
		public const char		CLOSE_PAREN				= ')';
		
		public const string	OPEN_PAREN_STR		= "(";
		public const string	CLOSE_PAREN_STR		= ")";
		
		public const char		COMMA							= ',';
		public const string	COMMA_STR					= ",";
		
		public const char		DOUBLE_QUOTE			= '"';
		public const char		SINGLE_QUOTE			= '\'';
		public const char		BACK_TICK					= '`';

		public const char		COLON							= ':';
		public const char		SEMI_COLON				= ';';

		public const char		DOT								= '.';
		public const char		EQUAL							= '=';
		public const char		BACKSLASH					= '\\';
		public const char		PERCENT						= '%';
		public const char		ATCHAR						= '@';
//		public const char		MINUS							= '-';
		public const char		DASH							= '-';
		public const char		CARET							= '^';

		public const char		VBAR							= '|';
		public const char		OR								= '|';


		
//		public const char		TOKEN_PASTING_CHAR	= '#';
//		public const char		TO_END_OF_LINE_COMMENT	= '#';
		
		public const char		POUND							= '#';
		public const char		HASH							= '#';
		public const string	HASH_STR					= "#";
		public const char		STAR							= '*';
		public const char		DOLLAR						= '$';
		public const char		UNDERSCORE				= '_';

		public const char		ESCAPE_CHAR				= '\\';


		//public const char		REF_STARTS_WITH_CHAR	= '\uEEFF';


		//public const char		DEF_OPEN_QUOTE				= '`';
		//public const char		DEF_CLOSE_QUOTE				= '\'';
		//public const char		DEF_NULL_QUOTE				= '\0';
		//public const string	DEF_NULL_QUOTE_STR		= "\0";
		
		//public const string	DEF_OPEN_QUOTE_STR	= "`";
		//public const string	DEF_CLOSE_QUOTE_STR	= "\'";
		

		/*
		
			Private Use Area
				
				U+E000 ... U+F8FF   
				
			Supplementary Private Use Area-A (Plane 15)

				U+F0000 ... U+FFFFD

			There are 16 unicode characters that are specified
			as "Unicode Specials" 0xFFF0 - 0xFFFF. The last two
			are used to determine unicode encoding; as of Unicode
			5 there are an additional 5 with special meaning (listed
			below) and then there are the remainder.
			
			U+FFF9 "INTERLINEAR ANNOTATION ANCHOR", marks start of annotated text 
			U+FFFA "INTERLINEAR ANNOTATION SEPARATOR", marks start of annotating text 
			U+FFFB "INTERLINEAR ANNOTATION TERMINATOR", marks end of annotating text 
			U+FFFC "OBJECT REPLACEMENT CHARACTER", placeholder in the text for another unspecified object, for example in a compound document. 
			U+FFFD "REPLACEMENT CHARACTER" used to replace an unknown or unprintable character 

			We use various combinations of these in the text stream
			to represent a number of different things (see below)
		
		*/

		/////////////////////////////////////////////////////////////////////////////
		//
		// ranges of characters
		//		
		/////////////////////////////////////////////////////////////////////////////
		//
		// unicode private area (which we use)
		//
		public const char	FIRST_PRIVATEUSE_CHAR	= '\uE000';
		public const char	LAST_PRIVATEUSE_CHAR	= '\uF8FF';


		///////////////////////////////////////////////////////////////////////////////

		public static string HtmlEncode( string str, bool encodeAngleBrackets )
		{
			var sb = new StringBuilder();
			foreach( char ch in str ) {
				string result = TryHtmlEncode( ch, encodeAngleBrackets );
				if( 0 == result.Length ) {
					sb.Append( ch );
				}
				else {
					sb.Append( result );
				}
			}

			return sb.ToString();
		}

			
		///////////////////////////////////////////////////////////////////////////////

//
// common, but NOT all used character entities
//
//&nbsp;		\u00A0		non-breaking space ISO8559-1 
//&copy;		\u00A9		copyright sign ISO8559-1 
//&reg;			\u00AE		registered trade mark sign ISO8559-1 
//&sup2;		\u00B2		superscript 2 (squared) ISO8559-1 
//&sup3;		\u00B3		superscript 3 (cubed) ISO8559-1 
//&quot;		\u0022		quotation mark ISO10646 
//&amp;			\u0026		ampersand sign ISO10646 
//&lt;			\u003C		less than sign ISO10646 
//&gt;			\u003E		greater than sign ISO10646 
//&ndash;		\u2013		en dash ISO10646 
//&mdash;		\u2014		em dash ISO10646 
//&lsquo;		\u2018		left single quote ISO10646 
//&rsquo;		\u2019		right single quote, apostrophe ISO10646 
//&ldquo;		\u201C		left double quotation mark ISO10646 
//&rdquo;		\u201D		right double quotation mark ISO10646 
//&bull;		\u2022		small black circle, bullet ISO10646 
//&dagger;	\u2020		dagger sign ISO10646 
//&Dagger;	\u2021		double dagger sign ISO10646 
//&prime;		\u2032		prime = minutes = feet ISO10646 
//&Prime;		\u2033		double prime = seconds = inches ISO10646 
//&lsaquo;	\u2039		single left pointing angle quote ISO10646 
//&rsaquo;	\u203A		single right pointing angle quote ISO10646 
//&euro;		\u20AC		euro sign ISO10646 
//&trade;		\u2122		Registered Trademark sign ISO10646 
//&tilde;		\u02DC		tilde sign ISO10646 
//&circ;		\u02C6		circumflex (or caret) sign ISO10646 
//&spades;	\u2660		black spade suit ISO10646 
//&clubs;		\u2663		black clubs suit ISO10646 
//&hearts;	\u2665		black heart suit ISO10646 
//&diams;		\u2666		black diamonds suit ISO10646 
//&loz;			\u25CA		lozenge ISO10646 
//&larr;		\u2190		left arrow ISO10646 
//&rarr;		\u2192		right arrow ISO10646 
//&uarr;		\u2191		up arrow ISO10646 
//&darr;		\u2193		down arrow ISO10646 
//&harr;		\u2194		right-left arrow ISO10646 
//&not;			\u00AC		NOT sign ISO8859-1 

		public static string TryHtmlEncode( char ch, bool encodeAngleBrackets )
		{
			// ******
			switch( ch ) {
				case '\u00A0':		// non-breaking space ISO8559-1 
					return "&nbsp;";
					
				case '\u00A9':		// copyright sign ISO8559-1 
					return "&copy;";
					
				case '\u00AE':		// registered trade mark sign ISO8559-1 
					return "&reg;";
						
				case '\u00B2':		// superscript 2 (squared) ISO8559-1 
					return "&sup2;";
					
				case '\u00B3':		// superscript 3 (cubed) ISO8559-1 
					return "&sup3;";
					
				//
				// "
				//
				case '\u0022':		// quotation mark ISO10646 
					//return "&quot;";
					return encodeAngleBrackets ? "&quot;" : string.Empty;
					
				//
				// &
				//
				case '\u0026':		// ampersand sign ISO10646 
					//return "&amp;";
					return encodeAngleBrackets ? "&amp;" : string.Empty;

				//
				// '
				//
				case '\u0027':
					return "&apos;";

				//
				// <
				//						
				case '\u003C':		// less than sign ISO10646 
					//return "&lt;";
					return encodeAngleBrackets ? "&lt;" : string.Empty;
					
				//
				// >
				//	
				case '\u003E':		// greater than sign ISO10646 
					//return "&gt;";
					return encodeAngleBrackets ? "&gt;" : string.Empty;
						
				case '\u2013':		// en dash ISO10646 
					return "&ndash;";
					
				case '\u2014':		// em dash ISO10646 
					return "&mdash;";
					
				case '\u2018':		// left single quote ISO10646 
					return "&lsquo;";
					
				case '\u2019':		// right single quote, apostrophe ISO10646 
					return "&rsquo;";
					
				case '\u201C':		// left double quotation mark ISO10646 
					return "&ldquo;";
					
				case '\u201D':		// right double quotation mark ISO10646 
					return "&rdquo;";
					
				case '\u2022':		// small black circle, bullet ISO10646 
					return "&bull;";
					
				case '\u2020':		// dagger sign ISO10646 
					return "&dagger;";
				
				case '\u2021':		// double dagger sign ISO10646 
					return "&Dagger;";
				
				case '\u2032':		// prime = minutes = feet ISO10646 
					return "&prime;";
					
				case '\u2033':		// double prime = seconds = inches ISO10646 
					return "&Prime;";
					
				case '\u2039':		// single left pointing angle quote ISO10646 
					return "&lsaquo;";
				
				case '\u203A':		// single right pointing angle quote ISO10646 
					return "&rsaquo;";
				
				case '\u20AC':		// euro sign ISO10646 
					return "&euro;";
					
				case '\u2122':		// Registered Trademark sign ISO10646 
					return "&trade;";
					
				case '\u02DC':		// tilde sign ISO10646 
					return "&tilde;";
					
				case '\u02C6':		// circumflex (or caret) sign ISO10646 
					return "&circ;";
					
				case '\u2660':		// black spade suit ISO10646 
					return "&spades;";
				
				case '\u2663':		// black clubs suit ISO10646 
					return "&clubs;";
					
				case '\u2665':		// black heart suit ISO10646 
					return "&hearts;";
				
				case '\u2666':		// black diamonds suit ISO10646 
					return "&diams;";
					
				case '\u25CA':		// lozenge ISO10646 
					return "&loz;";
						
				case '\u2190':		// left arrow ISO10646 
					return "&larr;";
					
				case '\u2192':		// right arrow ISO10646 
					return "&rarr;";
					
				case '\u2191':		// up arrow ISO10646 
					return "&uarr;";
					
				case '\u2193':		// down arrow ISO10646 
					return "&darr;";
					
				case '\u2194':		// right-left arrow ISO10646 
					return "&harr;";
					
				case '\u00AC':		// NOT sign ISO8859-1 
					return "&not;";
						




				//
				//	¡	¡	&#161;	inverted exclamation mark
				//
				case (char) 0161:
					return "&iexcl;";

				//
				//	¢	¢	&#162;	cent sign
				//
				case (char) 0162:
					return "&cent;";

				//
				//	£	£	&#163;	pound sign
				//
				case (char) 0163:
					return "&pound;";

				//
				//	¤	¤	&#164;	currency sign
				//
				case (char) 0164:
					return "&curren;";

				//
				//	¥	¥	&#165;	yen sign = yuan sign
				//
				case (char) 0165:
					return "&yen;";

				//
				//	¦	¦	&#166;	broken vertical bar
				//
				case (char) 0166:
					return "&brvbar;";

				//
				//	§	§	&#167;	section sign
				//
				case (char) 0167:
					return "&sect;";

				//
				//	¨	¨	&#168;	diaeresis = spacing diaeresis
				//
				case (char) 0168:
					return "&uml;";

//				//
//				//	©	©	&#169;	copyright sign
//				//
//				case (char) 0169:
//					return "&copy;";

				//
				//	ª	ª	&#170;	feminine ordinal indicator
				//
				case (char) 0170:
					return "&ordf;";

				//
				//	«	«	&#171;	left-pointing double angle quotes (left pointing quillemet)
				//
				case (char) 0171:
					return "&laquo;";

//				//
//				//	¬	¬	&#172;	not sign
//				//
//				case (char) 0172:
//					return "&not;";

				//
				//	­	­	&#173;	soft hyphen
				//
				case (char) 0173:
					return "&shy;";

//				//
//				//	®	®	&#174;	registered sign
//				//
//				case (char) 0174:
//					return "&reg;";

				//
				//	¯	¯	&#175;	macron = spacing macron
				//
				case (char) 0175:
					return "&macr;";

				//
				//	°	°	&#176;	degree sign
				//
				case (char) 0176:
					return "&deg;";

				//
				//	±	±	&#177;	plus-minus sign
				//
				case (char) 0177:
					return "&plusmn;";

//				//
//				//	²	²	&#178;	superscript two (squared)
//				//
//				case (char) 0178:
//					return "&sup2;";

//				//
//				//	³	³	&#179;	superscript three (cubed)
//				//
//				case (char) 0179:
//					return "&sup3;";

				//
				//	´	´	&#180;	acute accent
				//
				case (char) 0180:
					return "&acute;";

				//
				//	µ	µ	&#181;	micro sign
				//
				case (char) 0181:
					return "&micro;";

				//
				//	¶	¶	&#182;	paragraph sign = pilcrow sign
				//
				case (char) 0182:
					return "&para;";

				//
				//	·	·	&#183;	middle dot = georgian comma
				//
				case (char) 0183:
					return "&middot;";

				//
				//	¸	¸	&#184;	cedilla sign
				//
				case (char) 0184:
					return "&cedil;";

				//
				//	¹	¹	&#185;	superscript one
				//
				case (char) 0185:
					return "&sup1;";

				//
				//	º	º	&#186;	masculine ordinal indicator
				//
				case (char) 0186:
					return "&ordm;";

				//
				//	»	»	&#187;	right-pointing double angle quotes (right pointing quillemet)
				//
				case (char) 0187:
					return "&raquo;";

				//
				//	¼	¼	&#188;	vulgar fraction one quarter
				//
				case (char) 0188:
					return "&frac14;";

				//
				//	½	½	&#189;	vulgar fraction one half
				//
				case (char) 0189:
					return "&frac12;";

				//
				//	¾	¾	&#190;	vulgar fraction three quarters
				//
				case (char) 0190:
					return "&frac34;";

				//
				//	¿	¿	&#191;	inverted question mark
				//
				case (char) 0191:
					return "&iquest;";

				//
				//	À	À	&#192;	latin capital A with grave accent
				//
				case (char) 0192:
					return "&Agrave;";

				//
				//	Á	Á	&#193;	latin capital A with acute accent
				//
				case (char) 0193:
					return "&Aacute;";

				//
				//	Â	Â	&#194;	latin capital A with circumflex
				//
				case (char) 0194:
					return "&Acirc;";

				//
				//	Ã	Ã	&#195;	latin capital A with tilde
				//
				case (char) 0195:
					return "&Atilde;";

				//
				//	Ä	Ä	&#196;	latin capital A with diaeresis
				//
				case (char) 0196:
					return "&Auml;";

				//
				//	Å	Å	&#197;	latin capital A with ring
				//
				case (char) 0197:
					return "&Aring;";

				//
				//	Æ	Æ	&#198;	latin capital AE
				//
				case (char) 0198:
					return "&AElig;";

				//
				//	Ç	Ç	&#199;	latin capital C with cedilla
				//
				case (char) 0199:
					return "&Ccedil;";

				//
				//	È	È	&#200;	latin capital E with grave accent
				//
				case (char) 0200:
					return "&Egrave;";

				//
				//	É	É	&#201;	latin capital E with acute accent
				//
				case (char) 0201:
					return "&Eacute;";

				//
				//	Ê	Ê	&#202;	latin capital E with circumflex
				//
				case (char) 0202:
					return "&Ecirc;";

				//
				//	Ë	Ë	&#203;	latin capital E with diaeresis
				//
				case (char) 0203:
					return "&Euml;";

				//
				//	Ì	Ì	&#204;	latin capital I with grave accent
				//
				case (char) 0204:
					return "&Igrave;";

				//
				//	Í	Í	&#205;	latin capital I with acute accent
				//
				case (char) 0205:
					return "&Iacute;";

				//
				//	Î	Î	&#206;	latin capital I with circumflex
				//
				case (char) 0206:
					return "&Icirc;";

				//
				//	Ï	Ï	&#207;	latin capital I with diaeresis
				//
				case (char) 0207:
					return "&Iuml;";

				//
				//	Ð	Ð	&#208;	latin capital letter ETH
				//
				case (char) 0208:
					return "&ETH;";

				//
				//	Ñ	Ñ	&#209;	latin capital N with tilde
				//
				case (char) 0209:
					return "&Ntilde;";

				//
				//	Ò	Ò	&#210;	latin capital O with grave accent
				//
				case (char) 0210:
					return "&Ograve;";

				//
				//	Ó	Ó	&#211;	latin capital O with acute accent
				//
				case (char) 0211:
					return "&Oacute;";

				//
				//	Ô	Ô	&#212;	latin capital O with circumflex
				//
				case (char) 0212:
					return "&Ocirc;";

				//
				//	Õ	Õ	&#213;	latin capital O with tilde
				//
				case (char) 0213:
					return "&Otilde;";

				//
				//	Ö	Ö	&#214;	latin capital O with diaeresis
				//
				case (char) 0214:
					return "&Ouml;";

				//
				//	×	×	&#215;	multiplication sign
				//
				case (char) 0215:
					return "&times;";

				//
				//	Ø	Ø	&#216;	latin capital O with stroke
				//
				case (char) 0216:
					return "&Oslash;";

				//
				//	Ù	Ù	&#217;	latin capital U with grave accent
				//
				case (char) 0217:
					return "&Ugrave;";

				//
				//	Ú	Ú	&#218;	latin capital U with acute accent
				//
				case (char) 0218:
					return "&Uacute;";

				//
				//	Û	Û	&#219;	latin capital U with circumflex
				//
				case (char) 0219:
					return "&Ucirc;";

				//
				//	Ü	Ü	&#220;	latin capital U with diaeresis
				//
				case (char) 0220:
					return "&Uml;";

				//
				//	Ý	Ý	&#221;	latin capital Y with acute accent
				//
				case (char) 0221:
					return "&Yacute;";

				//
				//	Þ	Þ	&#222;	latin capital THORN
				//
				case (char) 0222:
					return "&THORN;";

				//
				//	ß	ß	&#223;	latin small letter sharp s
				//
				case (char) 0223:
					return "&szlig;";

				//
				//	à	à	&#224;	latin small letter a with grave accent
				//
				case (char) 0224:
					return "&agrave;";

				//
				//	á	á	&#225;	latin small letter a with acute accent
				//
				case (char) 0225:
					return "&aacute;";

				//
				//	â	â	&#226;	latin small letter a with circumflex
				//
				case (char) 0226:
					return "&acirc;";

				//
				//	ã	ã	&#227;	latin small letter a with tilde
				//
				case (char) 0227:
					return "&atilde;";

				//
				//	ä	ä	&#228;	latin small letter a with diaeresis
				//
				case (char) 0228:
					return "&auml;";

				//
				//	å	å	&#229;	latin small letter a with ring
				//
				case (char) 0229:
					return "&aring;";

				//
				//	æ	æ	&#230;	latin small letter ae
				//
				case (char) 0230:
					return "&aelig;";

				//
				//	ç	ç	&#231;	latin small letter c with cedilla
				//
				case (char) 0231:
					return "&ccedil;";

				//
				//	è	è	&#232;	latin small letter e with grave accent
				//
				case (char) 0232:
					return "&egrave;";

				//
				//	é	é	&#233;	latin small letter e with acute accent
				//
				case (char) 0233:
					return "&eacute;";

				//
				//	ê	ê	&#234;	latin small letter e with circumflex
				//
				case (char) 0234:
					return "&ecirc;";

				//
				//	ë	ë	&#235;	latin small letter e with diaeresis
				//
				case (char) 0235:
					return "&euml;";

				//
				//	ì	ì	&#236;	latin small letter i with grave accent
				//
				case (char) 0236:
					return "&igrave;";

				//
				//	í	í	&#237;	latin small letter i with acute accent
				//
				case (char) 0237:
					return "&iacute;";

				//
				//	î	î	&#238;	latin small letter i with circumflex
				//
				case (char) 0238:
					return "&icirc;";

				//
				//	ï	ï	&#239;	latin small letter i with diaeresis
				//
				case (char) 0239:
					return "&iuml;";

				//
				//	ð	ð	&#240;	latin small letter eth
				//
				case (char) 0240:
					return "&eth;";

				//
				//	ñ	ñ	&#241;	latin small letter n with tilde
				//
				case (char) 0241:
					return "&ntilde;";

				//
				//	ò	ò	&#242;	latin small letter 0 with grave accent
				//
				case (char) 0242:
					return "&ograve;";

				//
				//	ó	ó	&#243;	latin small letter 0 with acute accent
				//
				case (char) 0243:
					return "&oacute;";

				//
				//	ô	ô	&#244;	latin small letter 0 with circumflex
				//
				case (char) 0244:
					return "&ocirc;";

				//
				//	õ	õ	&#245;	latin small letter 0 with tilde
				//
				case (char) 0245:
					return "&otilde;";

				//
				//	ö	ö	&#246;	latin small letter 0 with diaeresis
				//
				case (char) 0246:
					return "&ouml;";

				//
				//	÷	÷	&#247;	division sign
				//
				case (char) 0247:
					return "&divide;";

				//
				//	ø	ø	&#248;	latin small letter 0 with stroke
				//
				case (char) 0248:
					return "&oslash;";

				//
				//	ù	ù	&#249;	latin small letter u with grave accent
				//
				case (char) 0249:
					return "&ugrave;";

				//
				//	ú	ú	&#250;	latin small letter u with acute accent
				//
				case (char) 0250:
					return "&uacute;";

				//
				//	û	û	&#251;	latin small letter u with circumflex
				//
				case (char) 0251:
					return "&ucirc;";

				//
				//	ü	ü	&#252;	latin small letter u with diareresis
				//
				case (char) 0252:
					return "&uuml;";

				//
				//	ý	ý	&#253;	latin small letter y with acute accent
				//
				case (char) 0253:
					return "&yacute;";

				//
				//	þ	þ	&#254;	latin small letter thorn
				//
				case (char) 0254:
					return "&thorn;";

				//
				//	ÿ	ÿ	&#255;	latin small letter y with diaeresis
				//
				case (char) 0255:
					return "&yuml;";


				default:
					return string.Empty;
			}
		}


		/////////////////////////////////////////////////////////////////////////////

		private static string htmlDecodeStr = @"&.*?;";
		private static Regex htmlDecodeRegex;

		public static string HtmlDecode( string str )
		{
			// ******
			if( null == htmlDecodeRegex ) {
				htmlDecodeRegex = new Regex( htmlDecodeStr );
			}
			
			// ******
			string result;
			if( string.IsNullOrEmpty(str) ) {
				result = string.Empty;
			}
			else  {
				result = htmlDecodeRegex.Replace( str, match => { return HtmlDecodeMatch(match.Value); } );
			}
			
			// ******
			return result;
		}
		
		
		///////////////////////////////////////////////////////////////////////////////

		public static string HtmlDecodeMatch( string str )
		{
			// ******
			switch( str ) {

				//
				// non-breaking space ISO8559-1
				//
				case "&nbsp;":
					return "\u00A0";		 

				//
				// copyright sign ISO8559-1 
				//
				case "&copy;":
					return "\u00A9";

				//
				// registered trade mark sign ISO8559-1 
				//
				case "&reg;	":
					return "\u00AE";

				//
				// superscript 2 (squared) ISO8559-1 
				//
				case "&sup2;":
					return "\u00B2";

				//
				// superscript 3 (cubed) ISO8559-1 
				//
				case "&sup3;":
					return "\u00B3";

				//
				// quotation mark ISO10646 
				//
				case "&quot;":
					return "\u0022";

				//
				// ampersand sign ISO10646 
				//
				case "&amp;	":
					return "\u0026";

				//
				// less than sign ISO10646 
				//
				case "&lt;":			
					return "\u003C";

				//
				// greater than sign ISO10646 
				//
				case "&gt;":			
					return "\u003E";

				//
				// en dash ISO10646 
				//
				case "&ndash;":		
					return "\u2013";

				//
				// em dash ISO10646 
				//
				case "&mdash;":		
					return "\u2014";

				//
				// left single quote ISO10646 
				//
				case "&lsquo;":		
					return "\u2018";

				//
				// right single quote, apostrophe ISO10646 
				//
				case "&rsquo;":		
					return "\u2019";

				//
				// left double quotation mark ISO10646 
				//
				case "&ldquo;":		
					return "\u201C";

				//
				// right double quotation mark ISO10646 
				//
				case "&rdquo;":		
					return "\u201D";

				//
				// small black circle, bullet ISO10646 
				//
				case "&bull;":		
					return "\u2022";

				//
				// dagger sign ISO10646 
				//
				case "&dagger;":	
					return "\u2020";

				//
				// double dagger sign ISO10646 
				//
				case "&Dagger;":	
					return "\u2021";

				//
				// prime = minutes = feet ISO10646 
				//
				case "&prime;":		
					return "\u2032";

				//
				// double prime = seconds = inches ISO10646 
				//
				case "&Prime;":		
					return "\u2033";

				//
				// single left pointing angle quote ISO10646 
				//
				case "&lsaquo;":	
					return "\u2039";

				//
				// single right pointing angle quote ISO10646 
				//
				case "&rsaquo;":	
					return "\u203A";

				//
				// euro sign ISO10646 
				//
				case "&euro;":		
					return "\u20AC";

				//
				// Registered Trademark sign ISO10646 
				//
				case "&trade;":		
					return "\u2122";

				//
				// tilde sign ISO10646 
				//
				case "&tilde;":		
					return "\u02DC";

				//
				// circumflex (or caret) sign ISO10646 
				//
				case "&circ;":		
					return "\u02C6";

				//
				// black spade suit ISO10646 
				//
				case "&spades;":	
					return "\u2660";

				//
				// black clubs suit ISO10646 
				//
				case "&clubs;":		
					return "\u2663";

				//
				// black heart suit ISO10646 
				//
				case "&hearts;":	
					return "\u2665";

				//
				// black diamonds suit ISO10646 
				//
				case "&diams;":		
					return "\u2666";

				//
				// lozenge ISO10646 
				//
				case "&loz;":			
					return "\u25CA";

				//
				// left arrow ISO10646 
				//
				case "&larr;":		
					return "\u2190";

				//
				// right arrow ISO10646 
				//
				case "&rarr;":		
					return "\u2192";

				//
				// up arrow ISO10646 
				//
				case "&uarr;":		
					return "\u2191";

				//
				// down arrow ISO10646 
				//
				case "&darr;":		
					return "\u2193";

				//
				// right-left arrow ISO10646 
				//
				case "&harr;":		
					return "\u2194";

				//
				// NOT sign ISO8859-1 
				//
				case "&not;":			
					return "\u00AC";




				//
				//	ï¿½	ï¿½	&#161;	inverted exclamation mark
				//
				case "&iexcl;":
					return "\u00A1";

				//
				//	ï¿½	ï¿½	&#162;	cent sign
				//
				case "&cent;":
					return "\u00A2";

				//
				//	ï¿½	ï¿½	&#163;	pound sign
				//
				case "&pound;":
					return "\u00A3";

				//
				//	ï¿½	ï¿½	&#164;	currency sign
				//
				case "&curren;":
					return "\u00A4";

				//
				//	ï¿½	ï¿½	&#165;	yen sign = yuan sign
				//
				case "&yen;":
					return "\u00A5";

				//
				//	ï¿½	ï¿½	&#166;	broken vertical bar
				//
				case "&brvbar;":
					return "\u00A6";

				//
				//	ï¿½	ï¿½	&#167;	section sign
				//
				case "&sect;":
					return "\u00A7";

				//
				//	ï¿½	ï¿½	&#168;	diaeresis = spacing diaeresis
				//
				case "&uml;":
					return "\u00A8";

				//
				//	ï¿½	ï¿½	&#170;	feminine ordinal indicator
				//
				case "&ordf;":
					return "\u00AA";

				//
				//	ï¿½	ï¿½	&#171;	left-pointing double angle quotes (left pointing quillemet)
				//
				case "&laquo;":
					return "\u00AB";

				//
				//	ï¿½	ï¿½	&#173;	soft hyphen
				//
				case "&shy;":
					return "\u00AD";

				//
				//	ï¿½	ï¿½	&#175;	macron = spacing macron
				//
				case "&macr;":
					return "\u00AF";

				//
				//	ï¿½	ï¿½	&#176;	degree sign
				//
				case "&deg;":
					return "\u00B0";

				//
				//	ï¿½	ï¿½	&#177;	plus-minus sign
				//
				case "&plusmn;":
					return "\u00B1";

				//
				//	ï¿½	ï¿½	&#180;	acute accent
				//
				case "&acute;":
					return "\u00B4";

				//
				//	ï¿½	ï¿½	&#181;	micro sign
				//
				case "&micro;":
					return "\u00B5";

				//
				//	ï¿½	ï¿½	&#182;	paragraph sign = pilcrow sign
				//
				case "&para;":
					return "\u00B6";

				//
				//	ï¿½	ï¿½	&#183;	middle dot = georgian comma
				//
				case "&middot;":
					return "\u00B7";

				//
				//	ï¿½	ï¿½	&#184;	cedilla sign
				//
				case "&cedil;":
					return "\u00B8";

				//
				//	ï¿½	ï¿½	&#185;	superscript one
				//
				case "&sup1;":
					return "\u00B9";

				//
				//	ï¿½	ï¿½	&#186;	masculine ordinal indicator
				//
				case "&ordm;":
					return "\u00BA";

				//
				//	ï¿½	ï¿½	&#187;	right-pointing double angle quotes (right pointing quillemet)
				//
				case "&raquo;":
					return "\u00BB";

				//
				//	ï¿½	ï¿½	&#188;	vulgar fraction one quarter
				//
				case "&frac14;":
					return "\u00BC";

				//
				//	ï¿½	ï¿½	&#189;	vulgar fraction one half
				//
				case "&frac12;":
					return "\u00BD";

				//
				//	ï¿½	ï¿½	&#190;	vulgar fraction three quarters
				//
				case "&frac34;":
					return "\u00BE";

				//
				//	ï¿½	ï¿½	&#191;	inverted question mark
				//
				case "&iquest;":
					return "\u00BF";

				//
				//	ï¿½	ï¿½	&#192;	latin capital A with grave accent
				//
				case "&Agrave;":
					return "\u00C0";

				//
				//	ï¿½	ï¿½	&#193;	latin capital A with acute accent
				//
				case "&Aacute;":
					return "\u00C1";

				//
				//	ï¿½	ï¿½	&#194;	latin capital A with circumflex
				//
				case "&Acirc;":
					return "\u00C2";

				//
				//	ï¿½	ï¿½	&#195;	latin capital A with tilde
				//
				case "&Atilde;":
					return "\u00C3";

				//
				//	ï¿½	ï¿½	&#196;	latin capital A with diaeresis
				//
				case "&Auml;":
					return "\u00C4";

				//
				//	ï¿½	ï¿½	&#197;	latin capital A with ring
				//
				case "&Aring;":
					return "\u00C5";

				//
				//	ï¿½	ï¿½	&#198;	latin capital AE
				//
				case "&AElig;":
					return "\u00C6";

				//
				//	ï¿½	ï¿½	&#199;	latin capital C with cedilla
				//
				case "&Ccedil;":
					return "\u00C7";

				//
				//	ï¿½	ï¿½	&#200;	latin capital E with grave accent
				//
				case "&Egrave;":
					return "\u00C8";

				//
				//	ï¿½	ï¿½	&#201;	latin capital E with acute accent
				//
				case "&Eacute;":
					return "\u00C9";

				//
				//	ï¿½	ï¿½	&#202;	latin capital E with circumflex
				//
				case "&Ecirc;":
					return "\u00CA";

				//
				//	ï¿½	ï¿½	&#203;	latin capital E with diaeresis
				//
				case "&Euml;":
					return "\u00CB";

				//
				//	ï¿½	ï¿½	&#204;	latin capital I with grave accent
				//
				case "&Igrave;":
					return "\u00CC";

				//
				//	ï¿½	ï¿½	&#205;	latin capital I with acute accent
				//
				case "&Iacute;":
					return "\u00CD";

				//
				//	ï¿½	ï¿½	&#206;	latin capital I with circumflex
				//
				case "&Icirc;":
					return "\u00CE";

				//
				//	ï¿½	ï¿½	&#207;	latin capital I with diaeresis
				//
				case "&Iuml;":
					return "\u00CF";

				//
				//	ï¿½	ï¿½	&#208;	latin capital letter ETH
				//
				case "&ETH;":
					return "\u00D0";

				//
				//	ï¿½	ï¿½	&#209;	latin capital N with tilde
				//
				case "&Ntilde;":
					return "\u00D1";

				//
				//	ï¿½	ï¿½	&#210;	latin capital O with grave accent
				//
				case "&Ograve;":
					return "\u00D2";

				//
				//	ï¿½	ï¿½	&#211;	latin capital O with acute accent
				//
				case "&Oacute;":
					return "\u00D3";

				//
				//	ï¿½	ï¿½	&#212;	latin capital O with circumflex
				//
				case "&Ocirc;":
					return "\u00D4";

				//
				//	ï¿½	ï¿½	&#213;	latin capital O with tilde
				//
				case "&Otilde;":
					return "\u00D5";

				//
				//	ï¿½	ï¿½	&#214;	latin capital O with diaeresis
				//
				case "&Ouml;":
					return "\u00D6";

				//
				//	ï¿½	ï¿½	&#215;	multiplication sign
				//
				case "&times;":
					return "\u00D7";

				//
				//	ï¿½	ï¿½	&#216;	latin capital O with stroke
				//
				case "&Oslash;":
					return "\u00D8";

				//
				//	ï¿½	ï¿½	&#217;	latin capital U with grave accent
				//
				case "&Ugrave;":
					return "\u00D9";

				//
				//	ï¿½	ï¿½	&#218;	latin capital U with acute accent
				//
				case "&Uacute;":
					return "\u00DA";

				//
				//	ï¿½	ï¿½	&#219;	latin capital U with circumflex
				//
				case "&Ucirc;":
					return "\u00DB";

				//
				//	ï¿½	ï¿½	&#220;	latin capital U with diaeresis
				//
				case "&Uml;":
					return "\u00DC";

				//
				//	ï¿½	ï¿½	&#221;	latin capital Y with acute accent
				//
				case "&Yacute;":
					return "\u00DD";

				//
				//	ï¿½	ï¿½	&#222;	latin capital THORN
				//
				case "&THORN;":
					return "\u00DE";

				//
				//	ï¿½	ï¿½	&#223;	latin small letter sharp s
				//
				case "&szlig;":
					return "\u00DF";

				//
				//	ï¿½	ï¿½	&#224;	latin small letter a with grave accent
				//
				case "&agrave;":
					return "\u00E0";

				//
				//	ï¿½	ï¿½	&#225;	latin small letter a with acute accent
				//
				case "&aacute;":
					return "\u00E1";

				//
				//	ï¿½	ï¿½	&#226;	latin small letter a with circumflex
				//
				case "&acirc;":
					return "\u00E2";

				//
				//	ï¿½	ï¿½	&#227;	latin small letter a with tilde
				//
				case "&atilde;":
					return "\u00E3";

				//
				//	ï¿½	ï¿½	&#228;	latin small letter a with diaeresis
				//
				case "&auml;":
					return "\u00E4";

				//
				//	ï¿½	ï¿½	&#229;	latin small letter a with ring
				//
				case "&aring;":
					return "\u00E5";

				//
				//	ï¿½	ï¿½	&#230;	latin small letter ae
				//
				case "&aelig;":
					return "\u00E6";

				//
				//	ï¿½	ï¿½	&#231;	latin small letter c with cedilla
				//
				case "&ccedil;":
					return "\u00E7";

				//
				//	ï¿½	ï¿½	&#232;	latin small letter e with grave accent
				//
				case "&egrave;":
					return "\u00E8";

				//
				//	ï¿½	ï¿½	&#233;	latin small letter e with acute accent
				//
				case "&eacute;":
					return "\u00E9";

				//
				//	ï¿½	ï¿½	&#234;	latin small letter e with circumflex
				//
				case "&ecirc;":
					return "\u00EA";

				//
				//	ï¿½	ï¿½	&#235;	latin small letter e with diaeresis
				//
				case "&euml;":
					return "\u00EB";

				//
				//	ï¿½	ï¿½	&#236;	latin small letter i with grave accent
				//
				case "&igrave;":
					return "\u00EC";

				//
				//	ï¿½	ï¿½	&#237;	latin small letter i with acute accent
				//
				case "&iacute;":
					return "\u00ED";

				//
				//	ï¿½	ï¿½	&#238;	latin small letter i with circumflex
				//
				case "&icirc;":
					return "\u00EE";

				//
				//	ï¿½	ï¿½	&#239;	latin small letter i with diaeresis
				//
				case "&iuml;":
					return "\u00EF";

				//
				//	ï¿½	ï¿½	&#240;	latin small letter eth
				//
				case "&eth;":
					return "\u00F0";

				//
				//	ï¿½	ï¿½	&#241;	latin small letter n with tilde
				//
				case "&ntilde;":
					return "\u00F1";

				//
				//	ï¿½	ï¿½	&#242;	latin small letter 0 with grave accent
				//
				case "&ograve;":
					return "\u00F2";

				//
				//	ï¿½	ï¿½	&#243;	latin small letter 0 with acute accent
				//
				case "&oacute;":
					return "\u00F3";

				//
				//	ï¿½	ï¿½	&#244;	latin small letter 0 with circumflex
				//
				case "&ocirc;":
					return "\u00F4";

				//
				//	ï¿½	ï¿½	&#245;	latin small letter 0 with tilde
				//
				case "&otilde;":
					return "\u00F5";

				//
				//	ï¿½	ï¿½	&#246;	latin small letter 0 with diaeresis
				//
				case "&ouml;":
					return "\u00F6";

				//
				//	ï¿½	ï¿½	&#247;	division sign
				//
				case "&divide;":
					return "\u00F7";

				//
				//	ï¿½	ï¿½	&#248;	latin small letter 0 with stroke
				//
				case "&oslash;":
					return "\u00F8";

				//
				//	ï¿½	ï¿½	&#249;	latin small letter u with grave accent
				//
				case "&ugrave;":
					return "\u00F9";

				//
				//	ï¿½	ï¿½	&#250;	latin small letter u with acute accent
				//
				case "&uacute;":
					return "\u00FA";

				//
				//	ï¿½	ï¿½	&#251;	latin small letter u with circumflex
				//
				case "&ucirc;":
					return "\u00FB";

				//
				//	ï¿½	ï¿½	&#252;	latin small letter u with diareresis
				//
				case "&uuml;":
					return "\u00FC";

				//
				//	ï¿½	ï¿½	&#253;	latin small letter y with acute accent
				//
				case "&yacute;":
					return "\u00FD";

				//
				//	ï¿½	ï¿½	&#254;	latin small letter thorn
				//
				case "&thorn;":
					return "\u00FE";

				//
				//	ï¿½	ï¿½	&#255;	latin small letter y with diaeresis
				//
				case "&yuml;":
					return "\u00FF";


			}

			// ******
			return string.Empty;
		}


	}

//}
