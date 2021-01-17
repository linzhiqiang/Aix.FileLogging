using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aix.FileLogging.Utils
{
    internal static class With
    {
        public static void NoException(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
            }
        }
        public static void NoException(ILogger logger, Action action, string message)
        {
            if (action == null) return;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                //logger.LogError($"{message}, {ex.Message}, {ex.StackTrace}");
                logger.LogError(ex, message);
            }
        }
        public static async Task NoException(Func<Task> action)
        {
            if (action == null) return;
            try
            {
                await action();
            }
            catch (Exception)
            {
            }
        }

        public static async Task NoException(ILogger logger, Func<Task> action, string message)
        {
            if (action == null) return;
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, message);
            }
        }
    }
}
