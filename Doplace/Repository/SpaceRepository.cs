using Doplace.Repository.Core;
using System.Linq;
using Weavy.Core.Models;

namespace Doplace.Repository
{
    public class SpaceRepository
    {
        private readonly IRepository _repository;

        public SpaceRepository()
        {
            _repository = new DapperRepository();
        }

        public Space GetSpaceByAppId(int appId)
        {
            return _repository.Query<Space, Blob, Space>(@"SELECT Spaces.Id, Spaces.Teamname, Spaces.Name, Spaces.Description, Spaces.AvatarId, Spaces.Color, Spaces.Privacy, Spaces.Approval, Spaces.Tags, Spaces.IsHQ, Spaces.CreatedAt, Spaces.CreatedBy AS CreatedById, Spaces.ModifiedAt, Spaces.ModifiedBy AS ModifiedById, Spaces.ArchivedAt, Spaces.ArchivedBy AS ArchivedById, Spaces.TrashedAt, Spaces.TrashedBy AS TrashedById, Spaces.StarredBy AS StarredByIds, Spaces.FollowedBy AS FollowedByIds, Spaces.Members AS MemberIds, Spaces.Properties, CAST(Spaces.Timestamp AS bigint) Timestamp, Blobs.Id, Blobs.Name, Blobs.Width, Blobs.Height, Blobs.Size, Blobs.MediaType, Blobs.Properties, Blobs.CreatedAt, Blobs.CreatedBy AS CreatedById, Blobs.UploadedAt, Blobs.ETag, Blobs.ProviderGuid FROM dbo.Spaces Spaces
                    LEFT JOIN dbo.Blobs ON Blobs.Id = Spaces.AvatarId
                    INNER JOIN dbo.Apps a ON Spaces.Id = a.SpaceId
                WHERE a.Id = @id", 
                new { id = appId },
                (space, avatar) => {
                    space.Avatar = avatar;
                    return space;
                }).FirstOrDefault();
        }
    }
}
