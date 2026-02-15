using IncidentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IncidentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        void incerementId()
        {
            _nextId++;
        }

        private static readonly List<Incident> _incidents = new();
        private static int _nextId = 1;
        private static readonly string[] AllowedSeverities =
        { "LOW", "MEDIUM", "HIGH", "CRITICAL" };
        private static readonly string[] AllowedStatuses =
        { "OPEN", "IN_PROGRESS", "RESOLVED" };
        [HttpPost("create-incident")]
        public IActionResult CreateIncident([FromBody] Incident incident)
        {
            
            incident.Status= "OPEN";
            
            incident.Id = _nextId;
            
            _incidents.Add(incident);
            if(!AllowedSeverities.Contains(incident.Severity))
                {
                return BadRequest($"Invalid severity. Allowed values are: {string.Join(", ", AllowedSeverities)}");
            }
            incerementId();
            return Ok(incident);

        }
        [HttpGet("get-incidents")]
        public IActionResult GetAllIncidents()
        {
            return Ok(_incidents);
        }

            [HttpGet("getbyid/{id}")]
        public IActionResult GetIncidentById(int id)
        {
            try
            {
                var incident = _incidents.First(i => i.Id == id);
                return Ok(incident);
            }
            catch(Exception)
            {
                return BadRequest($"Icident inexistant");
            }
            
        }
        [HttpPut("update-status/{id}")]
        public IActionResult UpdateIncidentStatus(int id, [FromBody] string status)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);

            if (incident == null)
            {
                return NotFound($"Incident {id} n'existe pas.");
            }

            if (!AllowedStatuses.Contains(status))
            {
                return BadRequest("Statut invalide");
            }

            incident.Status = status;

            return Ok(incident);

        }
        [HttpDelete("delete-incident/{id}")]
        public IActionResult DeleteIncident(int id)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);

            if (incident != null)
            {
                if (!(incident.Status == "OPEN" && incident.Severity == "CRITICAL"))
                {
                  _incidents.Remove(incident);
                }
                return BadRequest("L'incident doit etre resolu");
            }
            return NotFound($"Incident {id} n'existe pas.");

        }
        [HttpGet("filter-by-status/status")]
        public IActionResult FilterByStatus(String stat)
        {
            List <Incident> ls = _incidents.FindAll(i=> i.Status==stat);
            if (ls.Count!= 0)
            {
                return Ok(ls);
            }
            return BadRequest($"Il n'existe aucune resultat avec le statut {stat}");
        }
        [HttpGet("filter-by-status/severity")]
        public IActionResult FilterBySeverity(String sev)
        {
            List<Incident> ls = _incidents.FindAll(i => i.Severity == sev);
            if (ls.Count != 0)
            {
                return Ok(ls);
            }
            return BadRequest($"Il n'existe aucune resultat avec la seevrité {sev}");
        }
    }
}
