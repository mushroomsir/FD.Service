

##News
August 21th: Version 1.0.0.7<br />
自定义异常下，禁用iis 7集成模式下iis自定义错误<br />



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
###Usage
API Statement：
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


