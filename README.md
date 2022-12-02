# ballast
A sleek library for retrieving data from online video game stores.
Made with C#, a dependency being HTMLAgilityPack.

Currently only Steam is supported but there is work being done to implement
itch.io.

Library functionality:
  - Retrieve name
  - Retrieve description
  - Retrieve rating
  - Retrieve price
  - Retrieve tags
  - Retrieve header image
  
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
/*right now this returns the final discounted price
future versions will have both discounted and base price*/
Console.WriteLine("Price: " + item.price);
Console.WriteLine(item.imageURL);
//the tags property in storeItem is a List<string>
foreach (string tag in item.tags)
{
    Console.WriteLine(tag);
}
```
