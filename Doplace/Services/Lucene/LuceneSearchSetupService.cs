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
using System.Linq;
using Weavy.Core.Models;
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

        public void AddToIndex(IndexDocument dto)
        {
            AddToIndex(new List<IndexDocument> { dto });
        }

        public void AddToIndex(IEnumerable<IndexDocument> dto)
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

        public void RemoveFromIndex(IndexDocument dto)
        {
            RemoveFromIndex(new List<IndexDocument> { dto });
        }

        public void RemoveFromIndex(IEnumerable<IndexDocument> dto)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                foreach (var item in dto)
                {
                    var idQuery = new TermQuery(new Term(SearchConstants.FIELD_ID, item.Id.ToString()));
                    var entityTypeQuery = new TermQuery(new Term(SearchConstants.FIELD_TYPE, item.Type.ToString()));
                    writer.DeleteDocuments(idQuery, entityTypeQuery);
                }

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public void UpdateIndex(IndexDocument dto)
        {
            UpdateIndex(new List<IndexDocument> { dto });
        }

        public void UpdateIndex(IEnumerable<IndexDocument> dto)
        {
            RemoveFromIndex(dto);
            AddToIndex(dto);
        }

        #region Private methods

        private static void _addToLuceneIndex(IndexDocument dto, IndexWriter writer)
        {
            // add new index entry
            var doc = new Document();

            // add lucene fields mapped to db fields
            doc.Add(new Field(SearchConstants.FIELD_ID, dto.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_SPACE, dto.SpaceId.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_TYPE, dto.Type.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_ICON, dto.Icon, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_COLOR, dto.Color, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_KIND, dto.Kind, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_NAME, dto.Name, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_TITLE, dto.Title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_TEXT, "text", Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_DESCRIPTION, dto.Description, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_MODIFIED_AT, DateTools.DateToString(dto.ModifiedAt, DateTools.Resolution.SECOND), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_MODIFIED_BY, dto.ModifiedById.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_CREATED_AT, DateTools.DateToString(dto.CreatedAt, DateTools.Resolution.SECOND), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_CREATED_BY, dto.CreatedById.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_TS, dto.Timestamp.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchConstants.FIELD_TAGS, string.Join(",", dto.Tags.ToArray()), Field.Store.YES, Field.Index.NOT_ANALYZED));

            // add entry to index
            writer.AddDocument(doc);
        }

        #endregion
    }
}
