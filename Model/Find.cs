namespace project.Model
{
    public class Rating
    {
        public string FilmId { get; set; }
        public string Score { get; set; }
    }
    public class Find
    {
        public string[] resultsSectionOrder { get; set; }
        public Findpagemeta findPageMeta { get; set; }
        public Keywordresults keywordResults { get; set; }
        public Titleresults titleResults { get; set; }
        public Nameresults nameResults { get; set; }
        public Companyresults companyResults { get; set; }
    }

    public class Findpagemeta
    {
        public string searchTerm { get; set; }
        public bool includeAdult { get; set; }
        public bool isExactMatch { get; set; }
    }

    public class Keywordresults
    {
        public object[] results { get; set; }
    }

    public class Titleresults
    {
        public Result[] results { get; set; }
        public string nextCursor { get; set; }
        public bool hasExactMatches { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string titleNameText { get; set; }
        public string titleReleaseText { get; set; }
        public string titleTypeText { get; set; }
        public Titleposterimagemodel titlePosterImageModel { get; set; }
        public string[] topCredits { get; set; }
        public string imageType { get; set; }
    }

    public class Titleposterimagemodel
    {
        public string url { get; set; }
        public int maxHeight { get; set; }
        public int maxWidth { get; set; }
        public string caption { get; set; }
    }

    public class Nameresults
    {
        public Result1[] results { get; set; }
        public string nextCursor { get; set; }
        public bool hasExactMatches { get; set; }
    }

    public class Result1
    {
        public string id { get; set; }
        public string displayNameText { get; set; }
        public string knownForJobCategory { get; set; }
        public string knownForTitleText { get; set; }
        public string knownForTitleYear { get; set; }
        public Avatarimagemodel avatarImageModel { get; set; }
    }

    public class Avatarimagemodel
    {
        public string url { get; set; }
        public int maxHeight { get; set; }
        public int maxWidth { get; set; }
        public string caption { get; set; }
    }

    public class Companyresults
    {
        public object[] results { get; set; }
    }
}

