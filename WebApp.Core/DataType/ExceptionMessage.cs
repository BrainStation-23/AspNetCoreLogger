using Microsoft.EntityFrameworkCore;
using System;

namespace WebApp.Core.DataType
{
    public static class DataException
    {
        public static string GetExceptionMessage(this Exception exception)
        {
            string message = exception.Message;

            // DbEntityValidationException
            // SqlException
            // ArgumentException
            // ArgumentNullException
            // DbUpdateConcurrencyException

            if (exception is DbUpdateException)
            {
                if (exception.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {

                    message = "Delete statement conflicted with other data.";
                }
            }

            return message;
        }
    }
}

/*
var message = exception.Message;
var sqlException = exception.GetBaseException() as SqlException;
if (sqlException?.Number == 547)
{
	message = "Can not delete due to reference table.";
}*/
