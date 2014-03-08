using System.Web;

namespace FD.Service
{
	public static class ExceptionHelper
	{
		public static void Throw403Exception(HttpContext context)
		{
			throw new HttpException(403,
				"很抱歉，您没有适合的权限访问该服务：" + context.Request.RawUrl);
		}

		public static void Throw404Exception(HttpContext context)
		{
			throw new HttpException(404,
				"没有找到能处理请求的服务类，当前请求地址：" + context.Request.RawUrl);
		}
	}
}
