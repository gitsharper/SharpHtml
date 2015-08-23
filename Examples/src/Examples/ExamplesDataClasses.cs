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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SharpHtml;

using static SharpHtml.TableTemplate;

namespace Examples {

	/////////////////////////////////////////////////////////////////////////////

	public class TransactionData : List<TransactionData.Data> {

		public class Data {
			public DateTime Date { get; set; }
			public decimal Amount { get; set; } = 0m;
			public string Category { get; set; } = string.Empty;
			public string Detail { get; set; } = string.Empty;
		}

		/////////////////////////////////////////////////////////////////////////////

		public void Add( string date, string category, decimal amt, string detail )
		{
			Data d;
			Add( d = new Data { } );
			d.Date = DateTime.Parse( date );
			d.Amount = amt;
			d.Category = category;
			d.Detail = detail;
		}

		/////////////////////////////////////////////////////////////////////////////

		//
		// returns list of groups where each group represents a "category" and
		// associated Data entries
		//

		public static IEnumerable<IGrouping<string, Data>> Create()
		{
			var transactions = new TransactionData {
				{ "21 Jul 2015",  "Auto", -250.00m, "BILL PAY SAFE CREDIT UNIO ON-LINE" },
				{ "21 Jul 2015",  "Auto", -215.00m, "BILL PAY SAFE CREDIT UNIO ON-LINE" },
				{ "21 Jul 2015",  "Phone", -120.29m, "BILL PAY VERIZON WIRELESS ON-LINE" },
				{ "21 Jul 2015",  "Storage", -65.00m, "BILL PAY Extra Space Stor RECURRING" },
				{ "21 Jul 2015",  "Phone", -52.16m, "BILL PAY FRONTIER, A CITI ON-LINE" },
				{ "17 Jul 2015",  "House", -60.00m, "BILL PAY MARTIN BAUTISTA RECURRING" },
				{ "07 Jul 2015",  "House", -1791.26m, "BILL PAY CHASE HOME FINAN ON-LINE" },
				{ "07 Jul 2015",  "PayCredit", -323.87m, "BILL PAY BANK OF AMERICA ON-LINE" },
				{ "07 Jul 2015",  "House", -64.00m, "BILL PAY Stonelake Master ON-LINE" },
				{ "7 Jul 2015",   "Credit", -524.98m, "Lunch at Halos" },

				{ "21 June 2015", "Auto", -250.00m, "BILL PAY SAFE CREDIT UNIO ON-LINE" },
				{ "21 June 2015", "Auto", -215.00m, "BILL PAY SAFE CREDIT UNIO ON-LINE" },
				{ "21 June 2015", "Phone", -120.29m, "BILL PAY VERIZON WIRELESS ON-LINE" },
				{ "21 June 2015", "Storage", -65.00m, "BILL PAY Extra Space Stor RECURRING" },
				{ "21 June 2015", "Phone", -52.16m, "BILL PAY FRONTIER, A CITI ON-LINE" },
				{ "17 June 2015", "House", -60.00m, "BILL PAY MARTIN BAUTISTA RECURRING" },
				{ "07 June 2015", "House", -1791.26m, "BILL PAY CHASE HOME FINAN ON-LINE" },
				{ "07 June 2015", "PayCredit", -93.01m, "BILL PAY BANK OF AMERICA ON-LINE" },
				{ "07 June 2015", "House", -64.00m, "BILL PAY Stonelake Master ON-LINE" },
				{ "7 June 2015",  "Credit", -89.56m, "Lunch at Dietz" },

				{ "21 May 2015",  "Auto", -250.00m, "BILL PAY SAFE CREDIT UNIO ON-LINE" },
				{ "21 May 2015",  "Auto", -215.00m, "BILL PAY SAFE CREDIT UNIO ON-LINE" },
				{ "21 May 2015",  "Phone", -120.29m, "BILL PAY VERIZON WIRELESS ON-LINE" },
				{ "21 May 2015",  "Storage", -65.00m, "BILL PAY Extra Space Stor RECURRING" },
				{ "21 May 2015",  "Phone", -52.16m, "BILL PAY FRONTIER, A CITI ON-LINE" },
				{ "17 May 2015",  "House", -60.00m, "BILL PAY MARTIN BAUTISTA RECURRING" },
				{ "07 May 2015",  "House", -1791.26m, "BILL PAY CHASE HOME FINAN ON-LINE" },
				{ "07 May 2015",  "PayCredit", -5.0m, "BILL PAY BANK OF AMERICA ON-LINE" },
				{ "07 May 2015",  "House", -64.00m, "BILL PAY Stonelake Master ON-LINE" },
				{ "7 May 2015",   "Credit", -383.22m, "Lunch at Londons" },
			};

			var ordered = transactions.OrderBy( t => t.Date );
			var grouped = ordered.GroupBy( t => t.Category ).ToList();

			return grouped;

		}

	}

	/////////////////////////////////////////////////////////////////////////////

	public class AccountSummaryData : List<AccountSummaryData.Data> {

