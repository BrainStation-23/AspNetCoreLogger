using System;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Core.Exceptions
{
    public static class ExceptionExtension
    {
        public static string InnerExceptionMessage(this Exception exception)
        {
            while (exception.InnerException != null) exception = exception.InnerException;

            return exception.Message;
        }

        public static string InnerExceptionMessages(this Exception exception)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Exception Detail - ");

            int i = 1;
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine($"Level : { i } ");
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine(exception.Message);
                i++;
            }

            return sb.ToString();
        }


        //public static Task ShowMessage(this Exception exception)
        //{
        //    var exceptionName = exception.GetType().FullName;

        //    if (exception is DbEntityValidationException)
        //    {
        //        var ex = exception as DbEntityValidationException;
        //        var errorMessages = ex.EntityValidationErrors
        //            .SelectMany(x => x.ValidationErrors)
        //            .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //    }
        //    else if (exception is DbUpdateException)
        //    {
        //        var ex = exception as DbUpdateException;
        //        //var entity = ex.Entries.Single().GetDatabaseValues();

        //        var sb = new StringBuilder();
        //        sb.AppendLine(ex.InnerExceptionMessages());

        //        foreach (var eve in ex.Entries)
        //        {
        //            sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
        //        }

        //        throw new DbUpdateException(sb.ToString(), exception);
        //    }
        //    else if (exception is DbUpdateConcurrencyException)
        //    {
        //        var ex = exception as DbUpdateConcurrencyException;
        //        var entity = ex.Entries.Single().GetDatabaseValues();

        //        if (entity == null)
        //        {
        //            throw new ArgumentNullException("entity id not found");
        //        }
        //        else
        //        {
        //            throw new ArgumentNullException("entity is modified not found");
        //        }
        //    }
        //    else
        //    {
        //        throw exception;
        //    }
        //}
    }
}
