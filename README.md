# ballast
A library for retrieving data from the Steam store.
Made with C#, a dependency being HTMLAgilityPack.

Library functionality:
  - Retrieve name
  - Retrieve description
  - Retrieve rating
  - Retrieve price
  - Retrieve tags
  - Retrieve header image
  - Retrieve icon
  
Here's an example code snippet:
```cs
using Ballast;

/*205100 is the appid of the game in the steam store
you can put whatever you want*/
storeItem item = new storeItem("205100");

/*although you would already know the appid
it would be useful to just have it as property anyway*/
Console.WriteLine("AppId: " + item.appid);
Console.WriteLine("Name: " + item.name);
Console.WriteLine("Description: " + item.description);
Console.WriteLine("Rating: " + item.rating);
/*right now this returns the final discounted price
future versions will have both discounted and base price*/
Console.WriteLine("Price: " + item.price);
Console.WriteLine(item.imageURL);
Console.WriteLine(item.iconURL);
//the tags property in storeItem is a List<string>
foreach (string tag in item.tags)
{
    Console.WriteLine(tag);
}
```
