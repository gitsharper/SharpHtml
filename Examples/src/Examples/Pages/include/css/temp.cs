

yield return new StylesheetBlock( "html" )
	.Color( "#222" )
	.Font-size( "1em" )
	.Line-height( "1.4" )
	;

yield return new StylesheetBlock( "-moz-selection" )
	.Background( "#b3d4fc" )
	.Text-shadow( "none" )
	;

yield return new StylesheetBlock( "selection" )
	.Background( "#b3d4fc" )
	.Text-shadow( "none" )
	;

yield return new StylesheetBlock( "hr" )
	.Display( "yield return new StylesheetBlock" )
	.Height( "1px" )
	.Border( "0" )
	.Border-top( "1px solid #ccc" )
	.Margin( "1em 0" )
	.Padding( "0" )
	;

yield return new StylesheetBlock( "audio, canvas, iframe, img, svg, video" )
	.Vertical-align( "middle" )
	;

yield return new StylesheetBlock( "fieldset" )
	.Border( "0" )
	.Margin( "0" )
	.Padding( "0" )
	;

yield return new StylesheetBlock( "textarea" )
	.Resize( "vertical" )
	;

yield return new StylesheetBlock( ".browserupgrade" )
	.Margin( "0.2em 0" )
	.Background( "#ccc" )
	.Color( "#000" )
	.Padding( "0.2em 0" )
	;

yield return new StylesheetBlock( "body" )
	.Font( "16px/26px Helvetica, Helvetica Neue, Arial" )
	;

yield return new StylesheetBlock( ".wrapper" )
	.Width( "90%" )
	.Margin( "0 5%" )
	;

yield return new StylesheetBlock( ".header-container" )
	.Border-bottom( "20px solid #e44d26" )
	;

yield return new StylesheetBlock( ".footer-container, .main aside" )
	.Border-top( "20px solid #e44d26" )
	;

yield return new StylesheetBlock( ".header-container, .footer-container, .main aside" )
	.Background( "#f16529" )
	;

yield return new StylesheetBlock( ".title" )
	.Color( "white" )
	;

yield return new StylesheetBlock( "nav ul" )
	.Margin( "0" )
	.Padding( "0" )
	.List-style-type( "none" )
	;

yield return new StylesheetBlock( "nav a" )
	.Display( "yield return new StylesheetBlock" )
	.Margin-bottom( "10px" )
	.Padding( "15px 0" )
	.Text-align( "center" )
	.Text-decoration( "none" )
	.Font-weight( "bold" )
	.Color( "white" )
	.Background( "#e44d26" )
	;

