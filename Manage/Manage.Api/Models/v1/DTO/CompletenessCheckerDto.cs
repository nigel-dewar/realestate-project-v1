using System.Collections.Generic;

namespace Manage.API.Models.v1.DTO
{
    public class CompletenessCheckerDto
    {
        public int ErrorCount { get; set; } = 0;

        public List<ErrorDto> ErrorMessages { get; set; } = new List<ErrorDto>();

        public bool Complete()
        {
            if (ErrorCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class ErrorDto
    {
        public int Code { get; set; }

        public string Item { get; set; }

        public string Error { get; set; }
    }
}