using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ballast
{
	/// <summary>
	/// This class retrieves online data from the Steam store.
	/// Utilizing a mix of Steam Web API JSON and HTML scraping.
	/// </summary>
	public class StoreItem
	{
		/*no need to auto-initialize 'appid' as it will be overwritten
		by the input regardless of the input being invalid or empty*/
		public string appid { get; protected set; }
		public string name { get; protected set; } = "unknown"; //auto-initialize in case of exception
		public string description { get; protected set; } = "unknown";
		public List<string> developers { get; protected set; } = new List<string> { "unknown" };
		public int rating { get; protected set; } = 0;
		public string price { get; protected set; } = "unknown";
		public string imageURL { get; protected set; } = "unknown"; //set this to the url of a placeholder '?' image like iconURL
		public string iconURL { get; protected set; } = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3c/Bang_icon_32x32.svg/1024px-Bang_icon_32x32.svg.png";
		public List<string> tags { get; protected set; } = new List<string> { "unknown" };


		//constructor will set all the store item properties
		public StoreItem(string id)
		{
			appid = id;
			//Via JSON, Steam Web API will return most of the needed data
			getAppJSON();

			/* Data that the JSON does not have will be done through scraping.
			Originally, only JSON was used because Steam sometimes blocked the
			HTML via an age verification wall. This has been fixed and Ballast 
			can now bypass it using cookies, however, some games will have more 
			than one wall. The JSON retrieved through Steam's Web API is more reliable 
			then the scraping from the store, but there is some data such as 
			icons and rating that aren't given in the JSON. So scraping will do 
			the job for the remaining data */
			var client = new SteamWebClient();
			HtmlDocument steamDBDoc = client.GetPage($"http://store.steampowered.com/app/{appid}");

			iconURL = getIcon(steamDBDoc);
			rating = getRating(steamDBDoc);
		}

		void getAppJSON()
		{
			string appDetails = new WebClient().DownloadString($"https://store.steampowered.com/api/appdetails/?appids={appid}");
			JObject appJSON = JObject.Parse(appDetails);

			//Scanning throug the JSON.
			if (appJSON[appid]["data"] != null)
			{
				///Some Steam apps will not have JSON results (rare),
				///so exception handling must be done with extra care
				name = appJSON[appid]["data"]["name"].ToString();
				description = appJSON[appid]["data"]["short_description"].ToString();
				developers = JArray.Parse(appJSON[appid]["data"]["developers"].ToString()).ToObject<List<string>>();

				foreach (JObject categoryProperty in appJSON[appid]["data"]["categories"])
				{
					tags.Add(categoryProperty["description"].ToString());
				}

				if (appJSON[appid]["data"]["price_overview"] != null)
				{
					price = appJSON[appid]["data"]["price_overview"]["final_formatted"].ToString();
				}
				else if ((bool)appJSON[appid]["data"]["is_free"] == true)
				{
					price = "free";
				}

				imageURL = appJSON[appid]["data"]["header_image"].ToString();
			}
		}

		/*following functions are all searching the html source for
		 elements matching the properties we needs*/
		private static int getRating(HtmlDocument steamDB)
		{
			HtmlNode ratingNode = steamDB.DocumentNode.SelectSingleNode("//meta[@itemprop='ratingValue']");
			if (ratingNode != null)
			{
				HtmlAttribute ratingAttribute = ratingNode.Attributes["content"];
				return Convert.ToInt32(ratingAttribute.Value);
			}
			else
			{
				return 0;
			}

		}

		private static string getIcon(HtmlDocument steamDB)
		{
			HtmlNode iconNode = steamDB.DocumentNode.SelectSingleNode("//div[@class='apphub_AppIcon']//img");
			if (iconNode != null) { return iconNode.Attributes["src"].Value.Replace("%CDN_HOST_MEDIA_SSL%", "cdn.akamai.steamstatic.com"); }
			else { return "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3c/Bang_icon_32x32.svg/1024px-Bang_icon_32x32.svg.png"; }

		}

	}
}
