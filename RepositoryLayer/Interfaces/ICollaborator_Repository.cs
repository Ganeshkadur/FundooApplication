using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ICollaborator_Repository
    {
        bool AddCollaborator(long userId, long noteId, string collaboratorEmail);

        Collaborator DeleteCollaborator(long userId, long noteId, long collaboratorId);
        IEnumerable<Collaborator> GetCollaboratorsByNoteId(long userId, long noteId);

        IEnumerable<Collaborator> GetCollaborators(long userId);
    }
}
