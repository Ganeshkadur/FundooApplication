using BusenessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.ResponseModel;
using ModelLayer.ResuestModels;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FandooNotesApp.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class Label_Controller : ControllerBase
    {
        private readonly ILabel_Buseness _buseness;

        public Label_Controller(ILabel_Buseness buseness)
        {
            _buseness = buseness;
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddLable(long noteId, string labelName)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);

            var result= _buseness.AddLable(userid, noteId,labelName);
            if (result)
            {
                return Ok(new ResponseModel<string>() {Success=true,Message="Label Added" });


            }
            else
            {
                return BadRequest(new ResponseModel<string>() {Success=false,Message="Label NotAdded" });
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateLable(long labelId, string labelname)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);

            Label result = _buseness.UpdateLable(userid,labelId,labelname);
            if (result!=null)
            {
                return Ok(new ResponseModel<Label>() { Success = true, Message = "Label Updated",Data=result });


            }
            else
            {
                return BadRequest(new ResponseModel<Label>() { Success = false, Message = "Label Not updated",Data=result});
            }

        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteLabel(long labelId)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            Label result = _buseness.DeleteLabel(userid,labelId);
            if (result != null)
            {
                return Ok(new ResponseModel<Label>() { Success = true, Message = "Label Deleted", Data = result });


            }
            else
            {
                return BadRequest(new ResponseModel<Label>() { Success = false, Message = "Label NotDeleted", Data = result });
            }

        }

        [Authorize]
        [HttpGet]
        public IActionResult getAllLabels()
        {

            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _buseness.GetAllLabels(userid);
            if (result != null)
            {
                return Ok(new ResponseModel<IEnumerable<Label>>() { Success = true, Message = "Labels Found", Data = result });


            }
            else
            {
                return BadRequest(new ResponseModel<IEnumerable<Label>>() { Success = false, Message = "Labels Not Found", Data = result });
            }

        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllLabelsByNoteId(long noteId)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _buseness.GetAllLabelsByNoteId(userid, noteId);
            if (result != null)
            {
                return Ok(new ResponseModel<IEnumerable<Label>>() { Success = true, Message = "Labels Found", Data = result });


            }
            else
            {
                return BadRequest(new ResponseModel<IEnumerable<Label>>() { Success = false, Message = "Labels Not Found", Data = result });
            }

        }

        [Authorize]
        [HttpPost]
        public IActionResult AddLableUsingUserId( string labelName)
        {

            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _buseness.AddLableUsingUserId(userid, labelName);
            if (result != null)
            {
                return Ok(new ResponseModel<Label>() { Success = true, Message = "Labels Added", Data = result });


            }
            else
            {
                return BadRequest(new ResponseModel<Label>() { Success = false, Message = "Labels Not Added", Data = result });
            }

        }


    }
}
