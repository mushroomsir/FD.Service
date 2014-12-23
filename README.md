

##News
December 06 2014: Version 1.0.0.8<br />
Add Register Route <br />



##Configuration
iis 7 Integrated Mode：
```webconfig   
<system.webServer>
    <modules>
     <add name="UrlRoutingModule" type="FD.Service.UrlRoutingModule,FD.Service"/>
    </modules>
</system.webServer>
````
iis 6
```webconfig   
<system.web>
 <httpModules>
  <add name="UrlRoutingModule" type="FD.Service.UrlRoutingModule,FD.Service"/>
 </httpModules>
</system.web>
````

### Register Service and Route on Global.asax
```csharp
    protected void Application_Start(object sender, EventArgs e)
        {
            FdRouteTable.RegisterService("FdServiceTest");

            FdRouteTable.RegisterRoute(
                name: "Default",
                url: "api/{controller}/{action}"
                );
        }
````
###Usage
API Statement：
```csharp
[FdService(SessionMode = SessionMode.Support, IsPublicAllMethod = true)]
[Auth(Message="Login Authorization")]
public class SchoolApi
{      
        [FdMethod]
        [Auth(Order = 1, Message = "Authorization")]
        [log(Order = 2, Message = "Log Record")]
        public static int GetPointsByID(int id)
        {
            return 10;
        }
        [FdMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Student> GetStudentList()
        {
            return new List<Student>()
            {
                new Student()
                {
                    Age = 13,
                    Name = "LOKI"
                },
                new Student()
                {
                    Age = 14,
                    Name = "Frigga"
                }
            };
        }
}
````

Client invoke：
````javascript
$.get("/api/SchoolApi/GetPointsByID/", { sid: 101, id: 100 }, function (data) {
            $("#textDetail").append("<br/>GetPointsByID:" + data);
 });
````


###Installation


FD.Service can be installed via the nuget UI (as [FD.Service](https://www.nuget.org/packages/FD.Service/)), or via the nuget package manager console:

   PM> Install-Package FD.Service
