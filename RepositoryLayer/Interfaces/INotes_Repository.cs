using Microsoft.AspNetCore.Http;
using ModelLayer.ResuestModels;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface INotes_Repository
    {
        string CreateNotes(NotesModel request, long userId);
        bool DeleteNote(long userId,long noteid);
        bool UpdateNote(long userid, long noteid, NotesModel model);
        Task<List<Notes>> GetAllNotes(long userId);
        IEnumerable<Notes> GetNotesByDate(long userid, DateTime date);
        Notes GetNoteById(long noteId, long userId);

        Notes ToggleTrash(long userId, long noteId);
        Notes AddColor(long userId, long noteId, string color);

        Notes AddReminder(long userId, long noteId, DateTime reminder);

        Notes TogglePin(long userId, long noteId);
        Notes ToggleArchive(long userId, long noteId);

        Notes AddImage(long userId, long noteId, IFormFile Image);

        bool AddReview(ReviewModel request);

        IEnumerable<Review> FetchAllReview();
    }
}
