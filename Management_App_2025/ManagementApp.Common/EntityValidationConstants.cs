namespace ManagementApp.Common
{
    public static class EntityValidationConstants
    {
        public static class DepartmentValidationConstants
        {
            public const int DepartmentNameMinLength = 2;
            public const int DepartmentNameMaxLength = 100;
        }

        public static class JobTitleValidationConstants
        {
            public const int JobTitleNameMinLength = 2;
            public const int JobTitleNameMaxLength = 50;
        }

        public static class UserValidationConstants
        {
            public const int UserFirstNameMinLength = 2;
            public const int UserFirstNameMaxLength = 50;
            public const int UserLastNameMinLength = 2;
            public const int UserLastNameMaxLength = 50;
            public const double SalaryMinAmount = 0.00;
            public const double SalaryMaxAmount = 10_000.00;
        }
    }
}
