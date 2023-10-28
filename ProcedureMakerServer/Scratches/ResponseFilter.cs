//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc;
//using ProcedureMakerServer.Scratches;

//public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
//{
//    // list of errors?
//    public HttpResponseExceptionFilter()
//    {

//    }

//    public int Order => int.MaxValue - 10;

//    public void OnActionExecuting(ActionExecutingContext context) { }

//    public void OnActionExecuted(ActionExecutedContext context)
//    {
//        if (context.Exception is MyInvalidExceptionBase myInvalidException)
//        {
//            context.Result = new ObjectResult(myInvalidException.Data)
//            {
//                StatusCode = myInvalidException.StatusCode,
//            };

//            context.ExceptionHandled = true;
//        }
//    }
//}