

##News
July 20th: Version 1.0.0.4 is released.<br /> 
1.增加filter

May 27th: Version 1.0.0.3 is released.


###Code samples
iis 7+  Webconfig configuration：
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
 </<httpModules>
</system.web>
````

Method Statement：
```csharp
[FdService(SessionMode = SessionMode.Support, IsPublicAllMethod = true)]
[Auth(Message="登录验证")]
public class SchoolApi
{      
        [FdMethod]
        [Auth(Order = 1, Message = "权限验证")]
        [log(Order = 2,Message="日志记录")]
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


