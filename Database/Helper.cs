using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthOrDrinkDemiBruls.Database
{
    internal static class Helper
    {
        public static string GetDbPath(string databaseName)
        {
            var dbPath = string.Empty;

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                dbPath = Path.Combine(dbPath, databaseName);
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                dbPath = Path.Combine(dbPath, "..", "Library", databaseName);
            }

            return dbPath;
        }
    }
}
