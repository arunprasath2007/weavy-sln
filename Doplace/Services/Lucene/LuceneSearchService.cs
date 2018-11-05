﻿using Doplace.Constants;
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
using Weavy.Core.Models;
using Version = Lucene.Net.Util.Version;
using Query = Lucene.Net.Search.Query;

namespace Doplace.Services.Lucene
{
    public class LuceneSearchService : LuceneSearch
    {
        public LuceneSearchService(string rootPath) : base(rootPath)
        {
        }

        public IEnumerable<IndexDocument> GetAll()
        {
            // validate search index
            if (!System.IO.Directory.EnumerateFiles(_luceneFolderPhysicalPath).Any()) return new List<IndexDocument>();

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

        public IEnumerable<IndexDocument> Search(SearchIndexDto dto)
        {
            if (string.IsNullOrEmpty(dto.SearchQuery)) return new List<IndexDocument>();

            var terms = dto.SearchQuery.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => $"*{x.Trim()}*");
            dto.SearchQuery = string.Join(" ", terms);

            return _search(dto);
        }

        #region Private Methods

        // main search method
        private IEnumerable<IndexDocument> _search(SearchIndexDto dto)
        {
            // validation
            if (string.IsNullOrEmpty(dto.SearchQuery.Replace("*", "").Replace("?", ""))) return new List<IndexDocument>();

            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                var hits_limit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                var parser = new QueryParser
                        (Version.LUCENE_30, SearchConstants.FIELD_TEXT, analyzer);
                parser.AllowLeadingWildcard = true;
                var query = parseQuery(dto.SearchQuery, parser);
                var hits = searcher.Search(query, GetFilter(dto.FilterField, dto.FilterValue), hits_limit, Sort.RELEVANCE).ScoreDocs;
                var results = _mapLuceneToDataList(hits, searcher);
                analyzer.Close();
                searcher.Dispose();

                return results;
            }
        }

        private static Filter GetFilter(string field, string value)
        {
            if (string.IsNullOrEmpty(field)) return null;
            return new QueryWrapperFilter(new TermQuery(new Term(field, value.ToLowerInvariant())));
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
        private static IEnumerable<IndexDocument> _mapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(_mapLuceneDocumentToData).ToList();
        }
        private static IEnumerable<IndexDocument> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }
        private static IndexDocument _mapLuceneDocumentToData(Document doc)
        {
            return new IndexDocument(doc);
        }

        #endregion
    }
}
