

##News
July 20th: Version 1.0.0.4 is released.
1.增加filter

May 27th: Version 1.0.0.3 is released.


###Code samples
Webconfig configuration：
```webconfig   
<system.webServer>
    <modules>
     <add name="UrlRoutingModule" type="FD.Service.UrlRoutingModule,FD.Service"/>
    </modules>
</system.webServer>
````

Method Statement：
```csharp
[FdService]
[Auth(Message="登录验证")]
public class SchoolApi
{      
        [FdMethod]
        [Auth(Order = 1, Message = "权限验证")]
        [log(Order = 2,Message="查询日志")]
        public static int GetPointsByID(int id,int sid)
        {
            return 10;
        }
}
````

Client invoke：
````javascript
$.get("/api/SchoolApi/GetPointsByID/", { sid: 101, id: 100 }, function (data) {
            $("#textDetail").append("<br/>GetPointsByID:" + data);
 });
````


