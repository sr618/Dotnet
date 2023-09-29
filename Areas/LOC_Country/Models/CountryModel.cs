
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace dbareas.Areas.LOC_Country.Models
{
    public class CountryModel
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CountryNames { get; set; }
    }
    public class CountryDdModel
    {
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }

    }
    public class Mainmodel
    {
        public CountryModel CountryModel { get; set; }
      public DataTable  DataTable { get; set; }
    }
}
