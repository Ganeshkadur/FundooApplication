using BusenessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusenessLayer.Service
{
    public class Collaborator_Buseness:ICollaborator_Buseness
    {
        private readonly ICollaborator_Repository _repo;

        public Collaborator_Buseness(ICollaborator_Repository repo)
        {
            _repo = repo;
        }

        public bool AddCollaborator(long userId, long noteId, string collaboratorEmail)
        {
            return _repo.AddCollaborator(userId, noteId, collaboratorEmail);
        }

        public Collaborator DeleteCollaborator(long userId, long noteId, long collaboratorId)
        {
            return _repo.DeleteCollaborator(userId,noteId, collaboratorId);
        }

        public IEnumerable<Collaborator> GetCollaboratorsByNoteId(long userId, long noteId)
        {
            return _repo.GetCollaboratorsByNoteId(userId, noteId);
        }

        public IEnumerable<Collaborator> GetCollaborators(long userId)
        {
            return _repo.GetCollaborators(userId);
        }

    }
}
