FD.Service
==========

简单的FD.Service例子


在webconfig里配置
```webconfig
<system.webServer>
    <handlers>
      <add name="fdService" verb="POST,GET" path="/fdapi/*.ashx" type="FD.Service.HandlerFactory, FD.Service" />
    </handlers>
</system.webServer>	
````

对外公开Api接口


```csharp

[FdService]
public class TestApi
{
        [FdMethod]
        public static string GetUserName()
        {
            return "佚名";
        }
}
````
调用方法
````javascript
$.get("/fdapi/TestApi.GetUserName.ashx",null, function(data) {
      alert(data);
});
````
其他更多使用方法，参考示例->[例子](https://github.com/mushroomsir/FD.Service/blob/master/FdServiceDemo/api/TestApi.cs)


