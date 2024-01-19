using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ModelLayer.ResuestModels
{
    public class NotesModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public DateTime Reminder { get; set; }

        public bool IsArchive { get; set; }

        public bool IsPinned { get; set; }

        public bool IsTrash { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
        [JsonIgnore]
        public long UserId { get; set; }
    }
}