		public class Data {
			public DateTime Col1 { get; set; }
			public decimal Col2 { get; set; } = 0;
			public decimal Col3 { get; set; } = 0;
			public decimal Col4 { get; set; } = 0;
			public decimal Col5 { get; set; } = 0;
			public decimal Col6 { get; set; } = 0;
			public decimal Col7 { get; set; } = 0;
		}

		/////////////////////////////////////////////////////////////////////////////

		public void Add( string s1, decimal d1, decimal d2, decimal d3, decimal d4, decimal d5, decimal d6 )
		{
			Data d;
			Add( d = new Data { } );
			d.Col1 = DateTime.Parse( s1 ); d.Col2 = d1; d.Col3 = d2; d.Col4 = d3; d.Col5 = d4; d.Col6 = d5; d.Col7 = d6;
		}


		/////////////////////////////////////////////////////////////////////////////

		public static AccountSummaryData Create()
		{
			return new AccountSummaryData {
				{ "07 Aug 2015", 2256.38m, 4181.81m, -1925.43m, 4056.74m, 5581.50m, -1524.76m },
				{ "31 Jul 2015", 5836.41m, 6258.67m, -422.26m, 6144.27m, 6400.80m, -256.53m },
				{ "30 Jun 2015", 4077.42m, 6304.02m, -2226.60m, 6660.57m, 6945.06m, -284.49m },
				{ "29 May 2015", 8518.98m, 6639.72m, 1879.26m, 7524.96m, 7594.78m, -69.82m },
				{ "30 Apr 2015", 7385.32m, 7891.44m, -506.12m, 7725.14m, 8204.91m, -479.77m },
				{ "31 Mar 2015", 6670.57m, 8253.18m, -1582.61m, 8023.20m, 8105.12m, -81.91m },
				{ "27 Feb 2015", 9119.52m, 8470.10m, 649.42m, 7902.59m, 7885.88m, 16.71m },
				{ "30 Jan 2015", 8279.52m, 7592.07m, 687.45m, 7393.23m, 7336.44m, 56.78m },
				{ "31 Dec 2014", 6308.74m, 7595.48m, -1286.74m, 7092.90m, 6218.56m, 874.34m },
				{ "28 Nov 2014", 7591.42m, 6821.78m, 769.64m, 7415.47m, 7664.33m, -248.86m },
				{ "31 Oct 2014", 7378.55m, 4238.43m, 3140.12m, 8222.23m, 7962.84m, 259.39m },
				{ "30 Sep 2014", 7276.43m, 11932.78m, -4656.35m, 8141.03m, 9360.53m, -1219.50m },
				{ "29 Aug 2014", 10011.72m, 7717.32m, 2294.40m, 7726.03m, 8282.67m, -556.64m },
				{ "31 Jul 2014", 7134.93m, 8431.49m, -1296.56m, 7142.07m, 10462.32m, -3320.25m },
				{ "30 Jun 2014", 6031.45m, 8699.21m, -2667.76m, 12015.91m, 12600.54m, -584.63m },
				{ "30 May 2014", 8259.83m, 14256.27m, -5996.44m, 10976.41m, 12694.37m, -1717.96m },
				{ "30 Apr 2014", 21756.45m, 14846.13m, 6910.32m, 13458.87m, 12404.63m, 1054.24m },
				{ "31 Mar 2014", 2912.95m, 8980.71m, -6067.76m, 0.00m, 0.00m, 0.00m },
				{ "28 Feb 2014", 15707.21m, 13387.04m, 2320.17m, 0.00m, 0.00m, 0.00m }
			};
		}

	}


	/////////////////////////////////////////////////////////////////////////////

	public static class SimplePageData {

		/////////////////////////////////////////////////////////////////////////////

		static string loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam sodales urna non odio egestas tempor. Nunc vel vehicula ante. Etiam bibendum iaculis libero, eget molestie nisl pharetra in. In semper consequat est, eu porta velit mollis nec.";

		public static IEnumerable<Tag> initializrBodyContent()
		{
			Tag outerTag;
			Tag tag;

			// ******
			outerTag = new Article { };
			{
				outerTag.AddChild( tag = new Header { } );
				tag.AddChild( new H1( "article header h1" ) );
				tag.AddChild( new P( loremIpsum ) );

				// ******
				outerTag.AddChild( tag = new Section { } );
				tag.AddChild( new H2( "article section h2" ) );
				tag.AddChild( new P( loremIpsum ) );

				// ******
				outerTag.AddChild( tag = new Section { } );
				tag.AddChild( new H2( "article section h2" ) );
				tag.AddChild( new P( loremIpsum ) );

				// ******
				outerTag.AddChild( tag = new Footer { } );
				tag.AddChild( new H2( "article footer h3" ) );
				tag.AddChild( new P( loremIpsum ) );

				yield return outerTag;
			}

			// ******
			outerTag = new Aside { };
			{
				outerTag.AddChild( tag = new H3( "asside" ) );
				tag.AddChild( new P( loremIpsum ) );

				yield return outerTag;
			}

		}





	}




}
