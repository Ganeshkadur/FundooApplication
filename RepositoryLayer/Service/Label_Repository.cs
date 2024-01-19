using CloudinaryDotNet.Actions;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Label = RepositoryLayer.Entities.Label;

namespace RepositoryLayer.Service
{
    public class Label_Repository:ILabel_Repository
    {
        private readonly Context _context;

        public Label_Repository(Context context)
        {
            _context = context;
        }

        public bool AddLable(long userId, long noteId, string labelName)
        {
            var note=_context.notes.Where(x => x.UserId == userId && x.NoteID == noteId).FirstOrDefault();
           if(note != null)
            {
                Label label = new Label(); 
                label.NoteId = noteId;
                label.LabelName = labelName;
                label.UserId = userId;
                _context.Add(label);
                _context.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }

        public Label UpdateLable(long userId,long labelId, string labelname)
        {
           var label= _context.labels.Where(x=>x.UserId==userId&&x.LabelId==labelId).FirstOrDefault();
            if(label != null)
            {
                label.LabelName=labelname;
                _context.Entry(label).State=EntityState.Modified;
                _context.SaveChanges();
                return label;
            }
            return null;

        }

        public Label DeleteLabel(long userId, long labelId)
        {
            var label = _context.labels.Where(x => x.UserId == userId && x.LabelId == labelId).FirstOrDefault();
            if (label != null)
            {
                _context.Remove(label);
                _context.SaveChanges();
                return label;
            }
            return null;

        }


        public IEnumerable<Label> GetAllLabels(long userId)
        {
            var label = _context.labels.Where(x => x.UserId == userId).ToList();
            if(label != null)
            {
                return label;
            }
            else
            {
                return null;
            }

        }

        public IEnumerable<Label> GetAllLabelsByNoteId(long userId, long noteId)
        {
            var label = _context.labels.Where(x => x.UserId == userId && x.NoteId == noteId).ToList();
            if (label != null)
            {
                return label;
            }
            else
            {
                return null;
            }

        }


        public Label AddLableUsingUserId(long userId, string labelName)
        {
            var user = _context.notes.Where(x => x.UserId == userId).FirstOrDefault();
            if (user != null)
            {
                Label lable = new Label();
                lable.LabelName = labelName;
                _context.Add(lable);
                _context.SaveChanges();
                return lable;
            }
            return null;

        }
    }
}
