// See https://aka.ms/new-console-template for more information
using ConsoleApp1;

Console.WriteLine("Hello, World!");


var apiHelper = new ApiHelper();

var parma = new { UserID = "James" };
var URI = "http://localhost:7001/api/v1/Session";
var result = await apiHelper.GetApi<object>(URI, parma);

Console.WriteLine(result);
Console.ReadKey();