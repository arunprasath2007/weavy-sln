using Weavy.Core.Models;

namespace Doplace.Dto
{
    public class IndexDataDto
    {
        public long Id { get; set; }

        public string SearchText { get; set; }

        public IndexEntityType IndexEntityType { get; set; }

        public string Space { get; set; }
    }

    public enum IndexEntityType
    {
        None = 0,
        Post = 1,
        Comment = 2
    }
}
