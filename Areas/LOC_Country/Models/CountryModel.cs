using System.ComponentModel.DataAnnotations;

namespace dbareas.Areas.LOC_Country.Models
{
    public class CountryModel
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }
    public class CountryDdModel
    {
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }

    }
}
