using BusenessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer.ResponseModel;
using ModelLayer.ResuestModels;
using Newtonsoft.Json;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FandooNotesApp.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class Notes_Controller : ControllerBase
    {

        private readonly INotes_Buseness _buseness;
        private readonly IDistributedCache _distributedCache;
        private string keyname = "Master";
        public Notes_Controller(INotes_Buseness buseness,IDistributedCache distributedCache)
        {
            _buseness = buseness;
            _distributedCache = distributedCache;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateNotes(NotesModel request)
        {
            long userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
           // HttpContext.Session.GetInt32("UserId");

            var result = _buseness.CreateNotes(request, userid);
            if (result != null)
            {
                return Ok(new ResponseModel<string>() { Success = true, Message = "Note Created Sucessfully.." });

            }
            else
            {
                return BadRequest(new ResponseModel<string>() { Success = false, Message = "Note Not Created..!", Data = result });
            }

        }

        

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteNote(long noteid)
        {
            long userid=long.Parse(User.Claims.Where(x=>x.Type == "UserId").FirstOrDefault().Value);
            bool result=_buseness.DeleteNote(userid,noteid);
            if (result)
            {
                return Ok(new ResponseModel<bool>() { Success = true, Message = "Note Deleted Sucessfully..." });
            }
            else
            {
                return BadRequest(new ResponseModel<bool>() { Success=false,Message="Note Not deleted...!"});
            }


        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateNote(long noteid,NotesModel request)
        {
            long userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            bool result=_buseness.UpdateNote(userid, noteid, request);
            if (result)
            {
                return Ok(new ResponseModel<bool>() { Success = true, Message = "Note Updated Sucessfully" });
            }
            else
            {
                return NotFound(new ResponseModel<bool>() { Success=false,Message="Note Not found"});
            }

        }
        [Authorize]
        [HttpGet]
        public IActionResult GetNoteById(long noteId)
        {
            var userid = long.Parse(User.Claims.Where(x=>x.Type=="UserId").FirstOrDefault().Value);
            var result=_buseness.GetNoteById(noteId,userid);
            if (result == null)
            {
                return BadRequest(new ResponseModel<Notes>() {Success=false,Message="Note Not Found..,!",Data=null });
            }
            else
            {
                return Ok(new ResponseModel<Notes>() {Success=true,Message="Note found...", Data=result });
            }

        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAllNotes()
        {

            ResponseModel<object> response = new ResponseModel<object>();
            string serializelist = string.Empty;
            var EncodedList = await _distributedCache.GetAsync(keyname);
            if (EncodedList != null)
            {
               //getting from redic cache
                serializelist = Encoding.UTF8.GetString(EncodedList);
                response.Data = JsonConvert.DeserializeObject<List<Notes>>(serializelist);

                return Ok(new ResponseModel<object>() { Message = "Not found", Success = true, Data = response });


            }
            else
            {
                //data not in rediscache
                // goes to the database
               //store into the radis

                var userid = long.Parse(User.Claims.Where(y => y.Type == "UserId").First().Value);
                var result = await _buseness.GetAllNotes(userid);
                if (result == null)
                {

                    return BadRequest(new ResponseModel<object>() { Success = false, Message = "No Notes Found", Data = null });
                }
                else
                {
                    //storing to cache
                    serializelist = JsonConvert.SerializeObject(result);
                    EncodedList=Encoding.UTF8.GetBytes(serializelist);
                    var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20)).SetAbsoluteExpiration(TimeSpan.FromHours(6));
                    await _distributedCache.SetAsync(keyname,EncodedList, option);

                    return Ok(new ResponseModel<object>() { Message = "Notes Found", Success = true, Data = result });
                }



            }


           

        }


        [Authorize]
        [HttpGet]
        public IActionResult GetNotesByDate(DateTime date)
        {
            var userid = long.Parse(User.Claims.Where(y => y.Type == "UserId").First().Value);
           var result= _buseness.GetNotesByDate(userid, date);

            if(result != null)
            {
                return Ok(new ResponseModel<IEnumerable<Notes>>() { Success=true,Message="Getting allnotes by date",Data=result});

            }
            else
            {
                return BadRequest(new ResponseModel<IEnumerable<Notes>>() { Success = false,Message="No notes Remaindon this date",Data= null });
            }
            
            

        }


        [Authorize]
        [HttpPost]
        public  IActionResult AddColor(long noteId, string color)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result=_buseness.AddColor(userid, noteId, color);
            if (result != null)
            {
                return Ok(new ResponseModel<Notes>() { Success = true, Message = "Color added...", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<Notes>() { Success = false, Message = "Color not added", Data = result });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddReminder(long noteId, DateTime reminder)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result=_buseness.AddReminder(userid, noteId, reminder);
            if (result != null)
            {
                return Ok(new ResponseModel<Notes>() {Success=true,Message="reminder added..",Data=result });
            }
            else
            {
                return BadRequest(new ResponseModel<Notes>() { Success=false, Message ="Some thing went wrong..!",Data= result});
            }
        }


        [Authorize]
        [HttpPut]
        public IActionResult ToggleTrash(int noteId)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _buseness.ToggleTrash(userid, noteId);
            if (result != null)
            {
                return Ok(new ResponseModel<Notes>() { Success = true, Message = "Toggled...", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<Notes>() { Success = false, Message = "Not toggled note not found", Data = result });
            }


        }

        [Authorize]
        [HttpPost]
        public IActionResult TogglePin(long noteId)
        {

            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _buseness.TogglePin(userid, noteId);
            if (result != null)
            {
                return Ok(new ResponseModel<Notes>() { Success = true, Message = "Pinned..", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<Notes>() { Success = false, Message = "Some thing went wrong..!", Data = result });
            }

        }


        [Authorize]
        [HttpPut]
        public IActionResult ToggleArchive(long noteId)
        {

            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _buseness.ToggleArchive(userid, noteId);
            if (result != null)
            {
                return Ok(new ResponseModel<Notes>() { Success = true, Message = "Archived..", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<Notes>() { Success = false, Message = "Some thing went wrong..!", Data = result });
            }

        }


        [Authorize]
        [HttpPost]
        public IActionResult ImageAdd(int noteId, IFormFile imageUrl)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _buseness.AddImage(userid, noteId, imageUrl);
            if (result != null)
            {
                return Ok(new ResponseModel<Notes>() { Success = true, Message = "Image addedsucessfully..", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<Notes>() { Success = false, Message = "Some thing went wrong..!", Data = result });
            }

        }

        [HttpPost]
        public IActionResult AddReview(ReviewModel request)
        {
            var result=_buseness.AddReview(request);
            if (result)
            {
                return Ok(new ResponseModel<Notes>() { Success = true, Message = "Review Added" });
            }
            else
            {
                return BadRequest(new ResponseModel<Notes>() { Success=false,Message="Something went wrong"});
            }
        }

        [HttpGet]
        public IActionResult GetAllReview()
        {
            var result=_buseness.FetchAllReview();
            if(result != null)
            {
                return Ok(new ResponseModel<IEnumerable<Review>> {Message="Data found",Success=true ,Data=result});
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Message="Data not found",Success=false,Data=null});
            }
        }

    }
}
