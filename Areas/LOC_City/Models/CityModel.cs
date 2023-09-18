using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace dbareas.Areas.LOC_City.Models
{
    public class CityModel
    {
        [Required]
        public int CityID { get; set; }
        [Required]
        public string CityName { get; set; }
        [Required]
        public int StateId { get; set; }
        [Required]
        public string CityCode { get; set; }
        [Required]
        public string CountryId { get; set; }
    }
    public class CityDDL
    {
        public int CityId{ get; set; }
        public string CityName { get; set; }
    }
}
