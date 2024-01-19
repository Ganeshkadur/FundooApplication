using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class Collaborator_Repository:ICollaborator_Repository
    {
       private readonly Context _context;

        public Collaborator_Repository(Context context)
        {
            _context = context;
        }

        public bool AddCollaborator(long userId, long noteId, string collaboratorEmail)
        {
            var notes = _context.notes.Where(x => x.UserId == userId && x.NoteID == noteId).FirstOrDefault();
            if(notes != null)
            {

                Collaborator collaborator = new Collaborator();
                collaborator.UserId = userId;
                collaborator.NoteId = noteId;
                collaborator.CollaboratorsEmail = collaboratorEmail;
                _context.Add(collaborator);
                _context.SaveChanges();

                return true; 

            }
            else
            {
                return false;
            }

        }

       

        public Collaborator DeleteCollaborator(long userId, long noteId, long collaboratorId)
        {
            var collab = _context.collaborators.Where(x => x.UserId == userId && x.NoteId == noteId&&x.CollaboratorsId== collaboratorId).FirstOrDefault();
            if(collab != null)
            {
                _context.collaborators.Remove(collab);
                _context.SaveChanges();
                return collab;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Collaborator> GetCollaboratorsByNoteId(long userId, long noteId)
        {
            var collab = _context.collaborators.Where(x => x.UserId == userId && x.NoteId == noteId).ToList();
            if (collab != null)
            {
                return collab;
            }
            return null;
        }

        public IEnumerable<Collaborator> GetCollaborators(long userId)
        {
            var collab = _context.collaborators.Where(x => x.UserId == userId).ToList();
            if (collab != null)
            {
                return collab;
            }
            return null;
        }

    }
}
