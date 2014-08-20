<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Transactions.Bridge.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\SMDiagnostics.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.EnterpriseServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.IdentityModel.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.IdentityModel.Selectors.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Messaging.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.DurableInstancing.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceModel.Activation.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceModel.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceModel.Internals.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceProcess.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.ApplicationServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Services.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <NuGetReference>Microsoft.Net.Http</NuGetReference>
  <NuGetReference>Moq</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>Rx-Core</NuGetReference>
  <NuGetReference>Rx-Interfaces</NuGetReference>
  <NuGetReference>Rx-Linq</NuGetReference>
  <NuGetReference>Rx-Main</NuGetReference>
  <NuGetReference>Rx-PlatformServices</NuGetReference>
  <NuGetReference>XmlUnit.Xunit</NuGetReference>
  <Namespace>Castle.DynamicProxy</Namespace>
  <Namespace>Castle.DynamicProxy.Generators</Namespace>
  <Namespace>Moq</Namespace>
  <Namespace>Moq.Language</Namespace>
  <Namespace>Moq.Language.Flow</Namespace>
  <Namespace>Moq.Protected</Namespace>
  <Namespace>Moq.Proxy</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Joins</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.PlatformServices</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Threading.Tasks</Namespace>
  <Namespace>System.ServiceModel.Syndication</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>XmlUnit.Xunit</Namespace>
  <Namespace>Xunit</Namespace>
  <Namespace>Xunit.Sdk</Namespace>
</Query>

class Foo<T>
{
    public T Baza { get; set; }
    public object Property2 { get; set; }
}
class Baz {
    public object Foo { get; set; }
    public object Bar { get; set; }
}
void Main()
{
    //123.DumpBrowser();
    //new[] {1,2,3,4}.DumpBrowser();
    //new List<string>() { "yer", "ber" }.DumpBrowser();
    //new { Foo = "Foo", Bar = 123, Baz = 12M }.DumpBrowser();
    //new Foo<int>() { Baza = 123, Property2 = "asdad"}.DumpBrowser();
	new object[] { 1,"st",3}.DumpBrowser();
    //new [] { new Baz { Foo = 7, Bar = 4 }, new Baz { Foo = 1, Bar = 2 } }.DumpBrowser();
   	//new Dictionary<int, string>() { {1, "12"}, {10, "123"}, }.DumpBrowser();
	//new Hashtable() { {1, "12"}, {10, "123"}, }.DumpBrowser();
	
   
//    new Exception("hello").Dump();
//    new Exception("hello").DumpBrowser();
}

// Define other methods and classes here