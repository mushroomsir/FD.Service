

##News

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
public class SchoolApi
{      
         [FdMethod]
        public static string GetTeacherName()
        {
            return "Odin";
        }
}
````

Client invoke：
````javascript
$.get("/api/SchoolApi/GetTeacherName/", null, function (data) {
            $("#textDetail").append("GetTeacherName: " + data);
});
````


