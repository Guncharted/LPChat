using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LPChat.Middleware
{
	public class ExceptionHandler
	{
		private readonly RequestDelegate _next;

		public ExceptionHandler(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch (Exception ex)
			{
				await HandleException(context, ex);
			}
		}

		// TODO. Re-implement!!!
		public async Task HandleException(HttpContext context, Exception exception)
		{
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			var error = context.Features.Get<IExceptionHandlerFeature>();

			if (error != null)
			{
				context.Response.Headers.Add("Application-Error", error.Error.Message);
				context.Response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");

				await context.Response.WriteAsync(error.Error.Message);
			}
		}
	}
}
