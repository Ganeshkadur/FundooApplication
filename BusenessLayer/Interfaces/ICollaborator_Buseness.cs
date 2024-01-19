using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusenessLayer.Interfaces
{
    public interface ICollaborator_Buseness
    {
        bool AddCollaborator(long userId, long noteId, string collaboratorEmail);

        Collaborator DeleteCollaborator(long userId, long noteId, long collaboratorId);

        IEnumerable<Collaborator> GetCollaboratorsByNoteId(long userId, long noteId);

        IEnumerable<Collaborator> GetCollaborators(long userId);
    }
}
