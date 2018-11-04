using Doplace.Constants;
using Doplace.Dto;
using Doplace.Helpers;
using Doplace.Repository;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using Version = Lucene.Net.Util.Version;

namespace Doplace.Services.Lucene
{
    public class LuceneSearchSetupService : LuceneSearch
    {
        private readonly LuceneRepository _luceneRepository;

        public LuceneSearchSetupService(string rootPath) : base(rootPath)
        {
            _luceneRepository = new LuceneRepository();
        }

        public void AddToIndex(IndexDataDto dto)
        {
            AddToIndex(new List<IndexDataDto> { dto });
        }

        public void AddToIndex(IEnumerable<IndexDataDto> dto)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entries if any)
                foreach (var data in dto)
                {
                    _addToLuceneIndex(data, writer);
                }

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public void CreateIndex(DateTime startTime, DateTime endTime)
        {
            FileHelper.CreateFolder(_rootPath, _luceneFolderName);
            var dataToBeRemovedDto = _luceneRepository.GetDeletedIndexData(startTime, endTime);
            RemoveFromIndex(dataToBeRemovedDto);
            var dataToBeUpdatedDto = _luceneRepository.GetModifiedIndexData(startTime, endTime);
            UpdateIndex(dataToBeUpdatedDto);
        }

        public void RemoveFromIndex(IndexDataDto dto)
        {
            RemoveFromIndex(new List<IndexDataDto> { dto });
        }

        public void RemoveFromIndex(IEnumerable<IndexDataDto> dto)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                foreach (var item in dto)
                {
                    var idQuery = new TermQuery(new Term(SearchConstants.FIELD_ID, item.Id.ToString()));
                    var entityTypeQuery = new TermQuery(new Term(SearchConstants.FIELD_INDEX_ENTITY_TYPE, item.IndexEntityType.ToString()));
                    writer.DeleteDocuments(idQuery, entityTypeQuery);
                }

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public void UpdateIndex(IndexDataDto dto)
        {
            UpdateIndex(new List<IndexDataDto> { dto });
        }

        public void UpdateIndex(IEnumerable<IndexDataDto> dto)
        {
            RemoveFromIndex(dto);
            AddToIndex(dto);
        }

        #region Private methods

        private static void _addToLuceneIndex(IndexDataDto dto, IndexWriter writer)
        {
            // add new index entry
            var doc = new Document();

            // add lucene fields mapped to db fields
            doc.Add(new Field(SearchConstants.FIELD_ID, dto.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_SEARCH_TEXT, dto.SearchText, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_INDEX_ENTITY_TYPE, dto.IndexEntityType.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_SPACE, dto.Space, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_MODIFIED_AT, dto.ModifiedAt.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_MODIFIED_BY, dto.ModifiedBy, Field.Store.YES, Field.Index.NOT_ANALYZED));

            // add entry to index
            writer.AddDocument(doc);
        }

        #endregion
    }
}
