using System;
using System.Collections.Generic;
using System.Text;

namespace ApexTrackerConsoleApp.Models
{
    public class OriginSearchUsersDTO
    {
        public int totalCount { get; set; }
        public List<OriginUserDTO> infoList { get; set; }

    }
}
