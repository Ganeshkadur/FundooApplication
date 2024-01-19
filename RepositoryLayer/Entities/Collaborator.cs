using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entities
{
    public class Collaborator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CollaboratorsId { get; set; }
        public string CollaboratorsEmail { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        [JsonIgnore]
        public virtual UserRegistration User { get; set; }


        [ForeignKey("Note")]// name downbellow fields
        public long NoteId { get; set; }
       
        [JsonIgnore]
        public virtual Notes Note { get; set; }

    }
}
