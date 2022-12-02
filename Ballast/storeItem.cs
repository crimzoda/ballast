using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Ballast
{
	public class storeItem
	{
		public string appid { get; protected set; }
		public string name { get; protected set; }
		public int rating { get; protected set; }
		public string description { get; protected set; }
		public string imageURL { get; protected set; }
		public string price { get; protected set; }
		public List<string> tags { get; protected set; }

		//constructor will set all the store item properties
		public storeItem(string id)
		{
			HtmlDocument steamDocument = new HtmlWeb().Load("http://store.steampowered.com/app/" + id);
			appid = id;
			name = getName(steamDocument);
			rating = getRating(steamDocument);
			description = getDescription(steamDocument);
			imageURL = "http://cdn.akamai.steamstatic.com/steam/apps/" + id + "/header.jpg";
			price = getPrice(steamDocument);
			tags = getTags(steamDocument);
		}

		/*following functions are all searching the html source for
		 elements matching the properties we needs*/
		private static string getName(HtmlDocument steamDoc)
		{
			HtmlNode node = steamDoc.DocumentNode.SelectSingleNode("//div[@class='apphub_AppName']");
			return node.InnerText;
		}

		private static string getDescription(HtmlDocument steamDoc)
		{
			HtmlNode node = steamDoc.DocumentNode.SelectSingleNode("//meta[@property='og:description']");
			HtmlAttribute descAttribute = node.Attributes["content"];
			return descAttribute.Value;
		}

		private static int getRating(HtmlDocument steamDoc)
		{
			HtmlNode node = steamDoc.DocumentNode.SelectSingleNode("//meta[@itemprop='ratingValue']");
			HtmlAttribute descAttribute = node.Attributes["content"];
			return Convert.ToInt32(descAttribute.Value);
		}

		private static string getPrice(HtmlDocument steamDoc)
		{
			HtmlNode priceNode = steamDoc.DocumentNode.SelectSingleNode("//div[@class='discount_final_price']");
			return (priceNode.InnerText);
		}

		private static List<string> getTags(HtmlDocument steamDoc)
		{
			//compile the app tags into a list
			HtmlNodeCollection gameTagCollection = steamDoc.DocumentNode.SelectNodes("//a[@class='app_tag']");
			List<string> gameTags = new List<string>();


			foreach (var gameTagNode in gameTagCollection)
			{
				gameTags.Add(gameTagNode.InnerText.Trim());
			}

			return (gameTags);

		}

	}
}
