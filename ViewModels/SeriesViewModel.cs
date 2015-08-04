using System.Collections.Generic;

namespace PostcardsManager.ViewModels
{
    public class SeriesViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string PublisherName { get; set; }
        public string Description { get; set; }
        public List<PostcardMainPageViewModel> Postcards { get; set; } 
    }
}