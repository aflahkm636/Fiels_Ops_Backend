using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Ops.Application.DTO.SubscriptionPlanDto
{
    public class SubscriptionPlanCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? FrequencyInDays { get; set; }
        public decimal? MonthlyCharge { get; set; }
        public int? CreatedBy { get; set; }
    }

    public class SubscriptionPlanUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? FrequencyInDays { get; set; }
        public decimal? MonthlyCharge { get; set; }
        public int? ModifiedBy { get; set; }
    }

}
