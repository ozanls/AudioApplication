using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AudioApplication.Models;
using System.Diagnostics;

namespace AudioApplication.Controllers
{
    public class AudioDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AudioData/ListAudios
        [HttpGet]
        public IEnumerable<AudioDto> ListAudios()
        {
            List<Audio> Audios = db.Audios.ToList();
            List<AudioDto> AudioDtos = new List<AudioDto>();

            Audios.ForEach(a => AudioDtos.Add(new AudioDto
            {
                AudioId = a.AudioId,
                AudioName = a.AudioName,
                AudioURL = a.AudioURL,
                AudioLength = a.AudioLength,
                AudioTimestamp = a.AudioTimestamp,
                AudioStreams = a.AudioStreams,
                AudioUploaderId = a.AudioUploaderId,
                CategoryName = a.Category.CategoryName
            }));
            return AudioDtos;
        }

        // GET: api/AudioData/FindAudio/5
        [ResponseType(typeof(Audio))]
        [HttpGet]
        public IHttpActionResult FindAudio(int id)
        {
            Audio Audio = db.Audios.Find(id);
            AudioDto AudioDto = new AudioDto()
            {
                AudioId = Audio.AudioId,
                AudioName = Audio.AudioName,
                AudioURL = Audio.AudioURL,
                AudioLength = Audio.AudioLength,
                AudioTimestamp = Audio.AudioTimestamp,
                AudioStreams = Audio.AudioStreams,
                AudioUploaderId = Audio.AudioUploaderId,
                CategoryName = Audio.Category.CategoryName
            };
            if (Audio == null)
            {
                return NotFound();
            }

            return Ok(AudioDto);
        }

        // POST: api/AudioData/UpdateAudio/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAudio(int id, Audio audio)
        {
            Debug.WriteLine("I have reached the audio update method");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");

                return BadRequest(ModelState);
            }

            if (id != audio.AudioId)
            {
                Debug.WriteLine("Id is invalid");
                Debug.WriteLine("GET Parameter: " + id);
                Debug.WriteLine("POST Parameter: " + audio.AudioId);
                return BadRequest();
            }

            db.Entry(audio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AudioExists(id))
                {
                    Debug.WriteLine("Audio not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AudioData/AddAudio
        [ResponseType(typeof(Audio))]
        [HttpPost]
        public IHttpActionResult AddAudio(Audio audio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Audios.Add(audio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = audio.AudioId }, audio);
        }

        // POST: api/AudioData/DeleteAudio/5
        [ResponseType(typeof(Audio))]
        [HttpPost]
        public IHttpActionResult DeleteAudio(int id)
        {
            Audio audio = db.Audios.Find(id);
            if (audio == null)
            {
                return NotFound();
            }

            db.Audios.Remove(audio);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AudioExists(int id)
        {
            return db.Audios.Count(e => e.AudioId == id) > 0;
        }
    }
}