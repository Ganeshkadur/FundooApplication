using BusenessLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusenessLayer.Service
{
    public class Label_Buseness:ILabel_Buseness
    {
        private readonly ILabel_Repository _repo;
        public Label_Buseness(ILabel_Repository repo)
        {
            _repo = repo;
        }

        public bool AddLable(long userId, long noteId, string labelName)
        {
            return _repo.AddLable(userId, noteId, labelName);
        }

        public Label UpdateLable(long userId, long labelId, string labelname)
        {
            return _repo.UpdateLable(userId,labelId,labelname);
        }

        public Label DeleteLabel(long userId, long labelId)
        {
            return _repo.DeleteLabel(userId, labelId);
        }

        public IEnumerable<Label> GetAllLabels(long userId)
        {
            return _repo.GetAllLabels(userId);
        }

        public IEnumerable<Label> GetAllLabelsByNoteId(long userId, long noteId)
        {
            return _repo.GetAllLabelsByNoteId(userId, noteId);
        }

        public Label AddLableUsingUserId(long userId, string labelName)
        {
            return _repo.AddLableUsingUserId(userId, labelName);
        }
    }
}
