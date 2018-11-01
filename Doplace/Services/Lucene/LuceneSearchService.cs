using Doplace.Dto;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = Lucene.Net.Util.Version;

namespace Doplace.Services.Lucene
{
    public class LuceneSearchService : LuceneSearch
    {
        public LuceneSearchService(string rootPath) : base(rootPath)
        {
        }

        public IEnumerable<IndexDataDto> GetAll()
        {
            // validate search index
            if (!System.IO.Directory.EnumerateFiles(_luceneFolderPhysicalPath).Any()) return new List<IndexDataDto>();

            // set up lucene searcher
            var searcher = new IndexSearcher(_directory, false);
            var reader = IndexReader.Open(_directory, false);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            while (term.Next()) docs.Add(searcher.Doc(term.Doc));
            reader.Dispose();
            searcher.Dispose();
            return _mapLuceneToDataList(docs);
        }

        public IEnumerable<IndexDataDto> Search(string query)
        {
            if (string.IsNullOrEmpty(query)) return new List<IndexDataDto>();

            var terms = query.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => $"*{x.Trim()}*");
            query = string.Join(" ", terms);

            return _search(query, string.Empty);
        }

        #region Private Methods

        // main search method
        private IEnumerable<IndexDataDto> _search(string searchQuery, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<IndexDataDto>();

            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                var hits_limit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                // search by single field
                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    parser.AllowLeadingWildcard = true;
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = _mapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                // search by multiple fields (ordered by RELEVANCE)
                else
                {
                    var parser = new QueryParser
                        (Version.LUCENE_30, "SearchText", analyzer);
                    parser.AllowLeadingWildcard = true;
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    var results = _mapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }
        private static Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        // map Lucene search index to data
        private static IEnumerable<IndexDataDto> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();
        }
        private static IEnumerable<IndexDataDto> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }
        private static IndexDataDto _mapLuceneDocumentToData(Document doc)
        {
            var val = new IndexDataDto
            {
                Id = Convert.ToInt32(doc.Get("Id")),
                SearchText = doc.Get("SearchText"),
                Space = doc.Get("Space")
            };
            Enum.TryParse(doc.Get("IndexEntityType"), out IndexEntityType entityType);
            val.IndexEntityType = entityType;
            return val;
        }

        #endregion
    }
}
