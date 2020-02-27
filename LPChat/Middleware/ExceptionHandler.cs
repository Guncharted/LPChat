using LPChat.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
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
			context.Response.ContentType = "application/json";

			context.Response.StatusCode = exception switch
			{
				PasswordMismatchException _ => StatusCodes.Status400BadRequest,
				PersonNotFoundException _ => StatusCodes.Status404NotFound,
				DuplicateException _ => StatusCodes.Status409Conflict,
				_ => StatusCodes.Status500InternalServerError

			};

			context.Response.Headers.Add("Application-Error", exception.Message);
			context.Response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");

			var json = JsonConvert.SerializeObject(new
			{
				error = new
				{
					message = exception.Message,
					stackTrace = exception.StackTrace
				}
			});

			await context.Response.WriteAsync(json);
		}
	}
}