yield return new StylesheetBlock( "nav a( "hover, nav a( "visited" )
	.Color( "white" )
	;

yield return new StylesheetBlock( "nav a( "hover" )
	.Text-decoration( "underline" )
	;

yield return new StylesheetBlock( ".main" )
	.Padding( "30px 0" )
	;

yield return new StylesheetBlock( ".main article h1" )
	.Font-size( "2em" )
	;

yield return new StylesheetBlock( ".main aside" )
	.Color( "white" )
	.Padding( "0px 5% 10px" )
	;

yield return new StylesheetBlock( ".footer-container footer" )
	.Color( "white" )
	.Padding( "20px 0" )
	;

yield return new StylesheetBlock( ".ie7 .title" )
	.Padding-top( "20px" )
	;


yield return new StylesheetBlock( ".hidden" )
	.Display( "none !important" )
	.Visibility( "hidden" )
	;

yield return new StylesheetBlock( ".visuallyhidden" )
	.Border( "0" )
	.Clip( "rect( "0 0 0 0" )" )
	.Height( "1px" )
	.Margin( "-1px" )
	.Overflow( "hidden" )
	.Padding( "0" )
	.Position( "absolute" )
	.Width( "1px" )
	;

yield return new StylesheetBlock( ".visuallyhidden.focusable( "active, .visuallyhidden.focusable( "focus" )
	.Clip( "auto" )
	.Height( "auto" )
	.Margin( "0" )
	.Overflow( "visible" )
	.Position( "static" )
	.Width( "auto" )
	;

yield return new StylesheetBlock( ".invisible" )
	.Visibility( "hidden" )
	;

yield return new StylesheetBlock( ".clearfix( "before, .clearfix( "after" )
	.Content( "" )
	.Display( "table" )
	;

yield return new StylesheetBlock( ".clearfix( "after" )
	.Clear( "both" )
	;

//yield return new StylesheetBlock( ".clearfix" )
//	*zoom( "1" )





//yield return new StylesheetBlock( "@media only screen and ( "min-width( "480px" )" )
//
//
//	nav a {
//		float( "left" )
//		width( "27%" )
//		margin( "0 1.7%" )
//		padding( "25px 2%" )
//		margin-bottom( "0" )
//	
//
//
//yield return new StylesheetBlock( "nav li( "first-child a" )
//
//		margin-left( "0" )
//	
//
//
//yield return new StylesheetBlock( "nav li( "last-child a" )
//
//		margin-right( "0" )
//	
//
//
//yield return new StylesheetBlock( "nav ul li" )
//
//		display( "inline" )
//	
//
//
//yield return new StylesheetBlock( ".oldie nav a" )
//
//		margin( "0 0.7%" )
//	
//
//
//yield return new StylesheetBlock( "@media only screen and ( "min-width( "768px" )" )
//
//
//	.header-container,	 .main aside {
//		-webkit-box-shadow( "0 5px 10px #aaa" )
//		   -moz-box-shadow( "0 5px 10px #aaa" )
//				box-shadow( "0 5px 10px #aaa" )
//	
//
//
//yield return new StylesheetBlock( ".title" )
//
//		float( "left" )
//	
//
//
//yield return new StylesheetBlock( "nav" )
//
//		float( "right" )
//		width( "38%" )
//	
//
//
//yield return new StylesheetBlock( ".main article" )
//
//		float( "left" )
//		width( "57%" )
//	
//
//
//yield return new StylesheetBlock( ".main aside" )
//
//		float( "right" )
//		width( "28%" )
//	
//
//
//yield return new StylesheetBlock( "@media only screen and ( "min-width( "1140px" )" )
//
//
//
//	.wrapper {
//		width( "1026px" ) /* 1140px - 10% for margins */
//		margin( "0 auto" )
//	
//
//
//yield return new StylesheetBlock( ".hidden" )
//
//	display( "none !important" )
//	visibility( "hidden" )
//
//
//
//yield return new StylesheetBlock( ".visuallyhidden" )
//
//	border( "0" )
//	clip( "rect( "0 0 0 0" )" )
//	height( "1px" )
//	margin( "-1px" )
//	overflow( "hidden" )
//	padding( "0" )
//	position( "absolute" )
//	width( "1px" )
//
//
//
//yield return new StylesheetBlock( ".visuallyhidden.focusable( "active, .visuallyhidden.focusable( "focus" )
//
//	clip( "auto" )
//	height( "auto" )
//	margin( "0" )
//	overflow( "visible" )
//	position( "static" )
//	width( "auto" )
//
//
//
//yield return new StylesheetBlock( ".invisible" )
//
//	visibility( "hidden" )
//
//
//
//yield return new StylesheetBlock( ".clearfix( "before, .clearfix( "after" )
//
//	content( "" )
//	display( "table" )
//
//
//
//yield return new StylesheetBlock( ".clearfix( "after" )
//
//	clear( "both" )
//
//
//
//yield return new StylesheetBlock( ".clearfix" )
//
//	*zoom( "1" )
//
//
//
//yield return new StylesheetBlock( "@media print" )
//
//	*,	 *( "before,	 *( "after {
//		background( "transparent !important" )
//		color( "#000 !important" )
//		box-shadow( "none !important" )
//		text-shadow( "none !important" )
//	
//
//
//yield return new StylesheetBlock( "a,	 a( "visited" )
//
//		text-decoration( "underline" )
//	
//
//
//yield return new StylesheetBlock( "a[href]( "after" )
//
//		content( " ( " attr( "href" )" )" )
//	
//
//
//yield return new StylesheetBlock( "abbr[title]( "after" )
//
//		content( " ( " attr( "title" )" )" )
//	
//
//
//yield return new StylesheetBlock( "a[href^="#"]( "after,	 a[href^="javascript( "]( "after" )
//
//		content( "" )
//	
//
//
//yield return new StylesheetBlock( "pre,	 yield return new StylesheetBlockquote" )
//
//		border( "1px solid #999" )
//		page-break-inside( "avoid" )
//	
//
//
//yield return new StylesheetBlock( "thead" )
//
//		display( "table-header-group" )
//	
//
//
//yield return new StylesheetBlock( "tr,	 img" )
//
//		page-break-inside( "avoid" )
//	
//
//
//yield return new StylesheetBlock( "img" )
//
//		max-width( "100% !important" )
//	
//
//
//yield return new StylesheetBlock( "p,	 h2,	 h3" )
//
//		orphans( "3" )
//		widows( "3" )
//	
//
//
//yield return new StylesheetBlock( "h2,	 h3" )
//
//		page-break-after( "avoid" )
//	
//
//