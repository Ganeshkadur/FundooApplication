using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusenessLayer.Interfaces
{
    public interface ILabel_Buseness
    {
        bool AddLable(long userId, long noteId, string labelName);
        Label AddLableUsingUserId(long userId, string labelName);
        Label UpdateLable(long userId, long labelId, string labelname);
        Label DeleteLabel(long userId, long labelId);
        IEnumerable<Label> GetAllLabels(long userId);
        IEnumerable<Label> GetAllLabelsByNoteId(long userId, long noteId);
    }
}
