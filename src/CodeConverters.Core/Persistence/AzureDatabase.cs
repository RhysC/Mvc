using System;
using System.Data.SqlClient;
using log4net;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CodeConverters.Core.Persistence
{
    public static class AzureDatabase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AzureDatabase));

        public static void ThrowIfConnectionUnavailable()
        {
            try
            {
                Logger.Info("Trying to verify database connectivity...");
                RetryOperation.For(() =>
                {
                    var cxString = RoleEnvironment.GetConfigurationSettingValue("DbConnectionString");
                    var con = new SqlConnection(cxString);
                    var dataSource = con.DataSource;
                    try
                    {
                        using (con)
                        {
                            con.Open();
                            Logger.Info("Verified database connectivity!");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unable to open database connection: " + dataSource, ex);
                    }
                }, 10, 6);
            }
            catch (Exception e)
            {
                Logger.Error("Failed to connect to database. Failing fast!", e);
                throw;
            }
        }
    }
}