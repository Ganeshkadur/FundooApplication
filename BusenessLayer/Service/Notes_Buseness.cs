using BusenessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using ModelLayer.ResuestModels;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusenessLayer.Service
{
    public class Notes_Buseness:INotes_Buseness
    {
        private readonly INotes_Repository _repo;

        public Notes_Buseness(INotes_Repository repo)
        {
            _repo = repo;
        }

        public string CreateNotes(NotesModel request, long userid)
        {
            return _repo.CreateNotes(request, userid);
        }

        public bool DeleteNote(long userId, long noteid)
        {
            return _repo.DeleteNote(userId,noteid);
        }

        public bool UpdateNote(long userid, long noteid, NotesModel model)
        {
            return _repo.UpdateNote(userid, noteid, model);   
        }

        public async Task<List<Notes>> GetAllNotes(long userId)
        {
            return await _repo.GetAllNotes(userId);
        }

        public IEnumerable<Notes> GetNotesByDate(long userid, DateTime date)
        {
            return _repo.GetNotesByDate(userid, date);
        }
        public Notes GetNoteById(long noteId, long userId)
        {
            return _repo.GetNoteById(noteId, userId);
        }

        public Notes ToggleTrash(long userId, long noteId)
        {
            return _repo.ToggleTrash(userId, noteId);
        }

        public  Notes AddColor(long userId, long noteId, string color)
        {
            return _repo.AddColor(userId, noteId, color);
        }

        public Notes AddReminder(long userId, long noteId, DateTime reminder)
        {
            return _repo.AddReminder(userId, noteId, reminder);
        }
        public Notes TogglePin(long userId, long noteId)
        {
            return _repo.TogglePin(userId, noteId);
        }

        public Notes ToggleArchive(long userId, long noteId)
        {
            return _repo.ToggleArchive(userId, noteId);
        }

        public Notes AddImage(long userId, long noteId, IFormFile Image)
        {
            return _repo.AddImage(userId, noteId, Image);
        }

        public bool AddReview(ReviewModel request)
        {
            return _repo.AddReview(request);
        }

        public IEnumerable<Review> FetchAllReview()
        {
            return _repo.FetchAllReview();  
        }

    }
}
