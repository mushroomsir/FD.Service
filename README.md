FD.Service
==========


Simple FD.Service example


```webconfig
 <system.webServer>
    <modules>
     <add name="UrlRoutingModule" type="FD.Service.UrlRoutingModule,FD.Service"/>
    </modules>
  </system.webServer>
````


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

````javascript
$.get("/api/SchoolApi/GetTeacherName/", null, function (data) {
            $("#textDetail").append("GetTeacherName: " + data);
});
````


