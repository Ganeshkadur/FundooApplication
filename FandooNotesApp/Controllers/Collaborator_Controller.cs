using BusenessLayer.Interfaces;
using BusenessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.ResponseModel;
using RepositoryLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FandooNotesApp.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class Collaborator_Controller : ControllerBase
    {
        private readonly ICollaborator_Buseness _buseness;

        public Collaborator_Controller(Collaborator_Buseness buseness)
        {
            _buseness = buseness;
        }


        [Authorize]
        [HttpPost]

        public IActionResult AddCollaborator(long noteId, string collaboratorEmail)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);

            var result=_buseness.AddCollaborator(userid,noteId,collaboratorEmail);
            if (result)
            {
                return Ok(new ResponseModel<string> {Success=true,Message="Collaborator added" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Ccollaborator not added" });
            }
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteCollaborator(long noteId, long collaboratorId)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result=_buseness.DeleteCollaborator(userid,noteId, collaboratorId);
            if (result!=null)
            {
                return Ok(new ResponseModel<Collaborator>() { Success=true,Message="Collaborator deleted ",Data=result});
            }
            else
            {
                return NotFound(new ResponseModel<Collaborator>() { Success=false,Message="Collaborator not deleted",Data=null});
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetCollaboratorsByNoteId( long noteId)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _buseness.GetCollaboratorsByNoteId(userid,noteId);
            if (result != null)
            {
                return Ok(new ResponseModel<IEnumerable<Collaborator>>() { Success = true, Message = "Collaborator found... ", Data = result });
            }
            else
            {
                return NotFound(new ResponseModel<IEnumerable<Collaborator>>() { Success = false, Message = "Collaborator not Not found", Data = null });
            }

        }

        [Authorize]
        [HttpGet]
        public IActionResult GetCollaborators()
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _buseness.GetCollaborators(userid);
            if (result != null)
            {
                return Ok(new ResponseModel<IEnumerable<Collaborator>>() { Success = true, Message = "Collaborator found... ", Data = result });
            }
            else
            {
                return NotFound(new ResponseModel<IEnumerable<Collaborator>>() { Success = false, Message = "Collaborator not Not found", Data = null });
            }

        }


    }
}
