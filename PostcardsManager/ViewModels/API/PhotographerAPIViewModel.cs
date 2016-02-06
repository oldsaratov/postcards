using Newtonsoft.Json;
using PostcardsManager.Models;

namespace PostcardsManager.ViewModels
{
    public class PhotographerAPIViewModel
    {
        public PhotographerAPIViewModel(Photographer photographer)
        {
            Id = photographer.Id;
            LastName = photographer.LastName;
            MiddleName = photographer.MiddleName;
            FirstName = photographer.FirstName;
        }

        public PhotographerAPIViewModel()
        {
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("fullName")]
        public string FullName
        {
            get { return string.Join(" ", LastName, MiddleName, FirstName); }
        }
    }
}