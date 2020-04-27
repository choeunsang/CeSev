
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ceSev.Models
{
    public class EconomicIdxModel
    {
        public EconomicIdxModel()
        {

        }

        //public string ID { get; set; }
        //public string CLASS_NAME { get; set; }
        //public string KEYSTAT_NAME { get; set; }
        //public string DATA_VALUE { get; set; }
        //public string CYCLE { get; set; }
        //public string UNIT_NAME { get; set; }
        //public string CREATE_DATE { get; set; }

        public List<EconomicIdxItem> EconList { get; set; }

        //public List<DateModel> Datelist { get; set; }

        public IEnumerable<SelectListItem> Datelist { get; set; }

        public IEnumerable<SelectListItem> ClassKindlist { get; set; }

        public IEnumerable<SelectListItem> KeyStatKindlist { get; set; }

        public int SelectedDateIdx { get; set; }

        public int SelectedClassKindIdx { get; set; }

        public int SelectedKeyStatKindIdx { get; set; }


    }
}