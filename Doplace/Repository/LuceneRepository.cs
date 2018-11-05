using Doplace.Dto;
using Doplace.Repository.Core;
using System;
using System.Collections.Generic;
using Weavy.Core.Models;

namespace Doplace.Repository
{
    public class LuceneRepository
    {
        private readonly IRepository _repository;

        public LuceneRepository()
        {
            _repository = new DapperRepository();
        }

        public IEnumerable<IndexDocument> GetModifiedIndexData(DateTime startTime, DateTime endTime)
        {
            //DECLARE @startTime DATETIME2 = '2018-11-1 00:00:00', @endTime DATETIME2  = '2018-11-2 00:00:00'

            return _repository.Query<IndexDocument>(@"SELECT 1 as Id,
	1 as [SpaceId],
	7 as [Type],
	'forum' as [Icon],
	'green' as [Color],
	'document' as [kind],
	'name' as [name],
	'title' as [title],
	'text' as [text],
	'description' as 'description',
	'2018-01-01' as 'createdAt',
	1 as 'createdById',
	'2018-01-01' as 'modifiedAt',
	1 as 'ModifiedById',
	2010101 as 'ts',
	't1' as 'tags'", 
                null);
        }

        public IEnumerable<IndexDocument> GetDeletedIndexData(DateTime startTime, DateTime endTime)
        {
            //DECLARE @startTime DATETIME2 = '2018-11-1 00:00:00', @endTime DATETIME2  = '2018-11-2 00:00:00'

            return _repository.Query<IndexDocument>(@"SELECT 1 as Id,
	1 as [SpaceId],
	7 as [Type],
	'forum' as [Icon],
	'green' as [Color],
	'document' as [kind],
	'name' as [name],
	'title' as [title],
	'text' as [text],
	'description' as 'description',
	'2018-01-01' as 'createdAt',
	1 as 'createdById',
	'2018-01-01' as 'modifiedAt',
	1 as 'ModifiedById',
	2010101 as 'ts',
	't1' as 'tags'",
                null);
        }
    }
}
