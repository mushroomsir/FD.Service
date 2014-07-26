using System;
using System.Web;
using FD.Service.Model;

namespace FD.Service
{
	internal static class ExceptionHelper
	{
		public static void Throw403Exception(HttpContext context)
		{
			throw new HttpException(403,
				Language.Exception403 + context.Request.RawUrl);
		}

		public static void Throw404Exception(HttpContext context)
		{
			throw new HttpException(404,
               Language.Exception404 + context.Request.RawUrl);
		}

        public static void ThrowNotFoundService(Exception exception)
        {
            throw new SystemException(Language.NotFoundService, exception);
        }
	}
}
