using Doplace.Dto;
using Doplace.Repository.Core;
using System;
using System.Collections.Generic;

namespace Doplace.Repository
{
    public class LuceneRepository
    {
        private readonly IRepository _repository;

        public LuceneRepository()
        {
            _repository = new DapperRepository();
        }

        public IEnumerable<IndexDataDto> GetModifiedIndexData(DateTime startTime, DateTime endTime)
        {
            //DECLARE @startTime DATETIME2 = '2018-11-1 00:00:00', @endTime DATETIME2  = '2018-11-2 00:00:00'

            return _repository.Query<IndexDataDto>(@"SELECT p.Id, p.[Text] AS 'SearchText', 1 AS 'IndexEntityType', s.[Id] AS 'Space'
	                FROM dbo.Posts p
		                INNER JOIN dbo.Spaces s ON p.SpaceId = s.Id
	                WHERE p.ModifiedAt >= @startTime AND p.ModifiedAt < @endTime
                UNION ALL
                SELECT c.Id, c.[Text] AS 'SearchText', 2 AS 'IndexEntityType', s.[Id] AS 'Space'
	                FROM dbo.Comments c
		                INNER JOIN dbo.Spaces s ON c.SpaceId = s.Id
	                WHERE c.ModifiedAt >= @startTime AND c.ModifiedAt < @endTime", 
                new {
                    startTime,
                    endTime
                });
        }

        public IEnumerable<IndexDataDto> GetDeletedIndexData(DateTime startTime, DateTime endTime)
        {
            //DECLARE @startTime DATETIME2 = '2018-11-1 00:00:00', @endTime DATETIME2  = '2018-11-2 00:00:00'

            return _repository.Query<IndexDataDto>(@"SELECT p.Id, p.[Text] AS 'SearchText', 1 AS 'IndexEntityType', s.[Id] AS 'Space'
	                FROM dbo.Posts p
		                INNER JOIN dbo.Spaces s ON p.SpaceId = s.Id
	                WHERE p.TrashedAt >= @startTime AND p.TrashedAt < @endTime
                UNION ALL
                SELECT c.Id, c.[Text] AS 'SearchText', 2 AS 'IndexEntityType', s.[Id] AS 'Space'
	                FROM dbo.Comments c
		                INNER JOIN dbo.Spaces s ON c.SpaceId = s.Id
	                WHERE c.TrashedAt >= @startTime AND c.TrashedAt < @endTime",
                new
                {
                    startTime,
                    endTime
                });
        }
    }
}
