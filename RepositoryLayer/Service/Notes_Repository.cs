using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.ResuestModels;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class Notes_Repository : INotes_Repository
    {
        private readonly Context _context;
        private readonly IConfiguration configuration;


        public Notes_Repository(Context context)
        {
            _context = context;
        }

        public string CreateNotes(NotesModel request, long userId)
        {


            if (userId != 0)
            {
                Notes note = new Notes();
                note.Title = request.Title;
                note.Description = request.Description;
                note.IsArchive = request.IsArchive;
                note.IsTrash = request.IsTrash;
                note.IsPinned = request.IsPinned;
                note.Reminder = request.Reminder;
                note.Color = request.Color;
                note.CreatedAt = request.CreatedAt;
                note.ModifiedAt = request.ModifiedAt;
                note.UserId = userId;
                // note.UserId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);//used for verifing token
                //note.User = model.User;
                _context.notes.Add(note);
                _context.SaveChanges();
                return "Note created Sucessfully...";

            }
            else
            {
                return null;
            }

        }

        public bool DeleteNote(long userId, long noteid)
        {
            var note = _context.notes.FirstOrDefault(x => x.NoteID == noteid);
            if (note != null)
            {
                _context.notes.Remove(note);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool UpdateNote(long userid, long noteid, NotesModel model)
        {
            var note = _context.notes.Where(y => y.UserId == userid && y.NoteID == noteid).FirstOrDefault();
            if (note != null)
            {

                note.Title = model.Title;
                note.Description = model.Description;
                note.Reminder = model.Reminder;
                note.Color = model.Color;
                note.CreatedAt = note.CreatedAt;
                note.ModifiedAt = model.CreatedAt;
                note.Reminder = model.Reminder;
                note.IsPinned = model.IsPinned;
                
                note.IsArchive = model.IsArchive;
                note.IsTrash = model.IsTrash;

                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Notes>> GetAllNotes(long userId)
        {
            List<Notes> notes = _context.notes.Where(x => x.UserId == userId).ToList();//findll 

            if (notes.Count == 0 || notes == null)
            {
                return null;
            }
            else
            {
                return notes;
            }
        }

        public IEnumerable<Notes> GetNotesByDate(long userid, DateTime date)
        {
            IEnumerable<Notes> user = _context.notes.Where(x => x.UserId == userid && x.Reminder.Date == date.Date).ToList();

            if (user.Any())
            {
                return user.ToList();
            }
            return null;
        }


        public Notes GetNoteById(long noteId, long userId)
        {
            var result = _context.notes.FirstOrDefault(x => x.NoteID == noteId && x.UserId == userId);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public Notes ToggleTrash(long userId, long noteId)
        {
            // var userId=HttpContext.Session.GetInt32("UserId");
            var trashnote = _context.notes.FirstOrDefault(x => x.UserId == userId && x.NoteID == noteId);
            if (trashnote == null)
            {
                return null;
            }
            else
            {
                if (trashnote.IsTrash == false)
                {
                    trashnote.IsTrash = true;
                    if (trashnote.IsPinned == true)
                    {
                        trashnote.IsPinned = false;
                    }

                    if (trashnote.IsArchive == true)
                    {
                        trashnote.IsArchive = false;
                    }


                    _context.Entry(trashnote).State = EntityState.Modified;
                    _context.SaveChanges();

                }
                else
                {
                    trashnote.IsTrash = false;

                    if (trashnote.IsPinned == true)
                    {
                        trashnote.IsPinned = false;
                    }
                    if (trashnote.IsArchive == true)
                    {
                        trashnote.IsArchive = false;
                    }


                    _context.Entry(trashnote).State = EntityState.Modified;
                    _context.SaveChanges();

                }

                return trashnote;

            }



        }

        public Notes AddColor(long userId, long noteId, string color)
        {
            var note = _context.notes.Where(x => x.UserId == userId && x.NoteID == noteId).FirstOrDefault();
            if (note == null)
            {
                return null;
            }
            else
            {
                note.Color = color;
                _context.Entry(note).State = EntityState.Modified;
                _context.SaveChanges();
                return note;
            }

        }

        public Notes AddReminder(long userId, long noteId, DateTime reminder)
        {
            var note = _context.notes.Where(x => x.UserId == userId && x.NoteID == noteId).FirstOrDefault();
            if (note == null)
            {
                return null;
            }
            else
            {

                if (reminder > DateTime.Now)
                {
                    note.Reminder = reminder;
                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }

        }

        public Notes TogglePin(long userId, long noteId)
        {
            var note = _context.notes.Where(x => x.UserId == userId && x.NoteID == noteId).FirstOrDefault();
            if (note == null)
            {
                return null;
            }
            else
            {
                if (note.IsPinned == true)
                {
                    note.IsPinned = false;
                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();

                }
                else
                {
                    note.IsPinned = true;
                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return note;
            }

        }

        public Notes ToggleArchive(long userId, long noteId)
        {
            var note = _context.notes.Where(x => x.UserId == userId && x.NoteID == noteId).FirstOrDefault();
            if (note == null)
            {
                return null;
            }
            else
            {
                if (note.IsArchive == true)
                {
                    note.IsArchive = false;

                    if (note.IsPinned == true)
                    {
                        note.IsPinned = false;
                    }
                    if (note.IsTrash = true)
                    {
                        note.IsTrash = false;
                    }
                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();
                    return note;
                }
                else
                {
                    note.IsArchive = true;
                    if (note.IsPinned == true)
                    {
                        note.IsPinned = false;
                    }
                    if (note.IsTrash = true)
                    {
                        note.IsTrash = false;
                    }

                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();
                    return note;
                }
            }

        }



        public bool AddReview(ReviewModel request)
        {
            if (request != null)
            {
                Review review =new Review();
                review.rating = request.rating;
                review.Comment= request.Comment;

                _context.reviews.Add(review);
                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }

        }

        public IEnumerable<Review> FetchAllReview()
        {
            var result=_context.reviews.ToList();
            if(result.Count != 0)
            {
                return result;
            }
            else
            {
                return null;
            }
        }


        public Notes AddImage(long userId, long noteId, IFormFile Image)
        {
            try
            {


                var note = _context.notes.Where(x => x.UserId == userId && x.NoteID == noteId).FirstOrDefault();

                if (note == null)
                {
                    return null;
                }
                else
                {


                    Account account = new Account("dgxwykdpf", "765185325949132", "hQsLfxKsm_lw-DiY9hDtQEcMufs");

                    /* Account account = new Account(
                     configuration["CloudinarySettings:dgxwykdpf"],
                     configuration["CloudinarySettings:765185325949132"],
                     configuration["CloudinarySettings:hQsLfxKsm_lw-DiY9hDtQEcMufs"]
                    );*/
                    Cloudinary cloudinary = new Cloudinary(account);
                    var uploadParameters = new ImageUploadParams()
                    {
                        File = new FileDescription(Image.FileName, Image.OpenReadStream())
                    };
                    var uploadResult = cloudinary.Upload(uploadParameters);
                    string ImagePath = uploadResult.Url.ToString();

                    note.Image = ImagePath;
                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();

                    return note;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
    }


    
}
