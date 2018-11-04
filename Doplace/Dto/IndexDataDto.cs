using System.Text.RegularExpressions;

namespace Doplace.Dto
{
    public class IndexDataDto
    {
        public long Id { get; set; }

        public string SearchText { get; set; }

        public IndexEntityType IndexEntityType { get; set; }

        public string Space { get; set; }

        public string Url {
            get
            {
                switch (IndexEntityType)
                {
                    case IndexEntityType.Comment:
                        return $"/comments/{Id}";
                    case IndexEntityType.Post:
                        return $"/posts/{Id}";
                    default:
                        return "/";
                }
            }
        }

        public string Icon
        {
            get
            {
                switch (IndexEntityType)
                {
                    case IndexEntityType.Post:
                        return "forum";
                    case IndexEntityType.Comment:
                        return "comment";
                    default:
                        return "";
                }
            }
        }

        public string Color
        {
            get
            {
                switch (IndexEntityType)
                {
                    case IndexEntityType.Post:
                        return "green";
                    case IndexEntityType.Comment:
                        return "lime";
                    default:
                        return "";
                }
            }
        }

        public string SearchTextHtml(string query)
        {
            var regex = "(" + string.Join("|", query.Split(' ')) + ")";
            MatchEvaluator myEvaluator = new MatchEvaluator(m => string.Format("<span class='highlight'>{0}</span>", m.Value));
            return Regex.Replace(SearchText, regex, myEvaluator, RegexOptions.IgnoreCase);
        }
    }

    public enum IndexEntityType
    {
        None = 0,
        Post = 1,
        Comment = 2
    }
}
