using Newtonsoft.Json;
using ManagementApp.Data.DataProcessor.ImportDtos;

using static ManagementApp.Common.ApplicationConstants;

namespace ManagementApp.Data.DataProcessor
{
    internal static class Deserializer
    {
        private static string GenerateFilePath()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryName = Path.GetFileName(currentDirectory);
            string filePath = directoryName + DataSetsPath + DataSetsFile;


            return filePath;
        }

        internal static DepartmentImportDto[] GenerateDepartmentImportDtos()
        {
            string filePath = GenerateFilePath();
            return JsonConvert.DeserializeObject<DepartmentImportDto[]>(File.ReadAllText(filePath))!;
        }

        internal static JobTitleImportDto[] GenerateJobTitleImportDtos()
        {
            string filePath = GenerateFilePath();
            return JsonConvert.DeserializeObject<JobTitleImportDto[]>(File.ReadAllText(filePath))!;
        }

        internal static UserImportDto[] GenerateUserImportDtos()
        {
            string filePath = GenerateFilePath();
            return JsonConvert.DeserializeObject<UserImportDto[]>(File.ReadAllText(filePath))!;
        }
    }
}
